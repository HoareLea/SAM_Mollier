namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static MollierPoint DewPointMollierPoint(this MollierPoint mollierPoint, double efficiency = 1)
        {
            double dryBulbTemperature = mollierPoint.DryBulbTemperature;
            if(efficiency != 1)
            {
                dryBulbTemperature -= efficiency * (mollierPoint.DryBulbTemperature - DryBulbTemperature_ByHumidityRatio(mollierPoint.HumidityRatio, 100, mollierPoint.Pressure));
            }

            return new MollierPoint(dryBulbTemperature, mollierPoint.HumidityRatio, mollierPoint.Pressure);
        }
    }
}
