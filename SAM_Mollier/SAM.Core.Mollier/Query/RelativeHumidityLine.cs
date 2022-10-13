using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {

        public static Dictionary<double, List<MollierPoint>> RelativeHumidityLine(int temperature_Min, int temperature_Max, double pressure, int relativeHumidity_Step = 10)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            for (int i=0; i<=100; i += relativeHumidity_Step)
            {
                List<MollierPoint> relativeHumidity_Points = new List<MollierPoint>();
                for(int j=temperature_Min; j<=temperature_Max; j++)
                {
                    double humidityRatio = Query.HumidityRatio(j, i, pressure);
                    MollierPoint mollierPoint = new MollierPoint(j, humidityRatio, pressure);
                    relativeHumidity_Points.Add(mollierPoint);
                }
                result.Add(i, relativeHumidity_Points);
            }

            return result;
        }

    }
}
