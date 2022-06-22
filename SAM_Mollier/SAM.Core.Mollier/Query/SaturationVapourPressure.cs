using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Saturation Vapour Pressure [Pa] for given dry-bulb temperature.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C]</param>
        /// <param name="relativeHumidity">Relative Humidity (0 - 100) [%]</param>
        /// <returns>Saturation Vapour Pressure [Pa]</returns>
        public static double SaturationVapourPressure(double dryBulbTemperature, double relativeHumidity)
        {
            return VapourPressure(dryBulbTemperature) * relativeHumidity / 100;
        }
    }
}
