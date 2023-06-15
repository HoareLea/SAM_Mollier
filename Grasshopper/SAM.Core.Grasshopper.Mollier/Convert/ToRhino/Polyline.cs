using SAM.Core.Mollier;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Convert
    {
        public static global::Rhino.Geometry.Polyline ToRhino_Polyline(this IEnumerable<MollierPoint> mollierPoints, ChartType chartType, double z = 0)
        {
            if (mollierPoints == null || mollierPoints.Count() < 2 || chartType == ChartType.Undefined)
            {
                return null;
            }

            global::Rhino.Geometry.Polyline result = new global::Rhino.Geometry.Polyline();
            foreach (MollierPoint mollierPoint in mollierPoints)
            {

                if (mollierPoint == null || !mollierPoint.IsValid())
                {
                    return null;
                }

                global::Rhino.Geometry.Point3d point3d = mollierPoint.ToRhino_Point3d(chartType, z);
                if(!point3d.IsValid || point3d == global::Rhino.Geometry.Point3d.Unset)
                {
                    return null;
                }

                result.Add(point3d);
            }

            return result;
        }

        public static global::Rhino.Geometry.Polyline ToRhino_Polyline(this MollierProcess mollierProcess, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            return ToRhino_Polyline(new MollierPoint[] { mollierProcess.Start, mollierProcess.End}, chartType, z);
        }

        public static global::Rhino.Geometry.Polyline ToRhino_Polyline(this IMollierProcess mollierProcess, ChartType chartType, double z = 0)
        {
            if(mollierProcess == null)
            {
                return null;
            }

            return ToRhino_Polyline(mollierProcess is UIMollierProcess ? ((UIMollierProcess)mollierProcess).MollierProcess : mollierProcess as MollierProcess, chartType, z);
        }
    }
}
