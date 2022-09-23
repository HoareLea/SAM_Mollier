namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static MollierPoint DewPoint(this MollierPoint mollierPoint, double efficiency = 1)
        {
            if(mollierPoint == null)
            {
                return null;
            }

            double dryBulbTemperature = mollierPoint.DryBulbTemperature - efficiency * (mollierPoint.DryBulbTemperature - DryBulbTemperature_ByHumidityRatio(mollierPoint.HumidityRatio, 100, mollierPoint.Pressure));

            return new MollierPoint(dryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure);
        }
    }
}
