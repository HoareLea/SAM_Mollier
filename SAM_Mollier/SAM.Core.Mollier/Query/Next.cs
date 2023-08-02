using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static IMollierProcess Next(this IEnumerable<IMollierProcess> mollierProcesses, MollierPoint mollierPoint, double tolerance = Tolerance.MacroDistance)
        {
            if (mollierProcesses == null || mollierPoint == null)
            {
                return null;
            }

            foreach(IMollierProcess mollierProcess in mollierProcesses)
            {
                if(mollierProcess.Start.AlmostEqual(mollierPoint))
                { 
                    return mollierProcess;
                }
            }

            return null;
        }
    }
}
