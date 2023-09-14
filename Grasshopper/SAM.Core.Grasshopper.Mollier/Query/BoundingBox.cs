﻿using Rhino.Geometry;
using SAM.Core.Mollier;
using System.Collections.Generic;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Query
    {
        public static BoundingBox BoundingBox(this MollierChartObject mollierChartObject)
        {
            if(mollierChartObject == null)
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            return BoundingBox(mollierChartObject.UIMollierObject, mollierChartObject.ChartType, mollierChartObject.Z);

        }

        public static BoundingBox BoundingBox(this IMollierObject uIMollierObject, ChartType chartType = ChartType.Mollier, double z = 0)
        {
            if(uIMollierObject == null || chartType == ChartType.Undefined || double.IsNaN(z))
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            if(uIMollierObject is IMollierPoint)
            {
                return BoundingBox((IMollierPoint)uIMollierObject,chartType, z);
            }

            if (uIMollierObject is IMollierProcess)
            {
                return BoundingBox((IMollierProcess)uIMollierObject, chartType, z);
            }

            if (uIMollierObject is IMollierCurve)
            {
                return BoundingBox((IMollierCurve)uIMollierObject, chartType, z);
            }

            if(uIMollierObject is IMollierGroup)
            {
                List<IMollierGroupable> mollierObjects = new List<IMollierGroupable>();

                if(uIMollierObject is UIMollierGroup)
                {
                    mollierObjects = ((UIMollierGroup)uIMollierObject).GetObjects<IMollierGroupable>();
                }
                else if(uIMollierObject is MollierGroup)
                {
                    mollierObjects = ((MollierGroup)uIMollierObject).GetObjects<IMollierGroupable>();
                }
                else
                {
                    throw new System.NotImplementedException();
                }

                List<BoundingBox> boudingBoxes = mollierObjects.ConvertAll(x => BoundingBox(x, chartType, z));
                BoundingBox result = global::Rhino.Geometry.BoundingBox.Empty;

                foreach(BoundingBox boundingBox in boudingBoxes)
                {
                    result.Union(boundingBox);
                }

                return result;
            }

            throw new System.NotImplementedException();
        }

        public static BoundingBox BoundingBox(this IMollierPoint mollierPoint, ChartType chartType = ChartType.Mollier, double z = 0)
        {
            if(mollierPoint == null)
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            Point3d point3D = mollierPoint.ToRhino_Point3d(chartType, z);
            if(point3D == Point3d.Unset)
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            return new BoundingBox(point3D, point3D);
        }

        public static BoundingBox BoundingBox(this IMollierProcess mollierProcess, ChartType chartType = ChartType.Mollier, double z = 0)
        {
            if (mollierProcess == null)
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            Polyline polyline = mollierProcess.ToRhino_Polyline(chartType, z);
            if (polyline == null)
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            return new BoundingBox(polyline);
        }

        public static BoundingBox BoundingBox(this IMollierCurve mollierCurve, ChartType chartType = ChartType.Mollier, double z = 0)
        {
            if (mollierCurve == null)
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            if(mollierCurve is IMollierProcess)
            {
                return BoundingBox((IMollierProcess)mollierCurve, chartType, z);
            }

            Polyline polyline = mollierCurve.ToRhino_Polyline(chartType, z);
            if (polyline == null)
            {
                return global::Rhino.Geometry.BoundingBox.Empty;
            }

            return new BoundingBox(polyline);
        }
    }
}
