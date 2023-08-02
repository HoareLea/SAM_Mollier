using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static int Count(this IEnumerable<IMollierProcess> mollierProcesses, MollierPoint mollierPoint, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierProcesses == null || mollierPoint == null || mollierProcesses.Count() == 0)
            {
                return -1;
            }

            return mollierProcesses.ToList().FindAll(x => mollierPoint.Distance(x.Start) < tolerance || mollierPoint.Distance(x.End) < tolerance).Count;

        }
    }
}
