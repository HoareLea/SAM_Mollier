using SAM.Core.Mollier;
using SAM.Geometry.Planar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.Mollier
{
    public static partial class Convert
    {
        public static AirHandlingUnit ToSAM_AirHandlingUnit(this MollierGroup mollierGroup, double supplyAirFlow, double tolerance = SAM.Core.Tolerance.MacroDistance)
        {
            if (mollierGroup == null)
                return null;

            List<IMollierProcess> mollierProcesses = new List<IMollierProcess>();
            List<IMollierProcess> mollierProcesses_Winter = new List<IMollierProcess>();
            List<IMollierProcess> mollierProcesses_Summer = new List<IMollierProcess>();
            foreach (IMollierGroupable mollierGroupable in mollierGroup)
            {
                List<IMollierProcess> mollierProcesses_Temp = mollierProcesses;

                if (mollierGroupable is MollierGroup)
                {
                    MollierGroup mollierGroup_Temp = (MollierGroup)mollierGroupable;
                    if (mollierGroup_Temp.Name.ToLower() == "winter")
                    {
                        mollierProcesses_Temp = mollierProcesses_Winter;
                    }
                    else if (mollierGroup_Temp.Name.ToLower() == "summer")
                    {
                        mollierProcesses_Temp = mollierProcesses_Summer;
                    }

                    foreach (IMollierGroupable mollierGroupable_MollierGroup in mollierGroup_Temp)
                    {
                        if (mollierGroupable_MollierGroup is IMollierProcess)
                        {
                            mollierProcesses_Temp.Add((IMollierProcess)mollierGroupable_MollierGroup);
                        }
                    }

                }
                else if (mollierGroupable is IMollierProcess)
                {
                    mollierProcesses.Add((IMollierProcess)mollierGroupable);
                }
            }

            if (mollierProcesses != null && mollierProcesses.Count > 0)
            {
                List<Tuple<MollierPoint, List<IMollierProcess>>> tuples = mollierProcesses.MollierPoints(tolerance).ConvertAll(x => new Tuple<MollierPoint, List<IMollierProcess>>(x, mollierProcesses?.MollierProcesses(x, tolerance)));
                tuples.RemoveAll(x => x.Item2 == null || x.Item2.Count != 1);
                if(tuples != null && tuples.Count != 0)
                {
                    foreach(Tuple<MollierPoint, List<IMollierProcess>> tuple in tuples)
                    {
                        if(mollierProcesses == null || mollierProcesses.Count == 0)
                        {
                            break;
                        }

                        List<IMollierProcess> mollierProcesses_Connected = mollierProcesses.Connected(tuple.Item1, tolerance);
                        if(mollierProcesses_Connected == null || mollierProcesses_Connected.Count == 0)
                        {
                            continue;
                        }

                        mollierProcesses.RemoveAll(x => mollierProcesses_Connected.Contains(x));

                        List<MollierPoint> mollierPoints = mollierProcesses_Connected.MollierPoints(tolerance);

                        List<Tuple<MollierPoint, IMollierProcess>> tuples_Temp = tuples.FindAll(x => mollierPoints.Find(y => y.AlmostEqual(x.Item1, tolerance)) != null).ConvertAll(x => new Tuple<MollierPoint, IMollierProcess>(x.Item1, x.Item2[0]));

                        MollierPoint mollierPoint_Start = tuples_Temp.Find(x => x.Item2.Start.AlmostEqual(x.Item1, tolerance))?.Item1;
                        MollierPoint mollierPoint_End = tuples_Temp.Find(x => x.Item2.End.AlmostEqual(x.Item1, tolerance))?.Item1;

                        if(mollierPoint_Start.DryBulbTemperature > mollierPoint_End.DryBulbTemperature)
                        {
                            mollierProcesses_Summer.AddRange(mollierProcesses_Connected);
                        }
                        else
                        {
                            mollierProcesses_Winter.AddRange(mollierProcesses_Connected);
                        }

                    }
                }
            }

            List<IMollierGroup> mollierGroups_Summer = mollierProcesses_Summer.Group(tolerance);
            List<IMollierGroup> mollierGroups_Winter = mollierProcesses_Winter.Group(tolerance);

            if((mollierGroups_Summer == null || mollierProcesses_Summer.Count == 0) && (mollierGroups_Winter == null || mollierProcesses_Winter.Count == 0))
            {
                return null;
            }

            List<Type> types_Summer = Query.SimpleEquipmentTypes(mollierGroups_Summer?.FirstOrDefault()?.ToList().ConvertAll(x => (IMollierProcess)x));
            List<Type> types_Winter = Query.SimpleEquipmentTypes(mollierGroups_Winter?.FirstOrDefault()?.ToList().ConvertAll(x => (IMollierProcess)x));



            throw new NotImplementedException();
        }
    }
}