using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Specific Heat for Dry Air [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <returns>Dry Air Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_Air(double dryBulbTempearture)
        {
            if(double.IsNaN(dryBulbTempearture))
            {
                return double.NaN;
            }

            double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            return 1.9327e-10 * Math.Pow(dryBulbTemperature_Kelvin, 4) - 7.9999e-7 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 1.1407e-3* Math.Pow(dryBulbTemperature_Kelvin, 2) - 4.4890e-1 * dryBulbTemperature_Kelvin + 1.0575e3;
        }

        /// <summary>
        /// Calculates Specific Heat for Moist Air [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <param name="humidtyRatio">Humidty Ratio [kg/kg]</param>
        /// <returns>Moist Air Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_MoistAir(double dryBulbTempearture, double humidtyRatio)
        {
            if (double.IsNaN(dryBulbTempearture) || double.IsNaN(humidtyRatio))
            {
                return double.NaN;
            }

            double specificHeat_Air = SpecificHeat_Air(dryBulbTempearture);
            if(double.IsNaN(specificHeat_Air))
            {
                return double.NaN;
            }

            if(humidtyRatio == 0)
            {
                return specificHeat_Air;
            }

            double specificHeat_WaterVapour = SpecificHeat_Air(dryBulbTempearture);
            if (double.IsNaN(specificHeat_Air))
            {
                return double.NaN;
            }

            return (specificHeat_Air + humidtyRatio * specificHeat_WaterVapour) / (1 + humidtyRatio);

        }

        /// <summary>
        /// Calculates Specific Heat for Water Vapour [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <returns>Water Vapour Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_WaterVapour(double dryBulbTempearture)
        {
            if (double.IsNaN(dryBulbTempearture))
            {
                return double.NaN;
            }

            double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            return 9e-12 * Math.Pow(dryBulbTemperature_Kelvin, 4) - 9e-8 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 0.0002 * Math.Pow(dryBulbTemperature_Kelvin, 2) - 0.5115 * dryBulbTemperature_Kelvin + 1692.7;
        }

        /// <summary>
        /// Calculates Specific Heat for Water [J/kg*K]
        /// </summary>
        /// <param name="dryBulbTempearture">Dry Bulb Temparture [C]</param>
        /// <returns>Water Specific Heat [J/kg*K]</returns>
        public static double SpecificHeat_Water(double dryBulbTempearture)
        {
            if (double.IsNaN(dryBulbTempearture))
            {
                return double.NaN;
            }

            double dryBulbTemperature_Kelvin = dryBulbTempearture + 273.15;

            return 5e-07*Math.Pow(dryBulbTemperature_Kelvin, 4) - 0.0002 * Math.Pow(dryBulbTemperature_Kelvin, 3) + 0.0328 * Math.Pow(dryBulbTemperature_Kelvin, 2) - 1.99 * dryBulbTemperature_Kelvin + 4215.6;
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
