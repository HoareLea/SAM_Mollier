using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        //public static List<IMollierGroup> Group_(this IEnumerable<IMollierProcess> mollierProcesses, bool sort = true, double tolerance = Tolerance.MacroDistance)
        //{
        //    // Old method 
        //    if(mollierProcesses == null)
        //    {
        //        return null;
        //    }

        //    List<MollierPoint> mollierPoints = mollierProcesses.MollierPoints(tolerance);
        //    if(mollierPoints == null || mollierPoints.Count == 0)
        //    {
        //        return null;
        //    }

        //    List<IMollierProcess> mollierProcesses_Temp = new List<IMollierProcess>(mollierProcesses);

        //    List<IMollierGroup> result = new List<IMollierGroup>();
        //    int index = 1;
        //    foreach(MollierPoint mollierPoint in mollierPoints)
        //    {
        //        List<IMollierProcess> mollierProcesses_Connected = Connected(mollierProcesses_Temp, mollierPoint, tolerance);
        //        if(mollierProcesses_Connected == null || mollierProcesses_Connected.Count == 0)
        //        {
        //            continue;
        //        }

        //        MollierGroup mollierGroup = new MollierGroup(string.Format("Group {0}", index));
        //        index++;
        //        result.Add(mollierGroup);

        //        if(sort)
        //        {
        //            mollierProcesses_Connected = Sort(mollierProcesses_Connected);
        //        }

        //        mollierProcesses_Connected.ForEach(x => mollierGroup.Add(x));

        //        mollierProcesses_Temp.RemoveAll(x => mollierProcesses_Connected.Contains(x));

        //        if(mollierProcesses_Temp == null || mollierProcesses_Temp.Count == 0)
        //        {
        //            break;
        //        }
        //    }

        //    return result;

        //}

        public static List<IMollierGroup> Group(this IEnumerable<IMollierProcess> mollierProcesses, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierProcesses == null)
            {
                return null;
            }
            List<IMollierGroup> result = new List<IMollierGroup>();

            List<int> nextProcessID = new List<int>();
            List<bool> visited = new List<bool>();
            List<int> previousProcessCount = new List<int>();

            int none = -1;
            for (int i = 0; i < mollierProcesses.Count(); i++)
            {
                nextProcessID.Add(none);
                visited.Add(false);
                previousProcessCount.Add(0);
            }

            for (int i = 0; i < mollierProcesses.Count(); i++)
            {
                MollierPoint end = mollierProcesses.ElementAt(i).End;
                if (end == null) continue;

                for (int j = 0; j < mollierProcesses.Count(); j++)
                {
                    MollierPoint start = mollierProcesses.ElementAt(j).Start;
                    if (start == null || i == j) continue;

                    if (start.AlmostEqual(end, tolerance))
                    {
                        nextProcessID[i] = j;
                        previousProcessCount[j]++;
                        break;
                    }
                }
            }

            List<int> startingProcessesID = new List<int>();
            for (int i = 0; i < mollierProcesses.Count(); i++)
            {
                if (previousProcessCount[i] == 0 && !(mollierProcesses.ElementAt(i) is RoomProcess))
                {
                    startingProcessesID.Add(i);
                }
            }

            int index = 0;
            for (int i = 0; i < startingProcessesID.Count; i++)
            {
                int startingProcessID = startingProcessesID[i];
                if (visited[startingProcessID] == true) continue;

                MollierGroup mollierGroup = new MollierGroup(string.Format("Group {0}", index));
                index++;

                int currentProcessID = startingProcessID;
                while (visited[currentProcessID] == false)
                {
                    mollierGroup.Add(mollierProcesses.ElementAt(currentProcessID));
                    visited[currentProcessID] = true;

                    if (nextProcessID[currentProcessID] == none) break;
                    currentProcessID = nextProcessID[currentProcessID];
                }
                result.Add(mollierGroup);
            }


            for (int i = 0; i < mollierProcesses.Count(); i++)
            {
                if (visited[i] == true) continue;

                MollierGroup mollierGroup = new MollierGroup(string.Format("Group {0}", index));
                index++;

                int currentProcessID = i;
                while (visited[currentProcessID] == false)
                {
                    mollierGroup.Add(mollierProcesses.ElementAt(currentProcessID));
                    visited[currentProcessID] = true;

                    if (nextProcessID[currentProcessID] == none) break;
                    currentProcessID = nextProcessID[currentProcessID];
                }
                result.Add(mollierGroup);
            }

            return result;
        }
    }
}
