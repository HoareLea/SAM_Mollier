using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierEpsilon : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("0d75a767-db20-4ab5-9d89-cf4a809667a4");

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_totalLoad", NickName = "_totalLoad", Description = "Total Heat Gains Qtotal [kW]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_moistureGainsMassFlow", NickName = "_moistureGainsMassFlow Ẇ", Description = "Moisture Gains Mass Flow Ẇ[kg/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));


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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "epsilon", NickName = "ε", Description = "Slope coefficient Epsilon ε [kJ/kg]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Calculate Diagram Temperature from Dry Bulb Temperature and Humidity ration for given atmospheric pressure.
        /// </summary>
        public SAMMollierEpsilon()
          : base("SAMMollier.Epsilon", "SAMMollier.Epsilon",
              "Utility function to calculate slope coefficient Epsilon ε [kJ/kg].",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_totalLoad");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double totalLoad = double.NaN;
            if (!dataAccess.GetData(index, ref totalLoad) || double.IsNaN(totalLoad))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_moistureGainsMassFlow");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double moistureGainsMassFlow = double.NaN;
            if (!dataAccess.GetData(index, ref moistureGainsMassFlow) || double.IsNaN(moistureGainsMassFlow))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            //double pressure = double.NaN;
            //index = Params.IndexOfInputParam("_pressure_");
            //if (index == -1 || !dataAccess.GetData(index, ref pressure) || double.IsNaN(pressure))
            //{
            //    pressure = Standard.Pressure;
            //}

            double epsilon = Core.Mollier.Query.Epsilon(totalLoad, moistureGainsMassFlow);

            index = Params.IndexOfOutputParam("epsilon");
            if (index != -1)
            {
                dataAccess.SetData(index, epsilon);
            }
        }
    }
}