namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static ConstantTemperatureCurve ConstantTemperatureCurve_DryBulbTemperature(this MollierRange mollierRange, double dryBulbTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }


            double humidityRatio_1 = System.Math.Max(mollierRange.HumidityRatio_Min, 0);

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

            if (humidityRatio_2 > mollierRange.HumidityRatio_Max)
            {
                humidityRatio_2 = mollierRange.HumidityRatio_Max;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature, humidityRatio_2, pressure);
            if(!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantTemperatureCurve(Phase.Gas, dryBulbTemperature, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantTemperatureCurve ConstantTemperatureCurve_Liquid(this MollierRange mollierRange, double pressure)
        {
            if(double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            MollierPoint start = Query.SaturationMollierPoint(0, pressure);
            MollierPoint mollierPoint = MollierPoint_ByEnthalpy(start.Enthalpy, 0, pressure);

            double diagramTemperature = Query.DiagramTemperature(mollierPoint);

            if(!Query.Intersection(mollierPoint.HumidityRatio, diagramTemperature, start.HumidityRatio, 0, 0, mollierRange.DryBulbTemperature_Min, start.HumidityRatio, mollierRange.DryBulbTemperature_Min, out double dryBulbTemperature, out double humidityRatio))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(0, humidityRatio, pressure);

            return new ConstantTemperatureCurve(Phase.Liquid, 0, start, end);
        }

        public static ConstantTemperatureCurve ConstantTemperatureCurve_Solid(this MollierRange mollierRange, double pressure)
        {
            if (double.IsNaN(pressure) || mollierRange == null || !mollierRange.IsValid())
            {
                return null;
            }

            MollierPoint start = Query.SaturationMollierPoint(0, pressure);
            MollierPoint mollierPoint = MollierPoint_ByEnthalpy(start.Enthalpy, 0, pressure);

            double diagramTemperature = Query.DiagramTemperature(mollierPoint);

            if (!Query.Intersection(mollierPoint.HumidityRatio, diagramTemperature, start.HumidityRatio, 0, 0, mollierRange.DryBulbTemperature_Min, start.HumidityRatio, mollierRange.DryBulbTemperature_Min, out double dryBulbTemperature, out double humidityRatio))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(0, humidityRatio, pressure);

            return new ConstantTemperatureCurve(Phase.Solid, 0, start, end);
        }
    }
}
