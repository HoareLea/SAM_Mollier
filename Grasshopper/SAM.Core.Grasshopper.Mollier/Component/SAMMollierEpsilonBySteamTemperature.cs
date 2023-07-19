using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierEpsilonBySteamTemperature : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("f0352620-123c-4546-a1f9-fddc804a2de2");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_steamTemperature", NickName = "_steamTemperature", Description = "Steam Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "epsilon", NickName = "epsilon", Description = "Epsilon [kJ/kg] \n*vapourizationLatentHeat + specificHeat_WaterVapour * steamTemperature", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "epsilon_h", NickName = "epsilon_h", Description = "Epsilon [kJ/kg] calculated from saturated steam enthalpy h''from temperature ", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierEpsilonBySteamTemperature()
          : base("SAMMollier.EpsilonBySteamTemperature", "SAMMollier.EpsilonBySteamTemperature",
              "Epsilon By Steam Temperature",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_steamTemperature");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double steamTemperature = double.NaN;
            if (!dataAccess.GetData(index, ref steamTemperature) || double.IsNaN(steamTemperature))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double epsilon = Core.Mollier.Query.Epsilon(steamTemperature);
            double epsilon_h = Core.Mollier.Query.Epsilon_BySteamTemperatureUsingEnthalpy(steamTemperature);


            index = Params.IndexOfOutputParam("epsilon");
            if (index != -1)
            {
                dataAccess.SetData(index, epsilon);
            }
            index = Params.IndexOfOutputParam("epsilon_h");
            if (index != -1)
            {
                dataAccess.SetData(index, Math.Round(epsilon_h));
            }
        }
    }
}