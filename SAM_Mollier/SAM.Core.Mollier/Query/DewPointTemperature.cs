namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates dew point temperature from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="relativeHumidity">Relative humidity (0 - 100)[%]</param>
        /// <param name="pressure">Pressure [Pa]</param>
        /// <returns>Dew point temperature ttau [°C]</returns>
        public static double DewPointTemperature(double dryBulbTemperature, double relativeHumidity, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            double humidityRatio = HumidityRatio( dryBulbTemperature, relativeHumidity, pressure);
            if(double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            return DewPointTemperature_ByHumidityRatio(dryBulbTemperature, humidityRatio, pressure);
        }

        public static double DewPointTemperature_ByHumidityRatio(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return double.NaN;
            }

            //double partialVapourPressure = PartialVapourPressure_ByHumidityRatio(humidityRatio, dryBulbTemperature, pressure);
            double partialVapourPressure = (humidityRatio / (0.6222 + humidityRatio)) * pressure; //Has to be separate from PartialVapourPressure_ByHumidityRatio
            if (double.IsNaN(partialVapourPressure))
            {
                return double.NaN;
            }

            return SaturationTemperature(partialVapourPressure);
        }

        public static double DewPointTemperature(this MollierPoint mollierPoint)
        {
            if(mollierPoint == null)
            {
                return double.NaN;
            }

            return DewPointTemperature_ByHumidityRatio(mollierPoint.DryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure);
        }
    }
}
