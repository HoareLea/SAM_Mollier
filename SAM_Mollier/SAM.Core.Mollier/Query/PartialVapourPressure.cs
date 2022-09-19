namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Partial Vapour Pressure pW [Pa] for given dry-bulb temperature and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C]</param>
        /// <param name="relativeHumidity">Relative Humidity (0 - 100) [%]</param>
        /// <returns>Partial Vapour Pressure [Pa]</returns>
        public static double PartialVapourPressure(double dryBulbTemperature, double relativeHumidity)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity))
            {
                return double.NaN;
            }

            if(relativeHumidity == 0)
            {
                return 0;
            }

            return SaturationVapourPressure(dryBulbTemperature) * relativeHumidity / 100;
        }

        /// <summary>
        /// Partial Vapour Pressure pW [Pa] for given humidity ratio and pressure.
        /// </summary>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
        /// <returns>Partial Vapour Pressure [Pa]</returns>
        public static double PartialVapourPressure_ByHumidityRatio(double humidityRatio, double pressure)
        {
            return (humidityRatio / (0.6222 + humidityRatio)) * pressure;
        }

        public static double PartialVapourPressure(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return PartialVapourPressure(mollierPoint.DryBulbTemperature, mollierPoint.RelativeHumidity);
        }
    }
}
