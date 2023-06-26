using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SAM.Core.Grasshopper.Mollier
{
    public class GooMollierPoint : GooJSAMObject<IMollierPoint>
    {
        public GooMollierPoint()
            : base()
        {
        }

        public GooMollierPoint(IMollierPoint mollierPoint)
            : base(mollierPoint)
        {
        }

        public override bool CastTo<Y>(ref Y target)
        {
            if (typeof(Y).IsAssignableFrom(typeof(MollierPoint)))
            {
                if (Value is UIMollierPoint)
                {
                    target = (Y)(object)((UIMollierPoint)Value).MollierPoint;
                    return true;
                }
            }

            if (typeof(Y).IsAssignableFrom(typeof(UIMollierPoint)))
            {
                if (Value is MollierPoint)
                {
                    target = (Y)(object)new UIMollierPoint((MollierPoint)Value);
                    return true;
                }
            }

            return base.CastTo(ref target);
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMollierPoint(Value);
        }
    }

    public class GooMollierPointParam : GH_PersistentParam<GooMollierPoint>
    {
        public override Guid ComponentGuid => new Guid("0dbb3c67-374c-4534-b1c1-83d825566bc1");

        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        public GooMollierPointParam()
            : base("MollierPoint", "MollierPoint", "SAM Core Mollier MollierPoint", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMollierPoint> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMollierPoint value)
        {
            throw new NotImplementedException();
        }

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Save As...", Menu_SaveAs, VolatileData.AllData(true).Any());

            //Menu_AppendSeparator(menu);

            base.AppendAdditionalMenuItems(menu);
        }

        private void Menu_SaveAs(object sender, EventArgs e)
        {
            Grasshopper.Query.SaveAs(VolatileData);
        }
    }
}