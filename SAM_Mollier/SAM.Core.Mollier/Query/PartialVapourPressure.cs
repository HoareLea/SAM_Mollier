﻿using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Partial Vapour Pressure [Pa] for given dry-bulb temperature and relative humidity.
        /// </summary>
        /// <param name="dryBulbTemperature">Dry Bulb Temperature [°C]</param>
        /// <param name="relativeHumidity">Relative Humidity (0 - 100) [%]</param>
        /// <returns>Partial Vapour Pressure [Pa]</returns>
        public static double PartialVapourPressure(double dryBulbTemperature, double relativeHumidity)
        {
            return SaturationVapourPressure(dryBulbTemperature) * relativeHumidity / 100;
        }
    }
}
