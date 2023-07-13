using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierMoistureGainsMassFlow : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("edd284f2-fd81-43e9-8291-cc2e5a1c5dba");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_latentLoad", NickName = "_totalLoad", Description = "Total Heat Gains Qtotal [kW]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                //result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_vapourizationLatentHeat", NickName = "_r0", Description = "Latent Heat of Vapourization r0 [kJ/kg]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = null;
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_vapourizationLatentHeat", NickName = "_r0", Description = "Latent Heat of Vapourization r0 [kJ/kg]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(Zero.VapourizationLatentHeat/1000);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "MoistureGainsMassFlow", NickName = "Ẇ", Description = "Moisture Gains Mass Flow Ẇ [kg/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Calculate Diagram Temperature from Dry Bulb Temperature and Humidity ration for given atmospheric pressure.
        /// </summary>
        public SAMMollierMoistureGainsMassFlow()
          : base("SAMMollier.MoistureGainsMassFlow", "SAMMollier.Ẇ",
              "Calculates Moisture Gains Mass Flow Ẇ [kg/s] \nDefault r0=2501 [kJ/kg] is used.",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

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

            index = Params.IndexOfInputParam("_vapourizationLatentHeat");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double vapourizationLatentHeat = double.NaN;
            if (!dataAccess.GetData(index, ref vapourizationLatentHeat) || double.IsNaN(vapourizationLatentHeat))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double moistureGainsMassFlow = Core.Mollier.Query.MoistureGainsMassFlow(latentLoad, vapourizationLatentHeat);

            index = Params.IndexOfOutputParam("MoistureGainsMassFlow");
            if (index != -1)
            {
                dataAccess.SetData(index, moistureGainsMassFlow);
            }
        }
    }
}