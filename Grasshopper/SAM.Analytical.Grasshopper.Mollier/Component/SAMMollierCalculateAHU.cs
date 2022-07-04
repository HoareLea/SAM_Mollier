using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Grasshopper;
using SAM.Analytical.Mollier;

namespace SAM.Analytical.Grasshopper
{
    public class SAMMollierCalculateAHU : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("6af6f5ad-544c-414c-a543-4bad529f49f1");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.4";

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
                result.Add(new GH_SAMParam(new GooAnalyticalModelParam() { Name = "_analyticalModel", NickName = "_analytcailModel", Description = "SAM AnalyticalModel", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "_name", NickName = "_name", Description = "AHU Name", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooAnalyticalModelParam() { Name = "analyticalModel", NickName = "analyticalModel", Description = "SAM Analytical Model", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "sensibleHeatLoss", NickName = "sensibleHeatLoss", Description = "Sensible Heat Loss for connected Spaces [kW]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "sensibleHeatGain", NickName = "sensibleHeatGain", Description = "Sensible Heat Gain for connected Spaces [kW]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerDesignTemperature", NickName = "summerDesignTemperature", Description = "Summer Design Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerDesignRelativeHumidity", NickName = "summerDesignRelativeHumidity", Description = "Summer Design Relative Humidity [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "summerDesignDayName", NickName = "summerDesignDayName", Description = "Summer Design Day Name", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Integer() { Name = "summerDesignDayIndex", NickName = "summerDesignDayIndex", Description = "Summer Design Day Hour Index [0-23]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterDesignTemperature", NickName = "winterDesignTemperature", Description = "Winter Design Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterDesignRelativeHumidity", NickName = "winterDesignRelativeHumidity", Description = "Winter Design Relative Humidity [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "winterDesignDayName", NickName = "winterDesignDayName", Description = "Winter Design Day Name", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Integer() { Name = "winterDesignDayIndex", NickName = "winterDesignDayIndex", Description = "Winter Design Day Hour Index [0-23]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "supplyAirFlow", NickName = "supplyAirFlow", Description = "Supply Air Flow [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "outsideSupplyAirFlow", NickName = "outsideSupplyAirFlow", Description = "Outside Supply Air Flow [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "exhaustAirFlow", NickName = "exhaustAirFlow", Description = "Exhaust Air Flow [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "recirculationAirFlow", NickName = "recirculationAirFlow", Description = "Recirculation Air Flow [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterSpaceTemperature", NickName = "winterSpaceTemperature", Description = "Winter Space Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterSpaceHumidity", NickName = "winterSpaceHumidty", Description = "Winter Space Humidity [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerSpaceTemperature", NickName = "summerSpaceTemperature", Description = "Summer Space Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerSpaceHumidity", NickName = "summerSpaceHumidty", Description = "Summer Space Humidity [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCalculateAHU()
          : base("SAMMollier.CalculateAHU", "SAMMollier.CalculateAHU",
              "Calculate Air Handlin Unit",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_analyticalModel");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            AnalyticalModel analyticalModel = null;
            if (!dataAccess.GetData(index, ref analyticalModel) || analyticalModel == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_name");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            string name = null;
            if (!dataAccess.GetData(index, ref name) || string.IsNullOrWhiteSpace(name))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            Analytical.Mollier.AirHandlingUnitResult airHandlingUnitResult = null;

            airHandlingUnitResult = Analytical.Mollier.Create.AirHandlingUnitResult(analyticalModel, name, out AirHandlingUnit airHandlingUnit);

            index = Params.IndexOfOutputParam("analyticalModel");
            if (index != -1)
            {
                dataAccess.SetData(index, analyticalModel);
            }

            if (airHandlingUnitResult != null)
            {
                index = Params.IndexOfOutputParam("sensibleHeatLoss");
                if (index != -1)
                {
                    dataAccess.SetData(index, airHandlingUnitResult.GetValue<double>(AirHandlingUnitResultParameter.SensibleHeatLoss) / 1000);
                }

                index = Params.IndexOfOutputParam("sensibleHeatGain");
                if (index != -1)
                {
                    dataAccess.SetData(index, airHandlingUnitResult.GetValue<double>(AirHandlingUnitResultParameter.SensibleHeatGain) / 1000);
                }

                index = Params.IndexOfOutputParam("summerDesignTemperature");
                if (index != -1)
                {
                    dataAccess.SetData(index, airHandlingUnitResult.GetValue<double>(AirHandlingUnitResultParameter.SummerDesignTemperature));
                }

                index = Params.IndexOfOutputParam("summerDesignRelativeHumidity");
                if (index != -1)
                {
                    dataAccess.SetData(index, airHandlingUnitResult.GetValue<double>(AirHandlingUnitResultParameter.SummerDesignRelativeHumidity));
                }

                index = Params.IndexOfOutputParam("summerDesignDayName");
                if (index != -1)
                {
                    if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignDayName, out string summerDesignDayName))
                    {
                        summerDesignDayName = null;
                    }

                    dataAccess.SetData(index, summerDesignDayName);
                }

                index = Params.IndexOfOutputParam("summerDesignDayIndex");
                if (index != -1)
                {
                    if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignDayIndex, out int summerDesignDayIndex))
                    {
                        summerDesignDayIndex = -1;
                    }

                    dataAccess.SetData(index, summerDesignDayIndex);
                }

                index = Params.IndexOfOutputParam("winterDesignTemperature");
                if (index != -1)
                {
                    dataAccess.SetData(index, airHandlingUnitResult.GetValue<double>(AirHandlingUnitResultParameter.WinterDesignTemperature));
                }

                index = Params.IndexOfOutputParam("winterDesignRelativeHumidity");
                if (index != -1)
                {
                    dataAccess.SetData(index, airHandlingUnitResult.GetValue<double>(AirHandlingUnitResultParameter.WinterDesignRelativeHumidity));
                }

                index = Params.IndexOfOutputParam("winterDesignDayName");
                if (index != -1)
                {
                    if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignDayName, out string winterDesignDayName))
                    {
                        winterDesignDayName = null;
                    }

                    dataAccess.SetData(index, winterDesignDayName);
                }

                index = Params.IndexOfOutputParam("winterDesignDayIndex");
                if (index != -1)
                {
                    if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignDayIndex, out int winterDesignDayIndex))
                    {
                        winterDesignDayIndex = -1;
                    }

                    dataAccess.SetData(index, winterDesignDayIndex);
                }

                double supplyAirFlow = 0;

                index = Params.IndexOfOutputParam("supplyAirFlow");
                if (index != -1)
                {
                    if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SupplyAirFlow, out supplyAirFlow))
                    {
                        supplyAirFlow = 0;
                    }

                    dataAccess.SetData(index, supplyAirFlow);
                }

                double outsideSupplyAirFlow = 0;

                index = Params.IndexOfOutputParam("outsideSupplyAirFlow");
                if (index != -1)
                {
                    if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, out outsideSupplyAirFlow))
                    {
                        outsideSupplyAirFlow = 0;
                    }

                    dataAccess.SetData(index, outsideSupplyAirFlow);
                }

                index = Params.IndexOfOutputParam("exhaustAirFlow");
                if (index != -1)
                {
                    if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.ExhaustAirFlow, out double exhaustAirFlow))
                    {
                        exhaustAirFlow = 0;
                    }

                    dataAccess.SetData(index, exhaustAirFlow);
                }

                index = Params.IndexOfOutputParam("recirculationAirFlow");
                if (index != -1)
                {
                    double recirculationAirFlow = Math.Max(0, supplyAirFlow - outsideSupplyAirFlow);
                    dataAccess.SetData(index, recirculationAirFlow);
                }
            }

            index = Params.IndexOfOutputParam("winterSpaceTemperature");
            if (index != -1)
            {
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceTemperature, out double winterSpaceTemperature))
                {
                    winterSpaceTemperature =double.NaN;
                }

                dataAccess.SetData(index, winterSpaceTemperature);
            }

            index = Params.IndexOfOutputParam("summerSpaceTemperature");
            if (index != -1)
            {
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceTemperature, out double summerSpaceTemperature))
                {
                    summerSpaceTemperature = double.NaN;
                }

                dataAccess.SetData(index, summerSpaceTemperature);
            }

            index = Params.IndexOfOutputParam("winterSpaceHumidity");
            if (index != -1)
            {
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceHumidty, out double winterSpaceHumidty))
                {
                    winterSpaceHumidty = double.NaN;
                }

                dataAccess.SetData(index, winterSpaceHumidty);
            }

            index = Params.IndexOfOutputParam("summerSpaceHumidity");
            if (index != -1)
            {
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceHumidty, out double summerSpaceHumidty))
                {
                    summerSpaceHumidty = double.NaN;
                }

                dataAccess.SetData(index, summerSpaceHumidty);
            }
        }
    }
}