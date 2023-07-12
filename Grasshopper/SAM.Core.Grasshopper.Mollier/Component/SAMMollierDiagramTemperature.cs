using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierDiagramTemperature : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("a027225d-557c-4e8a-92f3-deaf262bc06c");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_dryBulbTemperature", NickName = "_dryBulbTemperature", Description = "Dry Bulb Temperature [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatio", NickName = "_humidityRatio", Description = "Humidity Ratio [kg_waterVapor/kg_dryAir]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));


                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_pressure_", NickName = "_pressure_", Description = "Atmospheric pressure [Pa]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(Standard.Pressure);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "diagramTemperature", NickName = "diagramTemperature", Description = "Diagram Temperature [°C] \nIt is used only for Mollier Diagam", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Calculate Diagram Temperature from Dry Bulb Temperature and Humidity ration for given atmospheric pressure.
        /// </summary>
        public SAMMollierDiagramTemperature()
          : base("SAMMollier.DiagramTemperature", "SAMMollier.DiagramTemperature",
              "Utility function to calculate Diagram Temperature from Dry Bulb Temperature and Humidity ration for given atmospheric pressure.\nIt is used to draw Mollier in correct location in Rhino",
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

            double pressure = double.NaN;
            index = Params.IndexOfInputParam("_pressure_");
            if (index == -1 || !dataAccess.GetData(index, ref pressure) || double.IsNaN(pressure))
            {
                pressure = Standard.Pressure;
            }

            double diagramTemperature = Core.Mollier.Query.DiagramTemperature(dryBulbTemperature, humidityRatio, pressure);


            index = Params.IndexOfOutputParam("diagramTemperature");
            if (index != -1)
            {
                dataAccess.SetData(index, diagramTemperature);
            }
        }
    }
}