namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Projects mollierPoint on line created by start and end MollierPoints
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="mollierPoint"></param>
        /// <returns>MollierPoint</returns>
        public static MollierPoint Project(this MollierPoint start, MollierPoint end, MollierPoint mollierPoint)
        {
            if (start == null || end == null || mollierPoint == null)
            {
                return null;
            }

            if (start.DryBulbTemperature == end.DryBulbTemperature)
                return new MollierPoint(start.DryBulbTemperature, mollierPoint.HumidityRatio, start.Pressure);

            double m = (end.HumidityRatio - start.HumidityRatio) / (end.DryBulbTemperature - start.DryBulbTemperature);
            double b = start.HumidityRatio - (m * start.DryBulbTemperature);

            double dryBulbTemperature = (m * mollierPoint.HumidityRatio + mollierPoint.DryBulbTemperature - m * b) / (m * m + 1);
            double humidityRatio = (m * m * mollierPoint.HumidityRatio + m * mollierPoint.DryBulbTemperature + b) / (m * m + 1);

            return new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);

        }
    }
}
