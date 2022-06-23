using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Partial Vapour Pressure [Pa] for given dry-bulb temperature.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C]</param>
        /// <returns>Partial Vapour Pressure [Pa]</returns>
        public static double PartialVapourPressure(double dryBulbTemperature)
        {
            if (double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            if (dryBulbTemperature < 0)
            {
                return Math.Pow( 4.689 * (1.486 + dryBulbTemperature / 100), 12.3);
            }

            if (dryBulbTemperature <= 100)
            {
                return 611.213 * Math.Exp(0.07257 * dryBulbTemperature - 0.0002937 * Math.Pow(dryBulbTemperature, 2) + 0.000000981 * Math.Pow(dryBulbTemperature, 3) - 0.000000001901 * Math.Pow(dryBulbTemperature, 4));
            }

            return double.NaN;
        }
    }
}
