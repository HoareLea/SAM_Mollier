using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SAM.Core.Grasshopper.Mollier
{
    public class GooMollierGeometry : GH_Goo<GH_MollierGeometry>, IGH_PreviewData, IGH_BakeAwareData
    {
        public GooMollierGeometry()
            : base()
        {
        }

        public GooMollierGeometry(GH_MollierGeometry curve)
            : base(curve)
        {
        }
        public override bool IsValid => Value != null;

        public override string TypeName
        {
            get
            {
                return "MollierGeometry";
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "MollierGeometry";
            }
        }
        public override string ToString()
        {
            return "MollierGeometry";
        }
        public BoundingBox ClippingBox
        {
            get
            {
                if (Value == null)
                    return BoundingBox.Empty;

                return new BoundingBox(Value.Point3ds);
            }
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMollierGeometry(Value);
        }

        public void DrawViewportWires(GH_PreviewWireArgs args)
        {
            if (Value == null)
                return;

            System.Drawing.Color color = Value.Color;

            if(Value.Point3ds.Count == 1)
            {
                args.Pipeline.DrawPoint(Value.Point3ds[0], color);
                return;
            }
            args.Pipeline.DrawPolyline(Value.Point3ds, color);
        }

        public void DrawViewportMeshes(GH_PreviewMeshArgs args)
        {
            //if (Value == null)
            //    return;

            //Face3D face3D = Value.GetFace3D();
            //if (face3D == null)
            //    return;

            //if (!ShowAll)
            //{
            //    Point3D point3D_CameraLocation = Geometry.Rhino.Convert.ToSAM(RhinoDoc.ActiveDoc.Views.ActiveView.ActiveViewport.CameraLocation);
            //    if (point3D_CameraLocation == null)
            //        return;

            //    double distance = face3D.Distance(point3D_CameraLocation);
            //    if (distance < 8 || distance > 15)
            //        return;
            //}

            //global::Rhino.Display.DisplayMaterial displayMaterial = Query.DisplayMaterial(Value.GH_CurveType);
            //if (displayMaterial == null)
            //    displayMaterial = args.Material;

            //Brep brep = Geometry.Rhino.Convert.ToRhino_Brep(face3D);
            //if (brep == null)
            //    return;

            //args.Pipeline.DrawBrepShaded(brep, displayMaterial);

            //List<Aperture> apertures = Value.Apertures;
            //if (apertures != null)
            //{
            //    foreach (Aperture aperture in apertures)
            //        foreach (IClosedPlanar3D closedPlanar3D in aperture.GetFace3D().GetEdge3Ds())
            //        {
            //            global::Rhino.Display.DisplayMaterial displayMaterial_Aperture = Query.DisplayMaterial(aperture.ApertureConstruction.ApertureType);
            //            if (displayMaterial_Aperture == null)
            //                displayMaterial_Aperture = args.Material;

            //            GooSAMGeometry gooSAMGeometry_Aperture = new GooSAMGeometry(closedPlanar3D);
            //            gooSAMGeometry_Aperture.DrawViewportMeshes(args, displayMaterial_Aperture);
            //        }
            //}
        }

        public bool BakeGeometry(RhinoDoc doc, ObjectAttributes att, out Guid obj_guid)
        {
            obj_guid = doc.Objects.AddCurve(new PolylineCurve(Value.Point3ds), att);
            return true;

        }

        public override bool CastFrom(object source)
        {
            if (source is GH_Curve)
            {
                List<Point3d> points3d = new List<Point3d>();
                for(int i = 0; i < Value.Point3ds.Count; i++)
                {
                    points3d.Add(Value.Point3ds[i]);
                }
                Value = new GH_MollierGeometry(points3d, System.Drawing.Color.Black);
                return true;
            }

            if (typeof(IGH_Goo).IsAssignableFrom(source.GetType()))
            {
                object object_Temp = null;

                try
                {
                    object_Temp = (source as dynamic).Value;
                }
                catch
                {
                }

                if (object_Temp is GH_Curve)
                {
                    List<Point3d> points3d = new List<Point3d>();
                    for (int i = 0; i < Value.Point3ds.Count; i++)
                    {
                        points3d.Add(Value.Point3ds[i]);
                    }
                    Value = new GH_MollierGeometry(points3d, System.Drawing.Color.Black);
                    return true;
                }
            }

            return base.CastFrom(source);
        }

        public override bool CastTo<Y>(ref Y target)
        {
            if (Value == null)
                return false;

            //if (typeof(Y).IsAssignableFrom(typeof(GH_Mesh)))
            //{
            //    target = (Y)(object)Value.ToGrasshopper_Mesh();
            //    return true;
            //}
            //else if (typeof(Y).IsAssignableFrom(typeof(GH_Brep)))
            //{
            //    target = (Y)(object)Value.GetFace3D()?.ToGrasshopper_Brep();
            //    return true;
            //}

            return base.CastTo(ref target);
        }
    }

    public class GooMollierGeometryParam : GH_PersistentParam<GooMollierGeometry>, IGH_PreviewObject, IGH_BakeAwareObject
    {
        public override Guid ComponentGuid => new Guid("bbf6a119-0bcb-402c-9dd9-b6b45112c169");

        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        bool IGH_PreviewObject.Hidden { get; set; }

        bool IGH_PreviewObject.IsPreviewCapable => !VolatileData.IsEmpty;

        BoundingBox IGH_PreviewObject.ClippingBox => Preview_ComputeClippingBox();

        public bool IsBakeCapable => true;

        void IGH_PreviewObject.DrawViewportMeshes(IGH_PreviewArgs args)
        {
            //foreach (var variable in VolatileData.AllData(true))
            //{
            //    GooGH_Curve gooGH_Curve = variable as GooGH_Curve;
            //    if (gooGH_Curve == null)
            //        continue;

            //    gooGH_Curve.ShowAll = showAll;
            //}

            //Preview_DrawMeshes(args);
        }

        void IGH_PreviewObject.DrawViewportWires(IGH_PreviewArgs args)
        {
            //foreach (var variable in VolatileData.AllData(true))
            //{tea
            //    GooGH_Curve gooGH_Curve = variable as GooGH_Curve;
            //    if (gooGH_Curve == null)
            //        continue;

            //    gooGH_Curve.ShowAll = showAll;
            //}
            //TODO: implement draw

            Preview_DrawWires(args);
        }

        public GooMollierGeometryParam()
            : base("MollierGeometry ", "MollierGeometry",
              "Draw points, lines and processes",
              "SAM", "Mollier")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMollierGeometry> values)
        {
            throw new Exception();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMollierGeometry value)
        {
            throw new Exception();
        }

        public void BakeGeometry(RhinoDoc doc, List<Guid> obj_ids)
        {
            BakeGeometry(doc, doc.CreateDefaultAttributes(), obj_ids);
        }

        public void BakeGeometry(RhinoDoc doc, ObjectAttributes att, List<Guid> obj_ids)
        {
            foreach (var value in VolatileData.AllData(true))
            {
                Guid uuid = default;
                (value as IGH_BakeAwareData)?.BakeGeometry(doc, att, out uuid);
                obj_ids.Add(uuid);
            }
        }


        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            //Menu_AppendItem(menu, "Show All", Menu_ShowAll, VolatileData.AllData(true).Any(), showAll).Tag = showAll;

            //Menu_AppendItem(menu, "Bake By Type", Menu_BakeByGH_CurveType, VolatileData.AllData(true).Any());
            //Menu_AppendItem(menu, "Bake By Construction", Menu_BakeByConstruction, VolatileData.AllData(true).Any());
            //Menu_AppendItem(menu, "Bake By Discharge Coefficient", Menu_BakeByDischargeCoefficient, VolatileData.AllData(true).Any());
            Menu_AppendItem(menu, "Save As...", Menu_SaveAs, VolatileData.AllData(true).Any());

            //Menu_AppendSeparator(menu);

            base.AppendAdditionalMenuItems(menu);
        }

        private void Menu_SaveAs(object sender, EventArgs e)
        {
            Query.SaveAs(VolatileData);
        }
        public override bool Write(GH_IWriter writer)
        {
            //liczbe punktow, punkty , kolor
            //writer.SetInt32("points_number", )
            //writer.SetPoint3D();
            //writer.SetDrawingColor("")
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {

            return base.Read(reader);
        }
    }
}