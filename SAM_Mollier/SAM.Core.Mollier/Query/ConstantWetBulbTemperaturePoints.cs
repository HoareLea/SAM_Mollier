using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    { 
        public static Dictionary<double, List<MollierPoint>> ConstantWetBulbTemperaturePoints(double wetBulbTemperature_Min = Default.WetBulbTemperature_Min, double wetBulbTemperature_Max = Default.WetBulbTemperature_Max, double pressure = Standard.Pressure, double wetBulbTemperatureStep = 5, double dryBulbTemperature_Min = Default.DryBulbTemperature_Min, double dryBulbTemperature_Max = Default.DryBulbTemperature_Max, double humidityRatio_Min = Default.HumidityRatio_Min, double humidityRatio_Max = Default.HumidityRatio_Max)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();
            while (wetBulbTemperature_Min <= wetBulbTemperature_Max)
            {
                result[wetBulbTemperature_Min] = new List<MollierPoint>();
                double temperature_1 = DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature_Min, 0, pressure);
                double humidityRatio_1 = HumidityRatio(temperature_1, 0, pressure);
                if (wetBulbTemperature_Min == 30)
                {
                    temperature_1 = DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature_Min, 20, pressure);
                    humidityRatio_1 = HumidityRatio(temperature_1, 20, pressure);
                }
                MollierPoint mollierPoint_1 = new MollierPoint(temperature_1, humidityRatio_1, pressure);

                double temperature_2 = DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature_Min, 100, pressure);
                double humidityRatio_2 = HumidityRatio(temperature_2, 100, pressure);
                MollierPoint mollierPoint_2 = new MollierPoint(temperature_2, humidityRatio_2, pressure);

                List<MollierPoint> points = ShortenLineByEndPoints(mollierPoint_1, mollierPoint_2, humidityRatio_Min, humidityRatio_Max, dryBulbTemperature_Min, dryBulbTemperature_Max);
                if (points != null && points.Count > 1)
                {
                    result[wetBulbTemperature_Min].Add(points[0]);
                    result[wetBulbTemperature_Min].Add(points[1]);
                }

                wetBulbTemperature_Min += wetBulbTemperatureStep;
            }
            return result;

        }
    }
}
