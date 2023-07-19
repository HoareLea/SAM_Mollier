namespace SAM.Core.Mollier
{
    public static partial class Create
    { 
        public static ConstantTemperatureCurve ConstantTemperatureCurve_DiagramTemperature(double dryBulbTemperature, double pressure)
        {
            if(double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return null;
            }

            double humidityRatio_1 = Query.HumidityRatio(dryBulbTemperature, 0, pressure);
            if(double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            double diagramTemperature_1 = Query.DiagramTemperature(dryBulbTemperature, humidityRatio_1, pressure);
            if(double.IsNaN(diagramTemperature_1))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(diagramTemperature_1, humidityRatio_1, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            double humidityRatio_2 = Query.HumidityRatio(dryBulbTemperature, 100, pressure);
            if (double.IsNaN(humidityRatio_1))
            {
                return null;
            }

            double diagramTemperature_2 = Query.DiagramTemperature(dryBulbTemperature, humidityRatio_2, pressure);
            if (double.IsNaN(diagramTemperature_1))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(diagramTemperature_2, humidityRatio_2, pressure);
            if (!mollierPoint_2.IsValid())
            {
                return null;
            }

            return new ConstantTemperatureCurve(Phase.Gas, dryBulbTemperature, mollierPoint_1, mollierPoint_2);
        }

        public static ConstantTemperatureCurve ConstantTemperatureCurve_DryBulbTemperature(double dryBulbTemperature, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(pressure))
            {
                return null;
            }

            MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature, 0, pressure);
            if (!mollierPoint_1.IsValid())
            {
                return null;
            }

            double humidityRatio = Query.HumidityRatio(dryBulbTemperature, 100, pressure);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }

            MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
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

            if(!Query.Intersection(mollierPoint.HumidityRatio, mollierPoint.DryBulbTemperature, start.HumidityRatio, start.DryBulbTemperature, 0, dryBulbTemperature_Min, start.HumidityRatio, dryBulbTemperature_Min, out double dryBulbTemperature, out double humidityRatio))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(0, humidityRatio, pressure);

            return new ConstantTemperatureCurve(Phase.Liquid, 0, start, end);
        }

        public static ConstantTemperatureCurve ConstantTemperatureCurve_Solid(double pressure)
        {
            MollierPoint start = Query.SaturationMollierPoint(0, pressure);
            MollierPoint end = null;

            throw new System.NotImplementedException();

            return new ConstantTemperatureCurve(Phase.Liquid, 0, start, end);
        }
    }
}
