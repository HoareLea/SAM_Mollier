using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierCreateSteamHumidificationProcessByHumidityRatioDifference : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("e5ed1f5b-f303-4d3b-b775-331091f07933");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatioDifference", NickName = "_humidityRatioDifference", Description = "Humidity Ratio Difference", Access = GH_ParamAccess.item}, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "steamHumidificationProcess", NickName = "steamHumidificationProcess", Description = "SteamHumidificationProcess", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateSteamHumidificationProcessByHumidityRatioDifference()
          : base("SAMMollier.CreateSteamHumidificationProcessByHumidityRatioDifference", "SAMMollier.CreateSteamHumidificationProcessByHumidityRatioDifference",
              "Creates SteamHumidificationProcess",
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

            index = Params.IndexOfInputParam("_humidityRatioDifference");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double humidityRatioDifference = double.NaN;
            if (!dataAccess.GetData(index, ref humidityRatioDifference) || double.IsNaN(humidityRatioDifference))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            SteamHumidificationProcess steamHumidificationProcess = Core.Mollier.Create.SteamHumidificationProcess_ByHumidityRatioDifference(mollierPoint, humidityRatioDifference);

            index = Params.IndexOfOutputParam("steamHumidificationProcess");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierProcess(steamHumidificationProcess));
            }
        }
    }
}