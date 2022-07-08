namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static MixingProcess MixingProcess(this MollierPoint point_1, MollierPoint point_2, double ratio)
        {
            if (point_1 == null || point_2 == null || double.IsNaN(ratio))
            {
                return null;
            }


            double dryBulbTemperature = ratio * point_1.DryBulbTemperature + (1 - ratio) * point_2.DryBulbTemperature;
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }


            double humidityRatio = ratio * point_1.HumidityRatio + (1 - ratio) * point_2.HumidityRatio;
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, point_1.Pressure);
            if(end == null)
            {
                return null;
            }

            return new MixingProcess(point_1, end);
        }

        public static MixingProcess MixingProcess(this MollierPoint point_1, MollierPoint point_2, double flow_1, double flow_2)
        {
            if (point_1 == null || point_2 == null || double.IsNaN(flow_1) || double.IsNaN(flow_2))
            {
                return null;
            }

            return MixingProcess(point_1, point_2, flow_1 == 0 ? 0 : flow_2 / flow_1);
        }
    }
}
