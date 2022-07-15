namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates saturation temperature (relative humidity 100%) from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Saturation Temperature (for relative humidity 100%)[°C]</returns>
        public static double SaturationTemperature(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double humidityRatio = SaturationHumidityRatio(dryBulbTemperature, pressure);
            if(double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            return DryBulbTemperature_ByHumidityRatio(humidityRatio, 100, pressure);

        }

        /// <summary>
        /// Calculates saturation temperature (relative humidity 100%) for given MollierPoint.
        /// </summary>
        /// <param name="mollierPoint">MollierPoint</param>
        /// <returns>Saturation Temperature (for relative humidity 100%)[°C]</returns>
        public static double SaturationTemperature(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return SaturationTemperature(mollierPoint.DryBulbTemperature, mollierPoint.Pressure);
        }
    }
}
