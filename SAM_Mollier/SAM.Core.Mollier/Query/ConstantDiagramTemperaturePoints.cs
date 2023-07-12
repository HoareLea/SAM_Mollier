using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static Dictionary<double, List<MollierPoint>> ConstantDiagramTemperaturePoints(double temperature_Min, double temperature_Max, double pressure)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            int min = System.Convert.ToInt32(temperature_Min);
            int max = System.Convert.ToInt32(temperature_Max);

            for (int i = min; i <= max; i++)
            {
                result[i] = new List<MollierPoint>();
            }

            double relativeHumidity = 0;

            while (relativeHumidity <= 100)
            {
                for (int j = min; j <= max; j++)
                {
                    double humidityRatio = HumidityRatio(j, relativeHumidity, pressure);
                    double diagramTemperature = DiagramTemperature(j, humidityRatio, pressure);

                    result[j].Add(new MollierPoint(diagramTemperature, humidityRatio, pressure));
                }

                relativeHumidity += 100;
            }

            return result;

            //Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            //for (int i = temperature_Min; i <= temperature_Max; i++)
            //{
            //    //List<MollierPoint> mollierPoints = new List<MollierPoint>();

            //    //double diagramTempearture_1 = DiagramTemperature(i, 0, pressure);

            //    //MollierPoint mollierPoint_1 = new MollierPoint(diagramTempearture_1, 0, pressure);

            //    //double humidityRatio = HumidityRatio(i, 100, pressure);

            //    //double diagramTempearture_2 = DiagramTemperature(i, humidityRatio, pressure);

            //    //MollierPoint mollierPoint_2 = SaturationDiagramPoint(diagramTempearture_2, humidityRatio, pressure);
            //    //if (mollierPoint_2 == null)
            //    //{
            //    //    humidityRatio = humidityRatio_Max;
            //    //    diagramTempearture_2 = DiagramTemperature(i, humidityRatio, pressure);
            //    //    mollierPoint_2 = new MollierPoint(diagramTempearture_2, humidityRatio, pressure);
            //    //}

            //    //mollierPoints.Add(mollierPoint_1);
            //    //mollierPoints.Add(mollierPoint_2);
            //    //result.Add(i, mollierPoints);

            //    MollierPoint mollierPoint_2 = new MollierPoint(i, HumidityRatio(i, 100, pressure), pressure);

            //    MollierPoint mollierPoint_2 = new MollierPoint(DiagramTemperature(i, HumidityRatio(i, 100, pressure), pressure), HumidityRatio(DiagramTemperature(i, HumidityRatio(i, 100, pressure), pressure), 100, pressure), pressure);

            //    List<MollierPoint> points = ShortenLineByEndPoints(mollierPoint_1, mollierPoint_2, humidityRatio_Min, humidityRatio_Max, temperature_Min, temperature_Max);
            //    if (points != null && points.Count > 1)
            //    {
            //        mollierPoints.Add(points[0]);
            //        mollierPoints.Add(points[1]);
            //        result.Add(i, mollierPoints);
            //    }
            //}
            //return result;
        }
    }
}
