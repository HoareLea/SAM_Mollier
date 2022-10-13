using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    { 
        public static Dictionary<double, List<MollierPoint>> WetBulbTemperatureLine(double wetBulbTemperature_Min = Default.WetBulbTemperatureMin, double wetBulbTemperature_Max = Default.WetBulbTemperatureMax, double pressure = Standard.Pressure, double wetBulbTemperatureStep = 5)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();
            while (wetBulbTemperature_Min <= wetBulbTemperature_Max)
            {
                result[wetBulbTemperature_Min] = new List<MollierPoint>();
                double temperature_1 = Query.DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature_Min, 0, pressure);
                double humidityRatio_1 = Query.HumidityRatio(temperature_1, 0, pressure);
                if (wetBulbTemperature_Min == 30)
                {
                    temperature_1 = Query.DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature_Min, 20, pressure);
                    humidityRatio_1 = Query.HumidityRatio(temperature_1, 20, pressure);
                }
                MollierPoint mollierPoint_1 = new MollierPoint(temperature_1, humidityRatio_1, pressure);
                result[wetBulbTemperature_Min].Add(mollierPoint_1);

                double temperature_2 = Query.DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature_Min, 100, pressure);
                double humidityRatio_2 = Query.HumidityRatio(temperature_2, 100, pressure);
                MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);
                result[wetBulbTemperature_Min].Add(mollierPoint_2);

                wetBulbTemperature_Min += wetBulbTemperatureStep;
            }
            return result;

        }
    }
}
