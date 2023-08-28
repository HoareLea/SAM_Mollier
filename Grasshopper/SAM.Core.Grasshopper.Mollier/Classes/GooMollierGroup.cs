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
    public class GooMollierGroup : GooJSAMObject<IMollierGroup>
    {
        public GooMollierGroup()
            : base()
        {
        }

        public GooMollierGroup(IMollierGroup mollierGroup)
            : base(mollierGroup)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMollierGroup(Value);
        }
    }

    public class GooMollierGroupParam : GH_PersistentParam<GooMollierPoint>
    {
        public override Guid ComponentGuid => new Guid("2bc4d547-aa2b-473e-b42e-575722ce8367");

        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        public GooMollierGroupParam()
            : base("MollierGroup", "MollierGroup", "SAM Core Mollier MollierGroup", "Params", "SAM")
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