namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static MollierPoint SaturationDiagramPoint(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            if (double.IsNaN(dryBulbTemperature) || double.IsNaN(humidityRatio) || double.IsNaN(pressure))
            {
                return null;
            }

            double diagramTempearture = DiagramTemperature(dryBulbTemperature, humidityRatio, pressure);

            double saturationHumidityRatio = SaturationHumidityRatio(diagramTempearture, pressure);
            if (double.IsNaN(saturationHumidityRatio))
            {
                return null;
            }

            while (saturationHumidityRatio - humidityRatio > 0)
            {
                humidityRatio += humidityRatio * 0.01;
                diagramTempearture = DiagramTemperature(dryBulbTemperature, humidityRatio, pressure);
                saturationHumidityRatio = SaturationHumidityRatio(diagramTempearture, pressure);
                if (double.IsNaN(saturationHumidityRatio))
                {
                    return null;
                }
            }

            while (saturationHumidityRatio - humidityRatio < 0)
            {
                humidityRatio -= humidityRatio * 0.001;
                diagramTempearture = DiagramTemperature(dryBulbTemperature, humidityRatio, pressure);
                saturationHumidityRatio = SaturationHumidityRatio(diagramTempearture, pressure);
                if (double.IsNaN(saturationHumidityRatio))
                {
                    return null;
                }
            }

            return new MollierPoint(diagramTempearture, humidityRatio, pressure);
        }
    }
}
