
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static Dictionary<double, List<MollierPoint>> ConstantDiagramTemperaturePoints(int temperature_Min, int temperature_Max, double pressure, double humidityRatio_Min = Default.HumidityRatioMin, double humidityRatio_Max = Default.HumidityRatioMax)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            for(int i= temperature_Min; i<=temperature_Max; i++)
            {
                List<MollierPoint> pointList = new List<MollierPoint>();
                MollierPoint mollierPoint_1 = new MollierPoint(i, 0, pressure);

                MollierPoint mollierPoint_2 = new MollierPoint(DiagramTemperature(i, HumidityRatio(i, 100, pressure)), HumidityRatio(DiagramTemperature(i, HumidityRatio(i, 100, pressure)), 100, pressure), pressure);

                List<MollierPoint> points = ShortenLineByEndPoints(mollierPoint_1, mollierPoint_2, humidityRatio_Min, humidityRatio_Max, temperature_Min, temperature_Max);
                if (points != null && points.Count > 1)
                {
                    pointList.Add(points[0]);
                    pointList.Add(points[1]);
                    result.Add(i, pointList);
                }
            }
            return result;
        }
    }
}
