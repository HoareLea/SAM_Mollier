using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierSpecificHeat : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("0de90958-6646-4345-a9da-86f00852724e");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Mollier;

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_dryBulbTemperature", NickName = "_dryBulbTemperature", Description = "Dry Bulb Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "cp_Air", NickName = "cp_Air", Description = "Specific Heat od Air [kJ/kg*K]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "cp_WaterVapour", NickName = "cp_WaterVapour", Description = "Specific Heat od Water Vapour [kJ/kg*K]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "cp_Water", NickName = "cp_Water", Description = "Specific Heat od Water [kJ/kg*K]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierSpecificHeat()
          : base("SAMMollier.SpecificHeat", "SAMMollier.SpecificHeat",
              "Specific Heat [kJ/kg*K]",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_dryBulbTemperature");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double dryBulbTemperature = double.NaN;
            if (!dataAccess.GetData(index, ref dryBulbTemperature) || double.IsNaN(dryBulbTemperature))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double specificHeat_Air = Core.Mollier.Query.SpecificHeat_Air(dryBulbTemperature);
            double specificHeat_WaterVapour = Core.Mollier.Query.SpecificHeat_WaterVapour(dryBulbTemperature);
            double specificHeat_Water = Core.Mollier.Query.SpecificHeat_Water(dryBulbTemperature);

            index = Params.IndexOfOutputParam("cp_Air");
            if (index != -1)
            {
                dataAccess.SetData(index, specificHeat_Air);
            }

            index = Params.IndexOfOutputParam("cp_WaterVapour");
            if (index != -1)
            {
                dataAccess.SetData(index, specificHeat_WaterVapour);
            }

            index = Params.IndexOfOutputParam("cp_Water");
            if (index != -1)
            {
                dataAccess.SetData(index, specificHeat_Water);
            }
        }
    }
}