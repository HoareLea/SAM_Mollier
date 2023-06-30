using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {

        public static Dictionary<double, List<MollierPoint>> ConstantRelativeHumidityPoints(int temperature_Min, int temperature_Max, double pressure, int relativeHumidity_Step = 10, double humidityRatio_Min = Default.HumidityRatioMin, double humidityRatio_Max = Default.HumidityRatioMax)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            for (int i = 0; i <= 100; i += relativeHumidity_Step)
            {
                List<MollierPoint> relativeHumidity_Points = new List<MollierPoint>();
                for (int j = temperature_Min; j <= temperature_Max; j++)
                {
                    double humidityRatio = HumidityRatio(j, i, pressure);
                    if (humidityRatio * 1000 < humidityRatio_Min)
                    {
                        continue;
                    }
                    if (humidityRatio * 1000 > humidityRatio_Max)
                    {
                        if (relativeHumidity_Points != null && relativeHumidity_Points.Count > 2)
                        {
                            double a = (relativeHumidity_Points[relativeHumidity_Points.Count - 2].DryBulbTemperature - relativeHumidity_Points[relativeHumidity_Points.Count - 1].DryBulbTemperature) / (relativeHumidity_Points[relativeHumidity_Points.Count - 2].HumidityRatio - relativeHumidity_Points[relativeHumidity_Points.Count - 1].HumidityRatio);
                            double b = relativeHumidity_Points[relativeHumidity_Points.Count - 2].DryBulbTemperature - a * relativeHumidity_Points[relativeHumidity_Points.Count - 2].HumidityRatio;
                            double dryBulbTemperature_Temp = a * (humidityRatio_Max / 1000) + b;
                            MollierPoint mollierPoint_Temp = new MollierPoint(dryBulbTemperature_Temp, humidityRatio_Max / 1000, pressure);
                            relativeHumidity_Points.Add(mollierPoint_Temp);
                        }
                        break;
                    }
                    MollierPoint mollierPoint = new MollierPoint(j, humidityRatio, pressure);
                    relativeHumidity_Points.Add(mollierPoint);
                }
                result.Add(i, relativeHumidity_Points);
            }

            return result;
        }

    }
}
