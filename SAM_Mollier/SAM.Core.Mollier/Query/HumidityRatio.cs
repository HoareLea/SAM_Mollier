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

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            if (double.IsNaN(saturationVapourPressure))
            {
                return double.NaN;
            }

            double relativeHumidity_Factor = relativeHumidity / 100;

            double result = (0.622 * relativeHumidity_Factor) / ((pressure / saturationVapourPressure) - relativeHumidity_Factor);

            double relativeHumidity_Temp = RelativeHumidity(dryBulbTemperature, result, pressure);

            double factor = 0.1;
            
            while (result > 0 && (double.IsNaN(relativeHumidity_Temp) || relativeHumidity_Temp > relativeHumidity))
            {
                result -= result * factor;
                relativeHumidity_Temp = RelativeHumidity(dryBulbTemperature, result, pressure);
            }

            for(int i = 0; i < 3; i++)
            {
                factor /= 10;

                while (!double.IsNaN(relativeHumidity_Temp) && relativeHumidity_Temp < relativeHumidity)
                {
                    result += result * factor;
                    relativeHumidity_Temp = RelativeHumidity(dryBulbTemperature, result, pressure);
                }

                while (result > 0 && (double.IsNaN(relativeHumidity_Temp) || relativeHumidity_Temp > relativeHumidity))
                {
                    result -= result * factor;
                    relativeHumidity_Temp = RelativeHumidity(dryBulbTemperature, result, pressure);
                }
            }

            return result;
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
            double specificHeat_Air = Zero.SpecificHeat_Air / 1000;

            return ((enthalpy / 1000) - (specificHeat_Air * dryBulbTemperature)) / (( specificHeat_WaterVapour * dryBulbTemperature) + vapourizationLatentHeat);

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

        public static double HumidityRatio_ByHumidityRatio(double humidityRatio, double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double partialVapourPressure = PartialVapourPressure_ByHumidityRatio(humidityRatio, dryBulbTemperature, pressure);
            if(double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            double partialDryAirPressure = PartialDryAirPressure(pressure, partialVapourPressure);

            return 0.622 * partialVapourPressure / partialDryAirPressure;
        }

        /// <summary>
        /// Calculates dry bulb temperature
        /// </summary>
        /// <param name="mollierPoint">MollierPoint</param>
        /// <param name="latentLoad">Latent Load [W]</param>
        /// <param name="airMassFlow">Air Mass Flow [kg/s]</param>
        /// <returns></returns>
        public static double HumidityRatio(this MollierPoint mollierPoint, double latentLoad, double airMassFlow)
        {
            if (mollierPoint == null || double.IsNaN(latentLoad) || double.IsNaN(airMassFlow))
            {
                return double.NaN;
            }

            return mollierPoint.HumidityRatio + ((latentLoad /1000) / (airMassFlow * 2450));  //latent heat of vaporization r0 for 20degC TO DO replce with formula 
        }

        /// <summary>
        /// Calculates humidity ratio by partial vapour pressure  
        /// </summary>
        /// <param name="partialVapourPressure">Partial Vapour Pressure [Pa]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns></returns>
        public static double HumidityRatio_ByPartialVapourPressure(double partialVapourPressure, double pressure)
        {
            if(double.IsNaN(partialVapourPressure) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            return 0.6222 * partialVapourPressure / (pressure - partialVapourPressure);
        }

    }
}
