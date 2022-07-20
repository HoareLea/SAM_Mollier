using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;
using SAM.Core.Grasshopper.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierCreateHeatRecoveryProcess : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("e2fe1d14-6c75-4ae8-bd76-b18a3fa27363");

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
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_supply", NickName = "_supply", Description = "MollierPoint for supply air parameters", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_return", NickName = "_return", Description = "MollierPoint for return air parameters", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = null;
                
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_sensibleHeatRecoveryEfficiency", NickName = "_sensibleHeatRecoveryEfficiency", Description = "Sensible Heat Recovery Efficiency [%]", Access = GH_ParamAccess.item, Optional = true};
                param_Number.SetPersistentData(75);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Voluntary));

                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_latentHeatRecoveryEfficiency", NickName = "_latentHeatRecoveryEfficiency", Description = "Latent Heat Recovery Efficiency [%]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(0);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "heatRecoveryProcess", NickName = "heatRecoveryProcess", Description = "Heat Recovery Process", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateHeatRecoveryProcess()
          : base("SAMMollier.CreateHeatRecoveryProcess", "SAMMollier.CreateHeatRecoveryProcess",
              "Creates HeatRecoveryProcess",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_supply");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            MollierPoint supply = null;
            if (!dataAccess.GetData(index, ref supply) || supply == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_return");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            MollierPoint @return = null;
            if (!dataAccess.GetData(index, ref @return) || supply == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_sensibleHeatRecoveryEfficiency");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double sensibleHeatRecoveryEfficiency = double.NaN;
            if (!dataAccess.GetData(index, ref sensibleHeatRecoveryEfficiency) || double.IsNaN(sensibleHeatRecoveryEfficiency))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_latentHeatRecoveryEfficiency");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double latentHeatRecoveryEfficiency = double.NaN;
            if (!dataAccess.GetData(index, ref latentHeatRecoveryEfficiency) || double.IsNaN(latentHeatRecoveryEfficiency))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            HeatRecoveryProcess heatRecoveryProcess = Core.Mollier.Create.HeatRecoveryProcess(supply, @return, sensibleHeatRecoveryEfficiency, latentHeatRecoveryEfficiency);


            index = Params.IndexOfOutputParam("heatRecoveryProcess");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierProcess(heatRecoveryProcess));
            }
        }
    }
}