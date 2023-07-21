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

            List<MollierPoint> mollierPoints_Range = Create.MollierPoints(humidityRatioRange, dryBulbTemperatureRange, constantValueCurve.Pressure);
            if(mollierPoints_Range == null || mollierPoints_Range.Count == 0)
            {
                return null;
            }
            mollierPoints_Range.Add(mollierPoints_Range[0]);

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

                mollierPoints_Temp.Add(start);

                List<MollierPoint> mollierPoints_Intersection = Intersections(start, end, mollierPoints_Range, tolerance);
                if(mollierPoints_Intersection == null || mollierPoints_Intersection.Count == 0)
                {
                    continue;
                }


                if (constantValueCurve.ChartDataType == ChartDataType.RelativeHumidity && constantValueCurve.Value == 100)
                {
                    mollierPoints_Intersection = mollierPoints_Intersection.ConvertAll(x => new MollierPoint(x.DryBulbTemperature, HumidityRatio(x.DryBulbTemperature, 100, x.Pressure), x.Pressure));
                }

                if (mollierPoints_Intersection.Count > 1)
                {
                    mollierPoints_Intersection.Sort((x, y) => func_Distance.Invoke(start, x).CompareTo(func_Distance.Invoke(start, y)));
                }

                mollierPoints_Intersection.ForEach(x => mollierPoints_Temp.Add(x));
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
