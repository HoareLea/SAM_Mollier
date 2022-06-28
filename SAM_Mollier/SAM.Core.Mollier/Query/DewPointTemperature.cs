namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates dew point temperature from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="relativeHumidity">Relative humidity (0 - 100)[%]</param>
        /// <returns>Dew point temperature ttau [°C]</returns>
        public static double DewPointTemperature(double dryBulbTemperature, double relativeHumidity)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature, relativeHumidity / 100);
            if(double.IsNaN(saturationVapourPressure))
            {
                return double.NaN;
            }

            double v = System.Math.Log10(saturationVapourPressure / 6.1078);
            if(double.IsNaN(v))
            {
                return double.NaN;
            }

            double a = dryBulbTemperature >= 0 ? 7.5 : 7.6;
            double b = dryBulbTemperature >= 0 ? 237.3 : 240.7;

            double result = b * v / (a - v);
            if(result > 100)
            {
                return double.NaN;
            }


            return result;
        }
    }
}
