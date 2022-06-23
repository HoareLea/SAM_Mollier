namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates density from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="relativeHumidity">Relative humidity (0 - 100) [%]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Density [kg/m3]</returns>
        public static double Density(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature, relativeHumidity);
            if (double.IsNaN(saturationVapourPressure))
            {
                return double.NaN;
            }

            //saturationVapourPressure = saturationVapourPressure * relativeHumidity / 100;

            return 1 / 287.1 * pressure / (273.15 + dryBulbTemperature) + 1 / 461.4 * saturationVapourPressure / (273.15 + dryBulbTemperature);
        }

        /// <summary>
        /// Calculates density from dry bulb temperature, humidity ratio and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Density [kg/m3]</returns>
        public static double Density_ByHumidityRatio(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            //double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            double partialVapourPressure = PartialVapourPressure(dryBulbTemperature);

            return 0.00348 * pressure / (273.15 + dryBulbTemperature) - 0.00217 * (partialVapourPressure * ((((humidityRatio * pressure / (0.6222 + humidityRatio)) / (partialVapourPressure))))) / (273.15 + dryBulbTemperature);
        }

        /// <summary>
        /// Calculates density from pressure.
        /// </summary>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <param name="dryBulbTemperature_In">Dry bulb temperature in [°C]</param>
        /// <param name="relativeHumidity_In">Relative humidity in (0 - 100) [%]</param>
        /// <param name="dryBulbTemperature_Out">Dry bulb temperature out [°C]</param>
        /// <param name="relativeHumidityOut">Relative humidity out (0 - 100) [%]</param>
        /// <returns>Density [kg/m3]</returns>
        public static double Density(double pressure, double dryBulbTemperature_In, double relativeHumidity_In, double dryBulbTemperature_Out, double relativeHumidityOut)
        {
            double density_In = Density(dryBulbTemperature_In, relativeHumidity_In, pressure);
            double density_Out = Density(dryBulbTemperature_Out, relativeHumidityOut, pressure);

            return density_In + (density_Out - density_In) / 2;
        }
    }
}
