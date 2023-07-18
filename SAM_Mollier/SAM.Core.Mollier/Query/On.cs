using System;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool On(this MollierPoint start, MollierPoint end, MollierPoint mollierPoint, double tolerance = Tolerance.Distance)
        {
            if (start == null || end == null || mollierPoint == null)
            {
                return false;
            }

            MollierPoint closest = Closest(start, end, mollierPoint, true);
            if (closest == null)
            {
                return false;
            }

            double humidityRatioDifference = Math.Abs(closest.HumidityRatio - mollierPoint.HumidityRatio) * 1000;
            double dryBulbTemperatureDifference = Math.Abs(closest.DryBulbTemperature - mollierPoint.DryBulbTemperature);
        
            double distance = Math.Sqrt((humidityRatioDifference * humidityRatioDifference) + (dryBulbTemperatureDifference * dryBulbTemperatureDifference));

            return distance < tolerance;
        }
    }
}
