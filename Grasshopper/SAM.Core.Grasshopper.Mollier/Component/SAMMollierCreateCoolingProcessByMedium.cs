using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierCreateCoolingProcessByMedium : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("86a92a8e-c89a-45f4-97a5-9c2447543098");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_start", NickName = "_start", Description = "Start Point for MollierProcess", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() {Name = "_flowTemperature", NickName = "_flowTemperature", Description = "Flow Temperature [C]", Access = GH_ParamAccess.item}, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_returnTemperature", NickName = "_returnTemperature", Description = "Return Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_efficiency", NickName = "_efficiency", Description = "Efficiency [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "coolingProcess", NickName = "coolingProcess", Description = "Cooling Process", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateCoolingProcessByMedium()
          : base("SAMMollier.CreateCoolingProcessByMedium", "SAMMollier.CreateCoolingProcessByMedium",
              "Creates CoolingProcess",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_start");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            MollierPoint mollierPoint = null;
            if (!dataAccess.GetData(index, ref mollierPoint) || mollierPoint == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_flowTemperature");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double flowTemperature = double.NaN;
            if (!dataAccess.GetData(index, ref flowTemperature) || double.IsNaN(flowTemperature))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_returnTemperature");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double returnTemperature = double.NaN;
            if (!dataAccess.GetData(index, ref returnTemperature) || double.IsNaN(returnTemperature))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_efficency");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double efficiency = double.NaN;
            if (!dataAccess.GetData(index, ref efficiency) || double.IsNaN(efficiency))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            CoolingProcess coolingProcess = Core.Mollier.Create.CoolingProcess_ByMedium(mollierPoint,flowTemperature, returnTemperature, efficiency);


            index = Params.IndexOfOutputParam("coolingProcess");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierProcess(coolingProcess));
            }
        }
    }
}