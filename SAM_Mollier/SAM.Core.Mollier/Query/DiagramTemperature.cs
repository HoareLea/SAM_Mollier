using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates humidity ratio from dry bulb temperature, relative humidity and pressure.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry bulb temperature [°C]</param>
        /// <param name="relativeHumidity">Relative humidity (0 - 100)[%]</param>
        /// <returns>Diagram Temperature [°C]</returns>
        public static double DiagramTemperature(double dryBulbTemperature, double relativeHumidity)
        {
            if (double.IsNaN(relativeHumidity) || relativeHumidity > 100 || relativeHumidity < 0 || double.IsNaN(dryBulbTemperature))
            {
                return double.NaN;
            }

            return dryBulbTemperature + relativeHumidity / 1000 * 1.86 * dryBulbTemperature;
        }
    }
}
