using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates new end point by given start, end and efficiency in geometry way.
        /// </summary>
        /// <param name="start">Starting mollier point</param>
        /// <param name="end">Ending mollier point</param>
        /// <param name="efficiency">Process efficiency 0-1</param>
        /// <returns>New ending mollier point</returns>
        public static MollierPoint EndByEfficiency(MollierPoint start, MollierPoint end, double efficiency = 1)
        {
            double humidityRatioDifference = end.HumidityRatio - start.HumidityRatio;
            double dryBulbTemperatureDifference = end.DryBulbTemperature - start.DryBulbTemperature;

            double newHumidityRatio = start.HumidityRatio + humidityRatioDifference * efficiency;
            double newDryBulbTemperature = start.DryBulbTemperature + dryBulbTemperatureDifference * efficiency;

            return new MollierPoint(newDryBulbTemperature, newHumidityRatio, end.Pressure);
        }
    }
}
