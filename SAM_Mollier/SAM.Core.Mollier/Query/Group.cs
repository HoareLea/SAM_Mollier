using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<IMollierGroup> Group(this IEnumerable<IMollierProcess> mollierProcesses, bool sort = true, double tolerance = Tolerance.MacroDistance)
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

            List<IMollierProcess> mollierProcesses_Temp = new List<IMollierProcess>(mollierProcesses);

            List<IMollierGroup> result = new List<IMollierGroup>();
            int index = 1;
            foreach(MollierPoint mollierPoint in mollierPoints)
            {
                List<IMollierProcess> mollierProcesses_Connected = Connected(mollierProcesses_Temp, mollierPoint, tolerance);
                if(mollierProcesses_Connected == null || mollierProcesses_Connected.Count == 0)
                {
                    continue;
                }

                MollierGroup mollierGroup = new MollierGroup(string.Format("Group {0}", index));
                index++;

                if(sort)
                {
                    mollierProcesses_Connected = Sort(mollierProcesses_Connected);
                }

                mollierProcesses_Connected.ForEach(x => mollierGroup.Add(x));

                mollierProcesses_Temp.RemoveAll(x => mollierProcesses_Connected.Contains(x));

                if(mollierProcesses_Temp == null || mollierProcesses_Temp.Count == 0)
                {
                    break;
                }
            }

            return result;

        }
    }
}
