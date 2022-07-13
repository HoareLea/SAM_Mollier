using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Grasshopper;
using SAM.Core.Grasshopper.Mollier;

namespace SAM.Analytical.Grasshopper
{
    public class SAMMollierCreateMollierPoint : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("32196faa-2db4-4f6b-9c4f-cc470f9a2464");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_dryBulbTemperature", NickName = "_dryBulbTemperature", Description = "Dry Bulb Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatio", NickName = "_humidityRatio", Description = "Humidity Ratio [kg/kg]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "pressure_", NickName = "_pressure", Description = "Atmospheric Pressure", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "mollierPoint", NickName = "mollierPoint", Description = "SAM Core Mollier MollierPoint", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateMollierPoint()
          : base("SAMMollier.CreateMollierPoint", "SAMMollier.CreateMollierPoint",
              "Creates Mollier Point",
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

            index = Params.IndexOfInputParam("_humidityRatio");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double humidityRatio = double.NaN;
            if (!dataAccess.GetData(index, ref humidityRatio) || double.IsNaN(humidityRatio))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double pressure = Core.Mollier.Standard.Pressure;
            index = Params.IndexOfInputParam("pressure_");
            if(index != -1)
            {
                double pressure_Temp = double.NaN;
                if (dataAccess.GetData(index, ref pressure_Temp) && !double.IsNaN(pressure_Temp))
                {
                    pressure = pressure_Temp;
                }
            }


            index = Params.IndexOfOutputParam("mollierPoint");
            if (index != -1)
            {
                Core.Mollier.MollierPoint mollierPoint = new Core.Mollier.MollierPoint(dryBulbTemperature, humidityRatio, pressure);

                dataAccess.SetData(index, new GooMollierPoint(mollierPoint));
            }
        }
    }
}