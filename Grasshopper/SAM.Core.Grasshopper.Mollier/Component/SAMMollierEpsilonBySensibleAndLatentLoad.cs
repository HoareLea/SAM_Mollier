using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierEpsilonBySensibleAndLatentLoad : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("7be01fa4-a657-4461-a083-fb7f9a0b1e7f");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_sensibleLoad", NickName = "_sensibleLoad", Description = "Sensible Load [kW]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_latentLoad", NickName = "_latentLoad", Description = "Latent Heat [kW]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));;

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "epsilon", NickName = "ε", Description = "Slope coefficient Epsilon ε [kJ/kg]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "sensibleHeatRatio", NickName = "sensibleHeatRatio", Description = "Sensible Heat Ratio [-]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Calculate Diagram Temperature from Dry Bulb Temperature and Humidity ration for given atmospheric pressure.
        /// </summary>
        public SAMMollierEpsilonBySensibleAndLatentLoad()
          : base("SAMMollier.EpsilonBySensibleAndLatentLoad", "SAMMollier.EpsilonBySensibleAndLatentLoad",
              "Utility function to calculate slope coefficient Epsilon ε [kJ/kg] and Sensible Heat Ratio [-].",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_sensibleLoad");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double sensibleLoad = double.NaN;
            if (!dataAccess.GetData(index, ref sensibleLoad) || double.IsNaN(sensibleLoad))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_latentLoad");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double latentLoad = double.NaN;
            if (!dataAccess.GetData(index, ref latentLoad) || double.IsNaN(latentLoad))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double epsilon = Core.Mollier.Query.Epsilon_BySensibleAndLatentGain(sensibleLoad, latentLoad);

            index = Params.IndexOfOutputParam("epsilon");
            if (index != -1)
            {
                dataAccess.SetData(index, epsilon);
            }

            double sensibleHeatRatio = Core.Mollier.Query.SensibleHeatRatio(sensibleLoad, latentLoad);
            index = Params.IndexOfOutputParam("sensibleHeatRatio");
            if (index != -1)
            {
                dataAccess.SetData(index, sensibleHeatRatio);
            }
        }
    }
}