using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;

namespace SAM.Core.Grasshopper.Mollier
{
    public class GooMollierPoint : GooJSAMObject<MollierPoint>
    {
        public GooMollierPoint()
            : base()
        {
        }

        public GooMollierPoint(MollierPoint mollierPoint)
            : base(mollierPoint)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMollierPoint(Value);
        }
    }

    public class GooMollierPointParam : GH_PersistentParam<GooMollierProcess>
    {
        public override Guid ComponentGuid => new Guid("0dbb3c67-374c-4534-b1c1-83d825566bc1");

        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        public GooMollierPointParam()
            : base("MollierPoint", "MollierPoint", "SAM Core Mollier MollierPoint", "Params", "SAM")
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