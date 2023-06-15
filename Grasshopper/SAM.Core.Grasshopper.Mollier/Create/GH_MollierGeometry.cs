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

            Rhino.Geometry.Polyline polyLine = new Rhino.Geometry.Polyline();
            foreach (MollierPoint mollierPoint in mollierPoints)
            {
                double x = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature;
                double y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                polyLine.Add(x, y, z);
            }

            return new GH_MollierGeometry(new Rhino.Geometry.PolylineCurve(polyLine), color);
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
