using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierDryAirMassFlowByMollierPoint : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c90598ed-3f3b-4d26-bb1e-93258a3bad95");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "massFlow_", NickName = "massFlow_", Description = "Mass Flow [kg/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "volumetricFlowRate_", NickName = "volumetricFlowRate_", Description = "Volumetric Flow Rate [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_mollierPoint", NickName = "_mollierPoint", Description = "MollierPoint", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dryAirMassFlow", NickName = "dryAirMassFlow", Description = "dryAirMassFlow [kg/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierDryAirMassFlowByMollierPoint()
          : base("SAMMollier.DryAirMassFlowByMollierPoint", "SAMMollier.DryAirMassFlowByMollierPoint",
              "Dry Air Mass Flow [kg/s]",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            double massFlow = double.NaN;

            index = Params.IndexOfInputParam("_massFlow");
            double massFlow_Temp = double.NaN;
            if(index != -1 && dataAccess.GetData(index, ref massFlow_Temp))
            {
                massFlow = massFlow_Temp;
            }

            double volumetricFlowRate = double.NaN;
            index = Params.IndexOfInputParam("volumetricFlowRate_");
            double volumetricFlowRate_Temp = double.NaN;
            if (index != -1 && dataAccess.GetData(index, ref volumetricFlowRate_Temp))
            {
                volumetricFlowRate = volumetricFlowRate_Temp;
            }

            if((double.IsNaN(massFlow) && double.IsNaN(volumetricFlowRate)) || (!double.IsNaN(massFlow) && !double.IsNaN(volumetricFlowRate)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Provide mass flow or volumetric flow rate.");
                return;
            }

            index = Params.IndexOfInputParam("_mollierPoint");
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

            if(!double.IsNaN(volumetricFlowRate))
            {
                massFlow = volumetricFlowRate * mollierPoint.Density();
            }

            double massFlow_DryAir = Core.Mollier.Query.MassFlow_DryAir(mollierPoint, massFlow);

            index = Params.IndexOfOutputParam("dryAirMassFlow");
            if (index != -1)
            {
                dataAccess.SetData(index, massFlow_DryAir);
            }
        }
    }
}