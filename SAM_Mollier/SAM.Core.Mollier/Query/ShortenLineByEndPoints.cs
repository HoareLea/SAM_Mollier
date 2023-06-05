using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<MollierPoint> ShortenLineByEndPoints(MollierPoint point_1, MollierPoint point_2, double humidityRatio_Min, double humidityRatio_Max, double dryBulbTemperature_Min, double dryBulbTemperature_Max)
        {
            if(point_1 == null || point_2 == null)
            {
                return null;
            }

            List<MollierPoint> result = new List<MollierPoint>();

            humidityRatio_Max /= 1000;
            humidityRatio_Min /= 1000;
            
            double a = (point_1.DryBulbTemperature - point_2.DryBulbTemperature) / (point_1.HumidityRatio - point_2.HumidityRatio);
            if(double.IsNaN(a) || double.IsInfinity(a))
            {
                return null;
            }

            double b = point_1.DryBulbTemperature - a * point_1.HumidityRatio;
            if (double.IsNaN(b) || double.IsInfinity(b))
            {
                return null;
            }


            if (point_1.HumidityRatio < humidityRatio_Min)
            {
                point_1 = new MollierPoint(a * humidityRatio_Min + b, humidityRatio_Min, point_1.Pressure);
            }
            else if (point_1.HumidityRatio > humidityRatio_Max)
            {
                point_1 = new MollierPoint(a * humidityRatio_Max + b, humidityRatio_Max, point_1.Pressure);
            }
            if (point_1.DryBulbTemperature < dryBulbTemperature_Min)
            {
                point_1 = new MollierPoint(dryBulbTemperature_Min, (dryBulbTemperature_Min - b) / a, point_1.Pressure);
            }
            else if (point_1.DryBulbTemperature > dryBulbTemperature_Max)
            {
                point_1 = new MollierPoint(dryBulbTemperature_Max, (dryBulbTemperature_Max - b) / a, point_1.Pressure);
            }

            if (point_2.HumidityRatio < humidityRatio_Min)
            {
                point_2 = new MollierPoint(a * humidityRatio_Min + b, humidityRatio_Min, point_2.Pressure);
            }
            else if (point_2.HumidityRatio > humidityRatio_Max)
            {
                point_2 = new MollierPoint(a * humidityRatio_Max + b, humidityRatio_Max, point_2.Pressure);
            }
            if (point_2.DryBulbTemperature < dryBulbTemperature_Min)
            {
                point_2 = new MollierPoint(dryBulbTemperature_Min, (dryBulbTemperature_Min - b) / a, point_2.Pressure);
            }
            else if (point_2.DryBulbTemperature > dryBulbTemperature_Max)
            {
                point_2 = new MollierPoint(dryBulbTemperature_Max, (dryBulbTemperature_Max - b) / a, point_2.Pressure);
            }

            result.Add(point_1);
            result.Add(point_2);

            return result;
        }
    }
}
