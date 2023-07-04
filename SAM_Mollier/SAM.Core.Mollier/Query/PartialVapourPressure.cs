namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Partial Vapour Pressure pW [Pa] for given dry-bulb temperature and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C]</param>
        /// <param name="relativeHumidity">Relative Humidity (0 - 100) [%]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Partial Vapour Pressure [Pa]</returns>
        public static double PartialVapourPressure(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity))
            {
                return double.NaN;
            }

            if(relativeHumidity == 0)
            {
                return 0;
            }

            double result = SaturationVapourPressure(dryBulbTemperature) * relativeHumidity / 100;
            if(double.IsNaN(result))
            {
                double humidityRatio = HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                if (double.IsNaN(humidityRatio))
                {
                    return double.NaN;
                }

                return PartialVapourPressure_ByHumidityRatio(humidityRatio, pressure);
            }

            return result;
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

            return PartialVapourPressure(mollierPoint.DryBulbTemperature, mollierPoint.RelativeHumidity, mollierPoint.Pressure);
        }
    }
}
