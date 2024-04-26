using Rhino.Geometry;
using SAM.Core.Mollier;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Convert
    {
        public static Polyline ToRhino_Polyline(this IEnumerable<MollierPoint> mollierPoints, ChartType chartType, bool recalculateTemperature = true, double z = 0)
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

                Point3d point3d = mollierPoint.ToRhino_Point3d(chartType, recalculateTemperature, z);
                if(!point3d.IsValid || point3d == Point3d.Unset)
                {
                    return null;
                }

                result.Add(point3d);
            }

            return result;
        }

        public static Polyline ToRhino_Polyline(this IMollierCurve mollierCurve, ChartType chartType, double z = 0)
        {
            if(mollierCurve == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            return ToRhino_Polyline(mollierCurve.MollierPoints, chartType, mollierCurve.ChartDataType != ChartDataType.DiagramTemperature, z);
        }

        public static Polyline ToRhino_Polyline(this MollierProcess mollierProcess, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            return ToRhino_Polyline(new MollierPoint[] { mollierProcess.Start, mollierProcess.End}, chartType, true, z);
        }
    }
}
