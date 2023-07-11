using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Convert
    {
        public static global::Rhino.Geometry.Point3d ToRhino_Point3d(this IMollierPoint mollierPoint, ChartType chartType, double z = 0)
        {
            if (mollierPoint == null || chartType == ChartType.Undefined)
            {
                return global::Rhino.Geometry.Point3d.Unset;
            }

            MollierPoint mollierPoint_Temp = mollierPoint is UIMollierPoint ? ((UIMollierPoint)mollierPoint).MollierPoint : mollierPoint as MollierPoint;
            if(mollierPoint_Temp == null)
            {
                return global::Rhino.Geometry.Point3d.Unset;
            }

            if (!mollierPoint_Temp.IsValid())
            {
                return global::Rhino.Geometry.Point3d.Unset;
            }

            double humidityRatio = mollierPoint_Temp.HumidityRatio;

            double diagramTemperature = Core.Mollier.Query.DiagramTemperature(mollierPoint_Temp);
            if (mollierPoint_Temp.SaturationHumidityRatio() < humidityRatio)
            {
                if (Core.Mollier.Query.TryFindDiagramTemperature(mollierPoint_Temp, out double diagramTemperature_Temp))
                {
                    diagramTemperature = diagramTemperature_Temp;
                }
            }

            double x = chartType == ChartType.Mollier ? humidityRatio * 1000 : mollierPoint_Temp.DryBulbTemperature;
            double y = chartType == ChartType.Mollier ? diagramTemperature : humidityRatio * 1000;
            if (double.IsNaN(x) || double.IsNaN(y))
            {
                return global::Rhino.Geometry.Point3d.Unset;
            }

            return new global::Rhino.Geometry.Point3d(x, y, z);
        }
    }
}
