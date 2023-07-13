using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static List<MollierPoint> MollierPoints(Range<double> humidityRatioRange, Range<double> dryBulbTemperatureRange, double pressure)
        {
            if(humidityRatioRange == null || dryBulbTemperatureRange == null || double.IsNaN(pressure) || double.IsNaN(humidityRatioRange.Min) || double.IsNaN(humidityRatioRange.Max) || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max))
            {
                return null;
            }

            List<MollierPoint> result = new List<MollierPoint>();
            result.Add(new MollierPoint(dryBulbTemperatureRange.Min, humidityRatioRange.Min, pressure));
            result.Add(new MollierPoint(dryBulbTemperatureRange.Max, humidityRatioRange.Min, pressure));
            result.Add(new MollierPoint(dryBulbTemperatureRange.Max, humidityRatioRange.Max, pressure));
            result.Add(new MollierPoint(dryBulbTemperatureRange.Min, humidityRatioRange.Max, pressure));

            return result;
        }
    }
}
