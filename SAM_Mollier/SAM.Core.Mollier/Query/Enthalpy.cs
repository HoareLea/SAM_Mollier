﻿using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates enthalpy from dry bulb temperature and humidity ratio.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Enthalpy [J/kg]</returns>
        public static double Enthalpy(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            if(dryBulbTemperature < -50 || dryBulbTemperature > 100)
            {
                return double.NaN;
            }

            double saturationHumidityRatio = SaturationHumidityRatio(dryBulbTemperature, pressure);
            if(double.IsNaN(saturationHumidityRatio))
            {
                return double.NaN;
            }

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000; //[kJ/kg]
            double specificHeat_Air = Zero.SpecificHeat_Air / 1000; //[kJ/kg*K]
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000; //[kJ/kg*K]
            double specificHeat_Water = Zero.SpecificHeat_Water / 1000; //[kJ/kg*K]
            double specificHeat_Ice = Zero.SpecificHeat_Ice / 1000; //[kJ/kg*K]
            double meltingHeat_Ice = Zero.MeltingHeat_Ice / 1000; //[kJ/kg*K]

            double result = double.NaN;

            if (humidityRatio > saturationHumidityRatio)
            {
                if(dryBulbTemperature > 0)
                {
                    //Flussigkeitsnebel
                    result = specificHeat_Air * dryBulbTemperature + saturationHumidityRatio * (vapourizationLatentHeat + specificHeat_WaterVapour * dryBulbTemperature) + ((humidityRatio - saturationHumidityRatio) * specificHeat_Water * dryBulbTemperature);//Glueck 2.31
                }
                else
                {
                    //Eisnebel
                    result = specificHeat_Air * dryBulbTemperature + saturationHumidityRatio * (vapourizationLatentHeat + specificHeat_WaterVapour * dryBulbTemperature) + (humidityRatio - saturationHumidityRatio) * (-meltingHeat_Ice + specificHeat_Ice * dryBulbTemperature); //Glueck 2.36
                }
            }
            else
            {
                //Feutcht Luft
                result = specificHeat_Air * dryBulbTemperature + humidityRatio * (vapourizationLatentHeat + specificHeat_WaterVapour * dryBulbTemperature); //Glueck 2.27 or From Recknagel Sprenger 07/08 page 133
            }

            if(double.IsNaN(result))
            {
                return result;
            }

            return result * 1000;
        }
        /// <summary>
        /// Calculates enthalpy from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="relativeHumidity">Relative humidity [%]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Enthalpy [J/kg]</returns>
        public static double Enthalpy_ByRelativeHumidity(double dryBulbTemperature, double relativeHumidity, double pressure, bool allowRH100 = false)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            if(dryBulbTemperature < -50)
            {
                return double.NaN;
                //'it is too cold'
            }

            if(dryBulbTemperature < 100)
            {
                double humidityRatio = HumidityRatio(dryBulbTemperature, relativeHumidity, pressure, allowRH100);
                if(!double.IsNaN(humidityRatio))
                {
                    return Enthalpy(dryBulbTemperature, humidityRatio, pressure);
                }
            }

            return double.NaN;
        }

        /// <summary>
        /// Calculates enthalpy for end point for given sensible heat ratio
        /// </summary>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <param name="sensibleHeatRatio">Sensible Heat Ratio (SHR) [-] value from 0 to 1</param>
        /// <param name="specificHeat">Air Specific Heat [J/kg*K]</param>
        /// <param name="dryBulbTemperature_Start">Start Dry Bulb Tempearture [C]</param>
        /// <param name="dryBulbTemperature_End">End Dry Bulb Tempearture [C]</param>
        /// <param name="enthalpy_Start">Start Enthalpy [J/kg]</param>
        /// <returns>End Enthalpy [J/kg]</returns>
        public static double Enthalpy_BySensibleHeatRatioAndDryBulbTemperature(double pressure, double sensibleHeatRatio, double specificHeat, double dryBulbTemperature_Start, double dryBulbTemperature_End, double enthalpy_Start)
        {
            if(double.IsNaN(sensibleHeatRatio) || double.IsNaN(specificHeat) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(dryBulbTemperature_End) || double.IsNaN(enthalpy_Start))
            {
                return double.NaN;
            }

            if (sensibleHeatRatio == 0 || (dryBulbTemperature_End - dryBulbTemperature_Start) == 0)
            {
                return double.NaN;
            }

            if (sensibleHeatRatio == 1)
            {
                double humidityRatio_Start = HumidityRatio_ByEnthalpy(dryBulbTemperature_Start, enthalpy_Start);

                return Enthalpy(dryBulbTemperature_End, humidityRatio_Start, pressure);
            }

            return enthalpy_Start - (specificHeat * (dryBulbTemperature_Start - dryBulbTemperature_End) / sensibleHeatRatio);
            //return enthalpy_Start - (1050 * (dryBulbTemperature_Start - dryBulbTemperature_End) / sensibleHeatRatio);
        }

        public static double Enthalpy_BySensibleHeatRatioAndDryBulbTemperature(double pressure, double sensibleHeatRatio, double specificHeat, MollierPoint mollierPoint_Start, double dryBulbTemperature_End)
        {
            if(mollierPoint_Start == null)
            {
                return double.NaN;
            }

            return Enthalpy_BySensibleHeatRatioAndDryBulbTemperature(pressure, sensibleHeatRatio, specificHeat, mollierPoint_Start.DryBulbTemperature, dryBulbTemperature_End, mollierPoint_Start.Enthalpy);
        }

        public static double Enthalpy_BySensibleHeatRatioAndEnthalpy(double sensibleHeatRatio, double specificHeat, double dryBulbTemperature_Start, double enthalpy_End, double enthalpy_Start)
        {
            if (double.IsNaN(sensibleHeatRatio) || double.IsNaN(specificHeat) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(enthalpy_End) || double.IsNaN(enthalpy_Start))
            {
                return double.NaN;
            }

            if(specificHeat == 0 || sensibleHeatRatio == 1)
            {
                return double.NaN;
            }

            return dryBulbTemperature_Start - (sensibleHeatRatio * (enthalpy_Start - enthalpy_End) / specificHeat);
        }

        public static double Enthalpy_BySensibleHeatRatioAndEnthalpy(double sensibleHeatRatio, double specificHeat, MollierPoint mollierPoint_Start, double enthalpy_End)
        {
            if (mollierPoint_Start == null)
            {
                return double.NaN;
            }

            return Enthalpy_BySensibleHeatRatioAndEnthalpy(sensibleHeatRatio, specificHeat, mollierPoint_Start.DryBulbTemperature, enthalpy_End, mollierPoint_Start.Enthalpy);
        }

        /// <summary>
        /// Calculates enthalpy of saturated steam from dry bulb temperature.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C] — measured by a standard thermometer, unaffected by moisture (not wet-bulb or dew point).</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Enthalpy h" [kJ/kg]</returns>
        public static double Enthalpy_SaturatedSteam_ByTemperature(double dryBulbTemperature)
        {
            if (double.IsNaN(dryBulbTemperature) )
            {
                return double.NaN;
            }

            if (dryBulbTemperature < 0)
            {
                return double.NaN;
                //'it is too cold'
            }

            if (dryBulbTemperature >= 0) //from 10C to 200C max 0.02% (Gluck 1.43)
            {
                return (2.501482e3 + 1.789736 * dryBulbTemperature + 8.957546e-4 * Math.Pow(dryBulbTemperature, 2) - 1.300254e-5 * Math.Pow(dryBulbTemperature, 3));
            }

            return double.NaN;
        }
    }
}
