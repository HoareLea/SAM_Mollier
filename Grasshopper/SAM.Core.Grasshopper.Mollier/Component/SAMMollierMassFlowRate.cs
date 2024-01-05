using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierMassFlowRate : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("b93ccff6-ff54-4e7e-ac7a-aab5b863b7a7");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_volumetricFlowRate", NickName = "_volumetricFlowRate", Description = "Volumetric flow rate [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                
                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = null;
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_density", NickName = "_density", Description = "Density [kg/m3]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(1.210);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
               
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "massFlowRate", NickName = "massFlowRate", Description = "Mass flow rate [kg/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Calculate Diagram Temperature from Dry Bulb Temperature and Humidity ration for given atmospheric pressure.
        /// </summary>
        public SAMMollierMassFlowRate()
          : base("SAMMollier.MassFlowRate", "SAMMollier.MassFlowRate",
              "Calculates mass flor rate from valumetric flow rate and density.",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_volumetricFlowRate");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double volumetricFlowRate = double.NaN;
            if (!dataAccess.GetData(index, ref volumetricFlowRate) || double.IsNaN(volumetricFlowRate))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_density");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double density = double.NaN;
            if (!dataAccess.GetData(index, ref density) || double.IsNaN(density))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double massFlow = Core.Mollier.Query.MassFlow(volumetricFlowRate, density);

            index = Params.IndexOfOutputParam("massFlowRate");
            if (index != -1)
            {
                dataAccess.SetData(index, massFlow);
            }
        }
    }
}