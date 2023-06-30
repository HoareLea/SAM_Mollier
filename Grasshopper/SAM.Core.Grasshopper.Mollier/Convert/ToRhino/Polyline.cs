using Rhino.Geometry;
using SAM.Core.Mollier;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Convert
    {
        public static Polyline ToRhino_Polyline(this IEnumerable<MollierPoint> mollierPoints, ChartType chartType, double z = 0)
        {
            if (mollierPoints == null || mollierPoints.Count() < 2 || chartType == ChartType.Undefined)
            {
                return null;
            }

            Polyline result = new Polyline();
            foreach (MollierPoint mollierPoint in mollierPoints)
            {

                if (mollierPoint == null || !mollierPoint.IsValid())
                {
                    return null;
                }

                Point3d point3d = mollierPoint.ToRhino_Point3d(chartType, z);
                if(!point3d.IsValid || point3d == Point3d.Unset)
                {
                    return null;
                }

                result.Add(point3d);
            }

            return result;
        }

        public static Polyline ToRhino_Polyline(this MollierProcess mollierProcess, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            return ToRhino_Polyline(new MollierPoint[] { mollierProcess.Start, mollierProcess.End}, chartType, z);
        }

        public static Polyline ToRhino_Polyline(this IMollierProcess mollierProcess, ChartType chartType, double z = 0)
        {
            if(mollierProcess == null)
            {
                return null;
            }

            if(mollierProcess is UIMollierProcess)
            {
                return ToRhino_Polyline((UIMollierProcess)mollierProcess, chartType, z);
            }

            return ToRhino_Polyline(mollierProcess is UIMollierProcess ? ((UIMollierProcess)mollierProcess).MollierProcess : mollierProcess as MollierProcess, chartType, z);
        }

        public static Polyline ToRhino_Polyline(this UIMollierProcess uIMollierProcess, ChartType chartType, double z = 0)
        {
            MollierProcess mollierProcess = uIMollierProcess?.MollierProcess;
            if(mollierProcess == null)
            {
                return null;
            }

            Polyline result = null;
            if (uIMollierProcess is UICoolingProcess && ((UICoolingProcess)uIMollierProcess).Realistic)
            {
                List<MollierPoint> mollierPoints = Core.Mollier.Query.ProcessMollierPoints((CoolingProcess)mollierProcess);
                result = mollierPoints.ToRhino_Polyline(chartType, z);
            }
            else
            {
                result = mollierProcess?.ToRhino_Polyline(chartType, z);
            }

            return result;
        }
    }
}
