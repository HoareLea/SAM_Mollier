namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static MollierPoint Closest(this MollierPoint start, MollierPoint end, MollierPoint mollierPoint, bool bounded = true)
        {
            if (start == null || end == null)
            {
                return null;
            }

            MollierPoint start_Temp = new MollierPoint(start);
            MollierPoint end_Temp = new MollierPoint(end);

            double a = mollierPoint.HumidityRatio - start_Temp.HumidityRatio;
            double b = mollierPoint.DryBulbTemperature - start_Temp.DryBulbTemperature;
            double c = end_Temp.HumidityRatio - start_Temp.HumidityRatio;
            double d = end_Temp.DryBulbTemperature - start_Temp.DryBulbTemperature;

            double dot = a * c + b * d;
            double len_sq = c * c + d * d;
            double parameter = -1;
            if (len_sq != 0)
                parameter = dot / len_sq;

            if (parameter < 0 && bounded)
                return start_Temp;
            else if (parameter > 1 && bounded)
                return end_Temp;
            else
                return new MollierPoint(start_Temp.HumidityRatio + parameter * c, start_Temp.DryBulbTemperature + parameter * d, start.Pressure);

        }
    }
}
