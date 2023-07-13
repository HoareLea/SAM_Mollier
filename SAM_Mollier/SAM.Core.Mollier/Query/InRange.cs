namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool InRange(this MollierPoint mollierPoint, Range<double> humidityRatioRange, Range<double> dryBulbTemperatureRange)
        {
            if (humidityRatioRange == null || dryBulbTemperatureRange == null || mollierPoint == null || double.IsNaN(humidityRatioRange.Min) || double.IsNaN(humidityRatioRange.Max) || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max))
            {
                return false ;
            }

            return humidityRatioRange.In(mollierPoint.HumidityRatio) && dryBulbTemperatureRange.In(mollierPoint.DryBulbTemperature);
        }
    }
}
