using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierApparatusDewPoint : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("28bba6a2-41f3-4912-a438-a3a8e3dfe969");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Mollier;

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_start", NickName = "_start", Description = "Start Mollier Point", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_end", NickName = "_end", Description = "End Mollier Point", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "apparatusDewPoint", NickName = "apparatusDewPoint", Description = "Apparatus Dew Point", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "efficiency", NickName = "efficiency", Description = "Efficiency [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierApparatusDewPoint()
          : base("SAMMollier.ApparatusDewPoint", "SAMMollier.ApparatusDewPoint",
              "Calculates apparatus dew point (ADP)",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            MollierPoint start = null;
            index = Params.IndexOfInputParam("_start");
            if (index == -1 || !dataAccess.GetData(index, ref start) || start == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            MollierPoint end = null;
            index = Params.IndexOfInputParam("_end");
            if (index == -1 || !dataAccess.GetData(index, ref end) || end == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            MollierPoint apparatusDewPoint = Core.Mollier.Query.ApparatusDewPoint(start, end);
            index = Params.IndexOfOutputParam("apparatusDewPoint");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierPoint(apparatusDewPoint));
            }

            if(apparatusDewPoint != null)
            {
                index = Params.IndexOfOutputParam("efficiency");
                if (index != -1)
                {
                    dataAccess.SetData(index, start.Distance(end) / start.Distance(apparatusDewPoint));
                }
            }
        }
    }
}