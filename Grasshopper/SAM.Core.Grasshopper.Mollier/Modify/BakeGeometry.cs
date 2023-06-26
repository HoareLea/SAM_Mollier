using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Modify
    {
        public static void BakeGeometry(this RhinoDoc rhinoDoc, ChartType chartType, IEnumerable<IMollierProcess> mollierProcesses, IEnumerable<IMollierPoint> mollierPoints)
        {
            global::Rhino.DocObjects.Tables.LayerTable layerTable = rhinoDoc?.Layers;
            if (layerTable == null)
            {
                return;
            }

            Layer layer_SAM = Rhino.Modify.AddLayer(layerTable, string.Format("SAM{0}_", chartType.Description()));
            if (layer_SAM == null)
            {
                return;
            }

            int index = -1;

            if(mollierProcesses != null && mollierProcesses.Count() != 0)
            {
                index = layerTable.Add();
                Layer layer_Process = layerTable[index];
                layer_Process.Name = "Process";
                layer_Process.ParentLayerId = layer_SAM.Id;

                ObjectAttributes objectAttributes = rhinoDoc.CreateDefaultAttributes();

                List<Guid> guids = new List<Guid>();
                foreach (IMollierProcess mollierProcess in mollierProcesses)
                {
                    if (mollierProcess == null)
                    {
                        continue;
                    }

                    string name = mollierProcess.Name();

                    Layer layer = Rhino.Modify.GetLayer(layerTable, layer_Process.Id, name, mollierProcess.Color());
                    Polyline polyline = Convert.ToRhino_Polyline(mollierProcess, chartType, 0);
                    objectAttributes.LayerIndex = layer.Index;

                    Guid guid = rhinoDoc.Objects.AddCurve(polyline?.ToPolylineCurve(), objectAttributes);
                    if (guid != Guid.Empty)
                    {
                        guids.Add(guid);
                    }
                }
            }

            if(mollierPoints != null && mollierPoints.Count() != 0)
            {
                index = layerTable.Add();
                Layer layer_Point = layerTable[index];
                layer_Point.Name = "Points";
                layer_Point.ParentLayerId = layer_SAM.Id;

                ObjectAttributes objectAttributes = rhinoDoc.CreateDefaultAttributes();

                List<Guid> guids = new List<Guid>();
                foreach (IMollierPoint mollierPoint in mollierPoints)
                {
                    if (mollierPoint == null)
                    {
                        continue;
                    }

                    Point3d point3d = Convert.ToRhino_Point3d(mollierPoint, chartType, 0);
                    objectAttributes.LayerIndex = layer_Point.Index;

                    Guid guid = rhinoDoc.Objects.AddPoint(point3d, objectAttributes);
                    if (guid != Guid.Empty)
                    {
                        guids.Add(guid);
                    }
                }
            }

        }
    }
}
