using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<MollierPoint> ProcessMollierPoints(this CoolingProcess coolingProcess, double tolerance = Tolerance.MacroDistance)
        {
            if (coolingProcess == null)
            {
                return null;
            }

            MollierPoint mollierPoint_Start = coolingProcess.Start;
            if(mollierPoint_Start == null || !mollierPoint_Start.IsValid())
            {
                return null;
            }

            MollierPoint mollierPoint_End = coolingProcess.End;
            if (mollierPoint_End == null || !mollierPoint_End.IsValid())
            {
                return null;
            }

            if(Core.Query.AlmostEqual(mollierPoint_Start.HumidityRatio, mollierPoint_End.HumidityRatio, tolerance))
            {
                return new List<MollierPoint>() { mollierPoint_Start, mollierPoint_End };
            }

            MollierPoint mollierPoint_75 = null;
            MollierPoint mollierPoint_85 = null;

            double dryBulbTemperature = mollierPoint_Start.RelativeHumidity < 75 ? DryBulbTemperature_ByHumidityRatio(mollierPoint_Start.HumidityRatio, 75, mollierPoint_Start.Pressure) : mollierPoint_Start.DryBulbTemperature;
            if (mollierPoint_Start.RelativeHumidity < 85)
            {
                mollierPoint_85 = Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature - 2.5, 85, mollierPoint_Start.Pressure);
                if (mollierPoint_Start.RelativeHumidity < 75)
                {
                    mollierPoint_75 = Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature, 75, mollierPoint_Start.Pressure);
                }
            }

            List<MollierPoint> result = null;
            if (mollierPoint_85 != null)
            {
                result = new List<MollierPoint>();
                result.Add(new MollierPoint(mollierPoint_Start));
                
                if (mollierPoint_75 != null)
                {
                    result.Add(mollierPoint_75);
                }

                result.Add(mollierPoint_85);
                result.Add(mollierPoint_End);
            }

            return result;
        }
    }
}
