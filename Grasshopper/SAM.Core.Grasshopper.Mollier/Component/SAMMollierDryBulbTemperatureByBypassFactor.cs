using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierDryBulbTemperatureByBypassFactor : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("2e116be1-e316-48d4-bd8c-d42d65b73b5b");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_dryBulbTemperature1", NickName = "_dryBulbTemperature1", Description = "Dry Bulb Temperature [°C] t1", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_dryBulbTemperature2", NickName = "_dryBulbTemperature2", Description = "Dry Bulb Temperature [°C] t2", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_bypassFactor", NickName = "_bypassFactor", Description = "ByPass Factor [0-1]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                //global::Grasshopper.Kernel.Parameters.Param_Number param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_pressure_", NickName = "_pressure_", Description = "Atmospheric pressure [Pa]", Access = GH_ParamAccess.item, Optional = true };
                //param_Number.SetPersistentData(Standard.Pressure);
                //result.Add(new GH_SAMParam(param_Number, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "DryBulbTemperature", NickName = "DryBulbTemperature", Description = "Dry Bulb Temperature [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Calculate from ByPass factor Dry Bulb Temperature.
        /// </summary>
        public SAMMollierDryBulbTemperatureByBypassFactor()
          : base("SAMMollier.DryBulbTemperatureByBypassFactor", "SAMMollier.DryBulbTemperatureByBypassFactor",
              "Calculate from ByPass factor Dry Bulb Temperature.\nIt is used to adjust process by ByPass Factor. \nUse as input for porcess by DryBulbTemperature",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_dryBulbTemperature1");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double dryBulbTemperature1 = double.NaN;
            if (!dataAccess.GetData(index, ref dryBulbTemperature1) || double.IsNaN(dryBulbTemperature1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_dryBulbTemperature2");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double dryBulbTemperature2 = double.NaN;
            if (!dataAccess.GetData(index, ref dryBulbTemperature2) || double.IsNaN(dryBulbTemperature2))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }


            index = Params.IndexOfInputParam("_bypassFactor");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double bypassFactor = double.NaN;
            if (!dataAccess.GetData(index, ref bypassFactor) || double.IsNaN(bypassFactor))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            //double dryBulbTemperature = dryBulbTemperature1 + ((dryBulbTemperature2 - dryBulbTemperature1) / (bypassFactor2 - bypassFactor1)) * (bypassFactor - 0);
            double dryBulbTemperature = dryBulbTemperature1 + ((dryBulbTemperature2 - dryBulbTemperature1) / (1 - 0)) * (bypassFactor - 0);


            index = Params.IndexOfOutputParam("DryBulbTemperature");
            if (index != -1)
            {
                dataAccess.SetData(index, dryBulbTemperature);
            }
        }
    }
}