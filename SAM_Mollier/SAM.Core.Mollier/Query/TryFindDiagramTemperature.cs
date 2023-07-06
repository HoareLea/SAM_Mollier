namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool TryFindDiagramTemperature(double humidityRatio_1, double dryBulbTemperature_1, double humidityRatio_2, double dryBulbTemperature_2, double humidityRatio_3, out double diagramTemperature)
        {
            diagramTemperature = double.NaN;

            //x => humidityRatio
            //y => dryBulbTemperature

            double dryBulbTemperature_3 = 0;

            double humidityRatio_4 = humidityRatio_3;
            double dryBulbTemperature_4 = 1;

            //Line 1
            double a1 = dryBulbTemperature_2 - dryBulbTemperature_1;
            double b1 = humidityRatio_1 - humidityRatio_2;
            double c1 = a1 * humidityRatio_1 + b1 * dryBulbTemperature_1;

            //Line 2
            double a2 = dryBulbTemperature_4 - dryBulbTemperature_3;
            double b2 = humidityRatio_3 - humidityRatio_4;
            double c2 = a2 * humidityRatio_3 + b2 * dryBulbTemperature_3;

            double det = a1 * b2 - a2 * b1;
            if (det == 0)
            {
                return false;
            }
            //humidityRatio = (b2 * c1 - c2 * b1) / det;
            diagramTemperature = (a1 * c2 - a2 * c1) / det;

            return true;
        }
    
        public static bool TryFindDiagramTemperature(MollierPoint mollierPoint_1, MollierPoint mollierPoint_2, double humidityRatio, out double diagramTemperature)
        {
            diagramTemperature = 0;

            if(mollierPoint_1 == null || mollierPoint_2 == null || double.IsNaN(humidityRatio))
            {
                return false;
            }

            return TryFindDiagramTemperature(mollierPoint_1.HumidityRatio, mollierPoint_1.DryBulbTemperature, mollierPoint_2.HumidityRatio, mollierPoint_2.DryBulbTemperature, humidityRatio, out diagramTemperature);
        }

        public static bool TryFindDiagramTemperature(double enthalpy, double humidityRatio, double pressure, out double diagramTemperature)
        {
            double dryBulbTemperature_1 = DryBulbTemperature(enthalpy, 0, pressure);
            MollierPoint mollierPoint_1 = Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature_1, 0, pressure);

            //MollierPoint mollierPoint_1_DT = Create.MollierPoint_ByRelativeHumidity(DiagramTemperature(dryBulbTemperature_1, humidityRatio, pressure), 0, pressure);

            double temperature_2 = DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            double humidityRatio_2 = HumidityRatio(temperature_2, 100, pressure);
            MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);

            //MollierPoint mollierPoint_2_DT = Create.MollierPoint_ByRelativeHumidity(DiagramTemperature(dryBulbTemperature_2, humidityRatio, pressure), 100, pressure);

            return TryFindDiagramTemperature(mollierPoint_1, mollierPoint_2, humidityRatio, out diagramTemperature);

        }

        public static bool TryFindDiagramTemperature(MollierPoint mollierPoint, out double diagramTemperature)
        {
            diagramTemperature = double.NaN;

            if (mollierPoint == null)
            {
                return false;
            }

            return TryFindDiagramTemperature(mollierPoint.Enthalpy, mollierPoint.HumidityRatio, mollierPoint.Pressure, out diagramTemperature);
        }


    }
}
