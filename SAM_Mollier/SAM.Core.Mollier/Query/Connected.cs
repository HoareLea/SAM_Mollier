using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<IMollierProcess> Connected(this IEnumerable<IMollierProcess> mollierProcesses, MollierPoint mollierPoint, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierProcesses == null || mollierPoint == null)
            {
                return null;
            }

            List<IMollierProcess> result = new List<IMollierProcess>();

            if(mollierProcesses.Count() == 0)
            {
                return result;
            }

            List<IMollierProcess> mollierProcesses_Temp = new List<IMollierProcess>(mollierProcesses);
            List<IMollierProcess> mollierProcesses_Start = mollierProcesses_Temp.FindAll(x => mollierPoint.Distance(x.Start) < tolerance);
            List<IMollierProcess> mollierProcesses_End = mollierProcesses_Temp.FindAll(x => mollierPoint.Distance(x.End) < tolerance);

            if(mollierProcesses_Start.Count == 0 && mollierProcesses_End.Count == 0)
            {
                return result;
            }

            result.AddRange(mollierProcesses_Start);
            result.AddRange(mollierProcesses_End);

            mollierProcesses_Temp.RemoveAll(x => mollierProcesses_Start.Contains(x) || mollierProcesses_End.Contains(x));
            
            foreach(IMollierProcess mollierProcess in mollierProcesses_Start)
            {
                List<IMollierProcess> mollierProcesses_Temp_End = Connected(mollierProcesses_Temp, mollierProcess.End, tolerance);
                if(mollierProcesses_Temp_End != null)
                {
                    foreach(IMollierProcess mollierProcess_Temp in mollierProcesses_Temp_End)
                    {
                        if(!result.Contains(mollierProcess_Temp))
                        {
                            result.Add(mollierProcess_Temp);
                        }
                    }

                }
            }

            foreach (IMollierProcess mollierProcess in mollierProcesses_End)
            {
                List<IMollierProcess> mollierProcesses_Temp_Start = Connected(mollierProcesses_Temp, mollierProcess.Start, tolerance);
                if (mollierProcesses_Temp_Start != null)
                {
                    foreach (IMollierProcess mollierProcess_Temp in mollierProcesses_Temp_Start)
                    {
                        if (!result.Contains(mollierProcess_Temp))
                        {
                            result.Add(mollierProcess_Temp);
                        }
                    }

                }
            }

            return result;
        }
    }
}
