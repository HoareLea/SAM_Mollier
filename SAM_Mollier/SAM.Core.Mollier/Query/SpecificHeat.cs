using System;
using System.IO;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Specific Heat for Dry Air (cp) [J/kg*K] for pressure p=0.1MPa for temperature from -20C to 200C, Max 0.05%
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [C]</param>
        /// <returns>Dry Air Specific Heat (cp) [J/kg*K]</returns>
        public static double SpecificHeat_Air(double dryBulbTemperature)
        {
            if(double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            //double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            //return 1.9327e-10 * Math.Pow(dryBulbTemperature_Kelvin, 4) - 7.9999e-7 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 1.1407e-3* Math.Pow(dryBulbTemperature_Kelvin, 2) - 4.4890e-1 * dryBulbTemperature_Kelvin + 1.0575e3;
            double result = 1.0065 + 5.309587e-6 * dryBulbTemperature + 4.758596e-7 * Math.Pow(dryBulbTemperature, 2) - 1.136145e-10 * Math.Pow(dryBulbTemperature, 3);  //Gluck (2.38)
            return result * 1000;
        }

        /// <summary>
        /// Calculates Specific Heat for Moist Air [J/kg*K] by Gluck (2.46)
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temparture [C]</param>
        /// <param name="humidtyRatio">Humidty Ratio [kg/kg]</param>
        /// <returns>Moist Air Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_MoistAir(double dryBulbTemperature, double humidtyRatio)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(humidtyRatio))
            {
                return double.NaN;
            }

            double specificHeat_Air = SpecificHeat_Air(dryBulbTemperature);
            if(double.IsNaN(specificHeat_Air))
            {
                return double.NaN;
            }

            if(humidtyRatio == 0)
            {
                return specificHeat_Air;
            }

            double specificHeat_WaterVapour = SpecificHeat_Air(dryBulbTemperature);
            if (double.IsNaN(specificHeat_Air))
            {
                return double.NaN;
            }

            return (specificHeat_Air + humidtyRatio * specificHeat_WaterVapour) / (1 + humidtyRatio); //Gluck (2.46)

        }

        /// <summary>
        /// Calculates Specific Heat for Water Vapour (cpw) [J/kg*K] Gluck (3.52) temperature range from 25C to 400C Max error 0.04% (at 0C 0.11%)
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [C]</param>
        /// <returns>Water Vapour Specific Heat (cpw) [J/kg*K]</returns>
        public static double SpecificHeat_WaterVapour(double dryBulbTemperature)
        {
            if (double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }
            double result =  1.863 + 2.680862e-4 * dryBulbTemperature + 6.794704e-7 * Math.Pow(dryBulbTemperature, 2) - 2.641422e-10 * Math.Pow(dryBulbTemperature, 3);
            //double result = 9e-12 * Math.Pow(dryBulbTemperature, 4) - 9e-8 * Math.Pow(dryBulbTemperature, 3) + 0.0002 * Math.Pow(dryBulbTemperature, 2) - 0.5115 * dryBulbTemperature + 1692.7;
            return result * 1000;
        }

        /// <summary>
        /// Calculates Specific Heat for Water (cw) [J/kg*K] by Gluck (1.19) for temperature range from 30 to 200. Erroe 0.02%
        /// </summary>
        /// <param name="temperature">Temparture [C]</param>
        /// <returns>Water Specific Heat (cw) [J/kg*K]</returns>
        public static double SpecificHeat_Water(double temperature)
        {
            if (double.IsNaN(temperature))
            {
                return double.NaN;
            }
            double result = 4.177375 - 2.144614 * Math.Pow(temperature, 6) - 3.165823e-7 * Math.Pow(temperature, 2) + 4.134309e-8 * Math.Pow(temperature, 3);
            //double result = 5e-07*Math.Pow(dryBulbTemperature, 4) - 0.0002 * Math.Pow(dryBulbTemperature, 3) + 0.0328 * Math.Pow(dryBulbTemperature, 2) - 1.99 * dryBulbTemperature + 4215.6;
            return result * 1000;
        }

        /// <summary>
        /// Calculate Specific Heat of the Air at MollierPoint
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <returns></returns>
        public static double SpecificHeat(this MollierPoint mollierPoint)
        {
            return SpecificHeat_MoistAir(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio);
        }
    }
}
