using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SAM.Core.Grasshopper.Mollier
{
    public class GooMollierObject : GooJSAMObject<IMollierObject>
    {
        public GooMollierObject()
            : base()
        {
        }

        public GooMollierObject(IMollierObject mollierObject)
            : base(mollierObject)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMollierObject(Value);
        }

        public override bool CastFrom(object source)
        {
            return base.CastFrom(source);
        }

        public override bool CastTo<Y>(ref Y target)
        {
            return base.CastTo(ref target);
        }
    }

    public class GooMollierObjectParam : GH_PersistentParam<GooMollierObject>
    {
        public override Guid ComponentGuid => new Guid("598315c1-c756-4bbb-b66e-c6640e56ff99");

        protected override Bitmap Icon => Resources.SAM_Small;

        public GooMollierObjectParam()
            : base("MollierObject", "MollierObject", "SAM Core MollierObject", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMollierObject> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMollierObject value)
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