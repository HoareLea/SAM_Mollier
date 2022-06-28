namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates relative humidity from dry bulb temperature, humidity ratio and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Relative Humidity (0 - 100) [%]</returns>
        public static double RelativeHumidity(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            //double humidityRatio_Temp = humidityRatio / 1000;

            double result = (humidityRatio * pressure / (0.6222 + humidityRatio)) / SaturationVapourPressure(dryBulbTemperature) * 100;

            if(result < 0  || result > 100)
            {
                result = System.Math.Round(result);
            }

            if(result < 0 || result > 100)
            {
                return double.NaN;
            }

            return result;
        }

        /// <summary>
        /// Calculates relative humidity from dry bulb temperature and dew point temperature.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="dewPointTemperature">Dew Point Temperature [°C]</param>
        /// <returns>Relative Humidity (0 - 100) [%]</returns>
        public static double RelativeHumidity_ByDewPointTemperature(double dryBulbTemperature, double dewPointTemperature)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(dewPointTemperature))
            {
                return double.NaN;
            }

            return Core.Query.Calculate((double x) => DewPointTemperature(dryBulbTemperature, x), dewPointTemperature, 0, 100);
        }

        /// <summary>
        /// Calculates relative humidity from dry bulb temperature, wet bulb temperature and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="wetBulbTemperature">Wet bulb temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Relative Humidity (0 - 100) [%]</returns>
        public static double RelativeHumidity_ByWetBulbTemperature(double dryBulbTemperature, double wetBulbTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(wetBulbTemperature))
            {
                return double.NaN;
            }

            return Core.Query.Calculate((double x) => WetBulbTemperature(dryBulbTemperature, x, pressure), wetBulbTemperature, 0, 100);
        }
    }
}
