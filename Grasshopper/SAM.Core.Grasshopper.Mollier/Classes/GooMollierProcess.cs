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
    public class GooMollierProcess : GooJSAMObject<IMollierProcess>
    {
        public GooMollierProcess()
            : base()
        {
        }

        public GooMollierProcess(IMollierProcess mollierProcess)
            : base(mollierProcess)
        {
        }

        public GooMollierProcess(IMollierProcess mollierProcess, Color color, string start_Label, string process_Label, string end_Label)
            : base((color == Color.Empty && start_Label == null && process_Label == null && end_Label == null) ? mollierProcess : new UIMollierProcess(mollierProcess as MollierProcess, color))
        {
            if(Value is UIMollierProcess)
            {
                UIMollierProcess uIMollierProcess = Value as UIMollierProcess;
                uIMollierProcess.UIMollierAppearance.Label = process_Label;
                uIMollierProcess.UIMollierPointAppearance_Start.Label = start_Label;
                uIMollierProcess.UIMollierPointAppearance_End.Label = end_Label;

                if(color.IsEmpty)
                {
                    Color color_Temp = Core.Mollier.Query.Color(uIMollierProcess);
                    if (!color_Temp.IsEmpty)
                    {
                        if (uIMollierProcess.UIMollierAppearance.Color.IsEmpty)
                        {
                            uIMollierProcess.UIMollierAppearance.Color = color_Temp;
                        }

                        if (uIMollierProcess.UIMollierPointAppearance_Start.Color.IsEmpty)
                        {
                            uIMollierProcess.UIMollierPointAppearance_Start.Color = color_Temp;
                        }

                        if (uIMollierProcess.UIMollierPointAppearance_End.Color.IsEmpty)
                        {
                            uIMollierProcess.UIMollierPointAppearance_End.Color = color_Temp;
                        }
                    }
                }
            }
        }
        public GooMollierProcess(IMollierProcess mollierProcess, Color color)
            : base(color == Color.Empty ? mollierProcess : new UIMollierProcess(mollierProcess as MollierProcess, color))
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMollierProcess(Value);
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

    public class GooMollierProcessParam : GH_PersistentParam<GooMollierProcess>
    {
        public override Guid ComponentGuid => new Guid("0e6f78f4-71ed-490d-891e-dcf36bd09771");

        protected override Bitmap Icon => Resources.SAM_Small;

        public GooMollierProcessParam()
            : base("MollierProcess", "MollierProcess", "SAM Core MollierProcess", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMollierProcess> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMollierProcess value)
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