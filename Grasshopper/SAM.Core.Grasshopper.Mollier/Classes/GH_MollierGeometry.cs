using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Drawing;
namespace SAM.Core.Grasshopper.Mollier
{
    public class GH_MollierGeometry : IJSAMObject
    {
        private List<Point3d> point3Ds;
        private System.Drawing.Color color;
        public bool FromJObject(JObject jObject)
        {
            throw new NotImplementedException();
        }

        public JObject ToJObject()
        {
            throw new NotImplementedException();
        }

        public GH_MollierGeometry(List<Point3d> point3Ds, Color color)
        {
            this.point3Ds = point3Ds;
            this.color = color;
        }
        public GH_MollierGeometry(PolylineCurve polylineCurve, Color color)
        {
            List<Point3d> points3d = new List<Point3d>();
            for (int i = 0; i < polylineCurve.PointCount; i++)
            {
                points3d.Add(polylineCurve.Point(i));
            }
            this.point3Ds = points3d;
            this.color = color;
        }

        public List<Point3d> Points
        {
            get
            {
                return point3Ds;
            }   
        }
        public Color Color
        {
            get
            {
                return color;
            }
        }
    }
}