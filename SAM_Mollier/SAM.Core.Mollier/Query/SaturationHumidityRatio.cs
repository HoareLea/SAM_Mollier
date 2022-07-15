namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates saturation humidity ratio (for relative humidity 100%) from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Saturation humidity ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double SaturationHumidityRatio(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature);
            return 0.6222 * saturationVapourPressure / (pressure - saturationVapourPressure); //by Gluck
        }

        /// <summary>
        /// Calculates saturation humidity ratio (for relative humidity 100%) for given MollierPoint.
        /// </summary>
        /// <param name="mollierPoint">Mollier Point</param>
        /// <returns>Saturation humidity ratio [kg_waterVapor/kg_dryAir]</returns>
        public static double SaturationHumidityRatio(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return SaturationHumidityRatio(mollierPoint.DryBulbTemperature, mollierPoint.Pressure);
        }
    }
}
