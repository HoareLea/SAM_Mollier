namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool TryFindDiagramTemperature(double humidityRatio_1, double dryBulbTemperature_1, double humidityRatio_2, double dryBulbTemperature_2, double humidityRatio_3, out double diagramTemperature)
        {
            return Intersection(humidityRatio_1, dryBulbTemperature_1, humidityRatio_2, dryBulbTemperature_2, humidityRatio_3, 0, humidityRatio_3, 1, out diagramTemperature, out double humidityRatio);
        }

        public static bool TryFindDiagramTemperature(double enthalpy, double humidityRatio, double pressure, out double diagramTemperature)
        {
            double dryBulbTemperature_1 = DryBulbTemperature(enthalpy, 0, pressure);
            double humidityRatio_1 = 0;// Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature_1, 0, pressure);

            double diagramTemperature_1 = DiagramTemperature(dryBulbTemperature_1, humidityRatio_1, pressure);

            double temperature_2 = DryBulbTemperature_ByEnthalpy(enthalpy, 100, pressure);
            double humidityRatio_2 = HumidityRatio(temperature_2, 100, pressure);
            MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);

            return TryFindDiagramTemperature(humidityRatio_1, diagramTemperature_1, mollierPoint_2.HumidityRatio, mollierPoint_2.DryBulbTemperature, humidityRatio, out diagramTemperature);

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
