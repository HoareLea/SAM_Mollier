using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static ConstantValueCurve Clamp(this ConstantValueCurve constantValueCurve, Range<double> humidityRatioRange, Range<double> dryBulbTemperatureRange, double tolerance = Tolerance.Distance)
        {
            if (constantValueCurve == null || dryBulbTemperatureRange == null || double.IsNaN(dryBulbTemperatureRange.Min) || double.IsNaN(dryBulbTemperatureRange.Max) || humidityRatioRange == null || double.IsNaN(humidityRatioRange.Min) || double.IsNaN(humidityRatioRange.Max))
            {
                return null;
            }

            List<MollierPoint> mollierPoints = constantValueCurve.MollierPoints;
            if (mollierPoints == null || mollierPoints.Count < 2)
            {
                return null;
            }


            Func<MollierPoint, MollierPoint, double> func_Distance = new Func<MollierPoint, MollierPoint, double>((MollierPoint mollierPoint_1, MollierPoint mollierPoint_2) =>
            {
                if(mollierPoint_1 == null || mollierPoint_2 == null || !mollierPoint_1.IsValid() || !mollierPoint_2.IsValid())
                {
                    return double.MaxValue;
                }

                double humidityRatioDifference = Math.Abs(mollierPoint_1.HumidityRatio - mollierPoint_2.HumidityRatio) * 1000;
                double dryBulbTemperatureDifference = Math.Abs(mollierPoint_1.DryBulbTemperature - mollierPoint_2.DryBulbTemperature);

                return Math.Sqrt((humidityRatioDifference * humidityRatioDifference) + (dryBulbTemperatureDifference * dryBulbTemperatureDifference));
            });

            List<MollierPoint> mollierPoints_Temp = new List<MollierPoint>();
            for(int i =0; i < mollierPoints.Count - 1; i++)
            {
                MollierPoint start = mollierPoints[i];
                MollierPoint end = mollierPoints[i + 1];

                List<Tuple<double, double>> tuples = new List<Tuple<double, double>>();

                double diagramTemperature_Start = start.DiagramTemperature();
                double diagramTemperature_End = end.DiagramTemperature();

                if (dryBulbTemperatureRange.In(diagramTemperature_Start) && humidityRatioRange.In(start.HumidityRatio))
                {
                    tuples.Add(new Tuple<double, double>(diagramTemperature_Start, start.HumidityRatio));
                }

                if (dryBulbTemperatureRange.In(diagramTemperature_End) && humidityRatioRange.In(end.HumidityRatio))
                {
                    tuples.Add(new Tuple<double, double>(diagramTemperature_End, end.HumidityRatio));
                }

                Intersection(start.HumidityRatio, diagramTemperature_Start, end.HumidityRatio, diagramTemperature_End, humidityRatioRange.Min, dryBulbTemperatureRange.Max, humidityRatioRange.Min + 1, dryBulbTemperatureRange.Max, out double dryBulbTemperature_1, out double humidityRatio_1);

                Intersection(start.HumidityRatio, diagramTemperature_Start, end.HumidityRatio, diagramTemperature_End, humidityRatioRange.Min, dryBulbTemperatureRange.Min, humidityRatioRange.Min + 1, dryBulbTemperatureRange.Min, out double dryBulbTemperature_2, out double humidityRatio_2);

                Intersection(start.HumidityRatio, diagramTemperature_Start, end.HumidityRatio, diagramTemperature_End, humidityRatioRange.Min, dryBulbTemperatureRange.Min, humidityRatioRange.Min, dryBulbTemperatureRange.Min + 1, out double dryBulbTemperature_3, out double humidityRatio_3);

                Intersection(start.HumidityRatio, diagramTemperature_Start, end.HumidityRatio, diagramTemperature_End, humidityRatioRange.Max, dryBulbTemperatureRange.Min, humidityRatioRange.Max, dryBulbTemperatureRange.Min + 1, out double dryBulbTemperature_4, out double humidityRatio_4);

                Range<double> dryBulbTemperatureRange_Temp = new Range<double>(diagramTemperature_Start, diagramTemperature_End);
                Range<double> humidityRatioRange_Temp = new Range<double>(start.HumidityRatio, end.HumidityRatio);
                

                if(dryBulbTemperatureRange.In(dryBulbTemperature_1) && dryBulbTemperatureRange_Temp.In(dryBulbTemperature_1) && humidityRatioRange.In(humidityRatio_1) && humidityRatioRange_Temp.In(humidityRatio_1))
                {
                    tuples.Add(new Tuple<double, double>(dryBulbTemperature_1, humidityRatio_1));
                }

                if (dryBulbTemperatureRange.In(dryBulbTemperature_2) && dryBulbTemperatureRange_Temp.In(dryBulbTemperature_2) && humidityRatioRange.In(humidityRatio_2) && humidityRatioRange_Temp.In(humidityRatio_2))
                {
                    tuples.Add(new Tuple<double, double>(dryBulbTemperature_2, humidityRatio_2));
                }

                if (dryBulbTemperatureRange.In(dryBulbTemperature_3) && dryBulbTemperatureRange_Temp.In(dryBulbTemperature_3) && humidityRatioRange.In(humidityRatio_3) && humidityRatioRange_Temp.In(humidityRatio_3))
                {
                    tuples.Add(new Tuple<double, double>(dryBulbTemperature_3, humidityRatio_3));
                }

                if (dryBulbTemperatureRange.In(dryBulbTemperature_4) && dryBulbTemperatureRange_Temp.In(dryBulbTemperature_4) && humidityRatioRange.In(humidityRatio_4) && humidityRatioRange_Temp.In(humidityRatio_4))
                {
                    tuples.Add(new Tuple<double, double>(dryBulbTemperature_4, humidityRatio_4));
                }

                List<MollierPoint> mollierPoints_Tuples = new List<MollierPoint>();
                foreach (Tuple<double, double> tuple in tuples)
                {
                    double dryBulbTemperature = DryBulbTemperature_ByDiagramTemperature(tuple.Item1, tuple.Item2, start.Pressure);
                    double diagramTemperature = DiagramTemperature(dryBulbTemperature, tuple.Item2, start.Pressure);

                    double difference = Math.Abs(tuple.Item1 - diagramTemperature);

                    MollierPoint mollierPoint = new MollierPoint(dryBulbTemperature, tuple.Item2, start.Pressure);
                    if(mollierPoint.IsValid())
                    {
                        mollierPoints_Tuples.Add(mollierPoint);
                    }
                }

                if(mollierPoints_Tuples.Count == 0)
                {
                    continue;
                }

                if(mollierPoints_Tuples.Count > 1)
                {
                    mollierPoints_Tuples.Sort((x, y) => func_Distance.Invoke(start, x).CompareTo(func_Distance.Invoke(start, y)));
                }

                mollierPoints_Tuples.ForEach(x => mollierPoints_Temp.Add(x));

                //mollierPoints_Temp.Add(start);

                //List<MollierPoint> mollierPoints_Intersection = Intersections(start, end, mollierPoints_Range, tolerance);
                //if(mollierPoints_Intersection == null || mollierPoints_Intersection.Count == 0)
                //{
                //    continue;
                //}


                //if (constantValueCurve.ChartDataType == ChartDataType.RelativeHumidity && constantValueCurve.Value == 100)
                //{
                //    mollierPoints_Intersection = mollierPoints_Intersection.ConvertAll(x => new MollierPoint(x.DryBulbTemperature, HumidityRatio(x.DryBulbTemperature, 100, x.Pressure), x.Pressure));
                //}

                //if (mollierPoints_Intersection.Count > 1)
                //{
                //    mollierPoints_Intersection.Sort((x, y) => func_Distance.Invoke(start, x).CompareTo(func_Distance.Invoke(start, y)));
                //}

                //mollierPoints_Intersection.ForEach(x => mollierPoints_Temp.Add(x));
            }

            mollierPoints_Temp.Add(mollierPoints.Last());

            mollierPoints = new List<MollierPoint>();
            for (int i = mollierPoints_Temp.Count - 1; i > 0; i--)
            {
                MollierPoint start = mollierPoints_Temp[i];
                MollierPoint end = mollierPoints_Temp[i - 1];

                if (!InRange(start.Mid(end), humidityRatioRange, dryBulbTemperatureRange))
                {
                    continue;
                }

                if(!mollierPoints.Contains(start))
                {
                    mollierPoints.Add(start);
                }

                if(!mollierPoints.Contains(end))
                {
                    mollierPoints.Add(end);
                }
            }



            return new ConstantValueCurve(constantValueCurve.ChartDataType, constantValueCurve.Value, mollierPoints);
        }

        public static List<ConstantValueCurve> Clamp(this IEnumerable<ConstantValueCurve> constantValueCurves, Range<double> humidityRatioRange, Range<double> dryBulbTemperatureRange, double tolerance = Tolerance.Distance)
        {
            if(constantValueCurves == null || humidityRatioRange == null || dryBulbTemperatureRange == null)
            {
                return null;
            }

            List<ConstantValueCurve> result = new List<ConstantValueCurve>();
            foreach(ConstantValueCurve constantValueCurve in constantValueCurves)
            {
                ConstantValueCurve constantValueCurve_Temp = constantValueCurve?.Clamp(humidityRatioRange, dryBulbTemperatureRange, tolerance);
                if(constantValueCurve_Temp == null)
                {
                    continue;
                }

                result.Add(constantValueCurve_Temp);
            }

            return result;
        }
    }
}
