using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<MollierPoint> MollierPoints(this IEnumerable<IMollierProcess> mollierProcesses, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierProcesses == null || mollierProcesses.Count() == 0)
            {
                return null;
            }

            List<MollierPoint> result = new List<MollierPoint>();

            foreach(IMollierProcess mollierProcess in mollierProcesses)
            {
                if(mollierProcess == null)
                {
                    continue;
                }

                if(result.Find(x => x.Distance(mollierProcess.Start) < tolerance) == null)
                {
                    result.Add(mollierProcess.Start);
                }

                if (result.Find(x => x.Distance(mollierProcess.End) < tolerance) == null)
                {
                    result.Add(mollierProcess.End);
                }
            }

            return result;
        }
    }
}
