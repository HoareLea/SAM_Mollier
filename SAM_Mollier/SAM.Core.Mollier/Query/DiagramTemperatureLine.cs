
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static Dictionary<double, List<MollierPoint>> DiagramTemperatureLine(int temperature_Min, int temperature_Max, double pressure)
        {
            Dictionary<double, List<MollierPoint>> result = new Dictionary<double, List<MollierPoint>>();

            for(int i= temperature_Min; i<=temperature_Max; i++)
            {
                List<MollierPoint> pointList = new List<MollierPoint>();
                MollierPoint mollierPoint_1 = new MollierPoint(i, 0, pressure);
                pointList.Add(mollierPoint_1); 

                MollierPoint mollierPoint_2 = new MollierPoint(i, HumidityRatio(i, 100, pressure), pressure);
                pointList.Add(mollierPoint_2);

                result.Add(i, pointList);
            }
            return result;
        }
    }
}
