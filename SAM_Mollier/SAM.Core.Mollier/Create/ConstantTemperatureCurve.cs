namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static ConstantTemperatureCurve ConstantTemperatureCurve_DryBulbTemperature(double dryBulbTemperature, Range<double> humidityRatioRange, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure) || humidityRatioRange == null || double.IsNaN(humidityRatioRange.Min) || double.IsNaN(humidityRatioRange.Max))
            {
                return null;
            }


            double humidityRatio_1 = System.Math.Max(humidityRatioRange.Min, 0);

            if(humidityRatio_1 > Query.SaturationHumidityRatio(dryBulbTemperature, pressure))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature, humidityRatio_1, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            double humidityRatio_2 = Query.HumidityRatio(dryBulbTemperature, 100, pressure);
            if (double.IsNaN(humidityRatio_2))
            {
                return null;
            }

            if (humidityRatio_2 > humidityRatioRange.Max)
            {
                humidityRatio_2 = humidityRatioRange.Max;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature, humidityRatio_2, pressure);
            if(!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantTemperatureCurve(Phase.Gas, dryBulbTemperature, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantTemperatureCurve ConstantTemperatureCurve_Liquid(double dryBulbTemperature_Min, double pressure)
        {
            if(double.IsNaN(pressure))
            {
                return null;
            }

            MollierPoint start = Query.SaturationMollierPoint(0, pressure);
            MollierPoint mollierPoint = MollierPoint_ByEnthalpy(start.Enthalpy, 0, pressure);

            double diagramTemperature = Query.DiagramTemperature(mollierPoint);

            if(!Query.Intersection(mollierPoint.HumidityRatio, diagramTemperature, start.HumidityRatio, 0, 0, dryBulbTemperature_Min, start.HumidityRatio, dryBulbTemperature_Min, out double dryBulbTemperature, out double humidityRatio))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(0, humidityRatio, pressure);

            return new ConstantTemperatureCurve(Phase.Liquid, 0, start, end);
        }

        public static ConstantTemperatureCurve ConstantTemperatureCurve_Solid(double dryBulbTemperature_Min, double pressure)
        {
            if (double.IsNaN(pressure))
            {
                return null;
            }

            MollierPoint start = Query.SaturationMollierPoint(0, pressure);
            MollierPoint mollierPoint = MollierPoint_ByEnthalpy(start.Enthalpy, 0, pressure);

            double diagramTemperature = Query.DiagramTemperature(mollierPoint);

            if (!Query.Intersection(mollierPoint.HumidityRatio, diagramTemperature, start.HumidityRatio, 0, 0, dryBulbTemperature_Min, start.HumidityRatio, dryBulbTemperature_Min, out double dryBulbTemperature, out double humidityRatio))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(0, humidityRatio, pressure);

            return new ConstantTemperatureCurve(Phase.Solid, 0, start, end);
        }
    }
}
