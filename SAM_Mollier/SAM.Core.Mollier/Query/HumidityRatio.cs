namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates humidity ratio from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="relativeHumidity">Relative humidity (0 - 100)[%]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Humidity Ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double HumidityRatio(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if (double.IsNaN(relativeHumidity) || relativeHumidity > 100 || relativeHumidity < 0 || double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                //if RelativeHumidity is greater than 100 then mist
                return double.NaN;
            }

            double partialVapourPressure = PartialVapourPressure(dryBulbTemperature, relativeHumidity);
            if(double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            //double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            return  0.6222 * partialVapourPressure / (pressure - partialVapourPressure);

            //return result;
            //saturationVapourPressure = saturationVapourPressure * relativeHumidity / 100;
            //return 0.6222 * partialVapourPressure / (pressure - partialVapourPressure); //from Recknagel Sprenger
        }

        /// <summary>
        /// Calculates humidity ratio from dry bulb temperature and enthalpy.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="enthalpy">Moist air Enthalpy[J / kg]</param>
        /// <returns>Humidity Ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double HumidityRatio_ByEnthalpy(double dryBulbTemperature, double enthalpy)
        {
            if(double.IsNaN(enthalpy) || double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000;
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;

            return ((enthalpy / 1000) - dryBulbTemperature) / (vapourizationLatentHeat + specificHeat_WaterVapour * dryBulbTemperature);

            //MD 2023-06-29
            //double vapourizationLatentHeat = Core.Query.VapourizationLatentHeat(dryBulbTemperature);
            //if(double.IsNaN(vapourizationLatentHeat))
            //{
            //    vapourizationLatentHeat = 2501;
            //}

            //return ((enthalpy / 1000) - dryBulbTemperature) / (vapourizationLatentHeat + 1.86 * dryBulbTemperature);
        }

        /// <summary>
        /// Calculates humidity ratio from dry bulb temperature and enthalpy.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="wetBulbTemperature">Wet Bulb Temperature [°C]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Humidity Ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double HumidityRatio_ByWetBulbTemperature(double dryBulbTemperature, double wetBulbTemperature, double pressure)
        {
            if (double.IsNaN(wetBulbTemperature) || double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double pressureRatio = 0.621945 * SaturationVapourPressure(wetBulbTemperature) / (pressure - SaturationVapourPressure(wetBulbTemperature));

            if (wetBulbTemperature >= 0)
            {
                return ((2501 - 2.326 * wetBulbTemperature) * pressureRatio - 1.006 * (dryBulbTemperature - wetBulbTemperature)) / (2501 + 1.86 * dryBulbTemperature - 4.186 * wetBulbTemperature);
            }

            return ((2830 - 0.24 * wetBulbTemperature) * pressureRatio - 1.006 * (dryBulbTemperature - wetBulbTemperature)) / (2830 + 1.86 * dryBulbTemperature - 2.1 * wetBulbTemperature);
        }

        /// <summary>
        /// Calculates dry bulb temperature
        /// </summary>
        /// <param name="mollierPoint">MollierPoint</param>
        /// <param name="latentLoad">Latent Load [W]</param>
        /// <param name="airFlow">Air Flow [m3/s]</param>
        /// <returns></returns>
        public static double HumidityRatio(this MollierPoint mollierPoint, double latentLoad, double airFlow)
        {
            if (mollierPoint == null || double.IsNaN(latentLoad) || double.IsNaN(airFlow))
            {
                return double.NaN;
            }

            return mollierPoint.HumidityRatio + ((latentLoad /1000) / (airFlow * mollierPoint.Density() * 2450));
        }

        /// <summary>
        /// Calculates humidity ratio for end point for given senisble heat ratio and start point humidity ratio
        /// </summary>
        /// <param name="sensibleHeatRatio">Sensible Heat Ratio (SHR) [-] value from 0 to 1</param>
        /// <param name="specificHeat">Air Specific Heat [J/kg*K]</param>
        /// <param name="dryBulbTemperature_Start">Start Dry Bulb Tempearture [C]</param>
        /// <param name="dryBulbTemperature_End">End Dry Bulb Tempearture [C]</param>
        /// <param name="humidityRatio_Start">Start Point Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <returns>End Point Humidity Ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double HumidityRatio_BySensibleHeatRatioAndHumidityRatio(double sensibleHeatRatio, double specificHeat, double dryBulbTemperature_Start, double dryBulbTemperature_End, double humidityRatio_Start)
        {
            if (double.IsNaN(sensibleHeatRatio) || double.IsNaN(specificHeat) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(dryBulbTemperature_End) || double.IsNaN(humidityRatio_Start))
            {
                return double.NaN;
            }

            double specificHeat_Air = specificHeat / 1000; //[kJ/kg*K]
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000; //[kJ/kg*K]

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000; //[kJ/kg]

            double divisor = specificHeat_WaterVapour * dryBulbTemperature_End + vapourizationLatentHeat;
            if(divisor == 0)
            {
                return double.NaN;
            }

            double dividend = (specificHeat_Air * (dryBulbTemperature_End - dryBulbTemperature_Start) / sensibleHeatRatio) + (specificHeat_Air * (dryBulbTemperature_Start - dryBulbTemperature_End)) + (humidityRatio_Start * (specificHeat_WaterVapour * dryBulbTemperature_Start + vapourizationLatentHeat));
            
            return dividend / divisor;
        }
    }
}
