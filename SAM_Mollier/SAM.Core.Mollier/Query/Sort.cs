using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<IMollierProcess> Sort(this IEnumerable<IMollierProcess> mollierProcesses, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierProcesses == null)
            {
                return null;
            }

            List<MollierPoint> mollierPoints = mollierProcesses.MollierPoints(tolerance);
            if(mollierPoints == null || mollierPoints.Count == 0)
            {
                return null;
            }

            MollierPoint mollierPoint = null;
            foreach(MollierPoint mollierPoint_Temp in mollierPoints)
            {
                List<IMollierProcess> mollierProcesses_Temp = MollierProcesses(mollierProcesses, mollierPoint_Temp, tolerance);
                if(mollierProcesses_Temp == null || mollierProcesses_Temp.Count != 1)
                {
                    continue;
                }

                mollierPoint = mollierPoint_Temp;
                break;
            }

            if(mollierPoint == null)
            {
                mollierPoint = mollierPoints[0];
            }

            List<IMollierProcess> result = new List<IMollierProcess>();
            IMollierProcess mollierProcess = Next(mollierProcesses, mollierPoint, tolerance);
            while(mollierProcess != null)
            {
                result.Add(mollierProcess);
                mollierProcess = Next(mollierProcesses, mollierProcess.End, tolerance);
            }

            return result;
        }
    }
}
