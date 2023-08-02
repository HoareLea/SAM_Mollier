using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<IMollierProcess> MollierProcesses(this IEnumerable<IMollierProcess> mollierProcesses, MollierPoint mollierPoint, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierProcesses == null || mollierProcesses.Count() == 0 || mollierPoint == null)
            {
                return null;
            }

            List<IMollierProcess> result = new List<IMollierProcess>();

            foreach(IMollierProcess mollierProcess in mollierProcesses)
            {
                if(mollierProcess == null)
                {
                    continue;
                }

                if(mollierPoint.Distance(mollierProcess.Start) < tolerance || mollierPoint.Distance(mollierProcess.End) < tolerance)
                {
                    result.Add(mollierProcess);
                }
            }

            return result;
        }
    }
}
