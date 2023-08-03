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


            double dryBulbTemperature = point_1.DryBulbTemperature + (point_2.DryBulbTemperature - point_1.DryBulbTemperature) * ratio;
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }


            double humidityRatio = point_1.HumidityRatio + (point_2.HumidityRatio - point_1.HumidityRatio) * ratio;
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

            //flow 1 required by Michal 03/08/2023
            return MixingProcess(point_1, point_2, (flow_1 == 0 && flow_2 == 0) ? 0 : flow_1 / (flow_1 + flow_2));
        }
    }
}
