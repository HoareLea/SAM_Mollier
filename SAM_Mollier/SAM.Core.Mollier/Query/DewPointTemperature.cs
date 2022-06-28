namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static double DewPointTemperature(double dryBulbTemperature, double relativeHumidity)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(relativeHumidity))
            {
                return double.NaN;
            }

            double saturationVapourPressure = SaturationVapourPressure(dryBulbTemperature, relativeHumidity);
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
