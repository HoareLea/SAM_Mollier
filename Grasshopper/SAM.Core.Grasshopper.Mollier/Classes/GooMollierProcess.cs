using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;

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

        public override IGH_Goo Duplicate()
        {
            return new GooMollierProcess(Value);
        }
    }

    public class GooMollierProcessParam : GH_PersistentParam<GooMollierProcess>
    {
        public override Guid ComponentGuid => new Guid("0e6f78f4-71ed-490d-891e-dcf36bd09771");

        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

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
    }
}