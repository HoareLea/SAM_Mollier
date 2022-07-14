namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates saturation Humidity Ratio from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Density [kg/m3]</returns>
        public static double SaturationHumidityRatio(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            return 0.6222 * saturationVapourPressure / (pressure - saturationVapourPressure); //by Gluck
        }
    }
}
