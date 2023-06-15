using SAM.Core.Mollier;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Create
    {
        public static GH_MollierGeometry GH_MollierGeometry(this IEnumerable<MollierPoint> mollierPoints, Color color, ChartType chartType, double z = 0)
        {
            if(mollierPoints == null || mollierPoints.Count() == 0 || chartType == ChartType.Undefined || color == Color.Empty)
            {
                return null;
            }

            global::Rhino.Geometry.Polyline polyline = Convert.ToRhino_Polyline(mollierPoints, chartType, z);

            return new GH_MollierGeometry(new global::Rhino.Geometry.PolylineCurve(polyline), color);
        }

        public static GH_MollierGeometry GH_MollierGeometry(this IMollierProcess mollierProcess, Color color, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            if(color == Color.Empty)
            {
                return GH_MollierGeometry(mollierProcess as UIMollierProcess, chartType, z);
            }

            return GH_MollierGeometry(new MollierPoint[] { mollierProcess.Start, mollierProcess.End }, color, chartType, z);
        }

        public static GH_MollierGeometry GH_MollierGeometry(this MollierProcess mollierProcess, Color color, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined || color == Color.Empty)
            {
                return null;
            }

            return GH_MollierGeometry(new MollierPoint[] { mollierProcess.Start, mollierProcess.End }, color, chartType, z);
        }

        public static GH_MollierGeometry GH_MollierGeometry(this UIMollierProcess uIMollierProcess, ChartType chartType, double z = 0)
        {
            if (uIMollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            Color color = Color.Empty;
            UIMollierAppearance uIMollierAppearance = uIMollierProcess.UIMollierAppearance;
            if(uIMollierAppearance != null)
            {
                color = uIMollierAppearance.Color;
            }

            return GH_MollierGeometry(new MollierPoint[] { uIMollierProcess.Start, uIMollierProcess.End }, color, chartType, z);
        }
    }
}
