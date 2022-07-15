using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Grasshopper;
using SAM.Analytical.Mollier;
using Grasshopper.Kernel.Types;
using System.Linq;

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
        public override string LatestComponentVersion => "1.0.9";

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_GenericObject() { Name = "_airHandlingUnit", NickName = "_airHandlingUnit", Description = "SAM Analytical AirHandlingUnit or name", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterSupplyTemperature_", NickName = "winterSupplyTemperature_", Description = "Winter Supply Temperture [C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerSupplyTemperature_", NickName = "summerSupplyTemperature_", Description = "Summer Supply Temperture [C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "frostCoilOffTemperature_", NickName = "frostCoilOffTemperature_", Description = "Frost Coil Off Temperture [C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterHeatRecoverySensibleEfficiency_", NickName = "winterHeatRecoverySensibleEfficiency_", Description = "Winter Heat Recovery Sensible Efficiency [%]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterHeatRecoveryLatentEfficiency_", NickName = "winterHeatRecoveryLatentEfficiency_", Description = "Winter Heat Recovery Latent Efficiency [%]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterHeatRecoveryDryBulbTemperature_", NickName = "winterHeatRecoveryDryBulbTemperature_", Description = "Winter Heat Recovery Dry BulbTemperature [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterHeatRecoveryRelativeHumidity_", NickName = "winterHeatRecoveryRelativeHumidity_", Description = "Winter Heat Recovery Relative Humidity [%]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerHeatRecoverySensibleEfficiency_", NickName = "summerHeatRecoverySensibleEfficiency_", Description = "Summer Heat Recovery Sensible Efficiency [%]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerHeatRecoveryLatentEfficiency_", NickName = "summerHeatRecoveryLatentEfficiency_", Description = "Summer Heat Recovery Latent Efficiency [%]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerHeatRecoveryDryBulbTemperature_", NickName = "summerHeatRecoveryDryBulbTemperature_", Description = "Summer Heat Recovery Dry BulbTemperature [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerHeatRecoveryRelativeHumidity_", NickName = "summerHeatRecoveryRelativeHumidity_", Description = "Summer Heat Recovery Relative Humidity [%]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "coolingCoilFluidFlowTemperature_", NickName = "coolingCoilFluidFlowTemperature_", Description = "Cooling Coil Fluid Flow Temperature [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "coolingCoilFluidReturnTemperature_", NickName = "coolingCoilFluidReturnTemperature_", Description = "Cooling Coil Fluid Return Temperature [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "heatingCoilFluidFlowTemperature_", NickName = "heatingCoilFluidFlowTemperature_", Description = "Heating Coil Fluid Flow Temperature [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "heatingCoilFluidReturnTemperature_", NickName = "heatingCoilFluidReturnTemperature_", Description = "Heating Coil Fluid Return Temperature [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterHeatingCoilSupplyTemperature_", NickName = "winterHeatingCoilSupplyTemperature_", Description = "Winter Heating Coil Supply Temperature [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerHeatingCoil_", NickName = "summerHeatingCoil_", Description = "Summer Heating Coil (Yes or No)", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooAnalyticalModelParam() { Name = "analyticalModel", NickName = "analyticalModel", Description = "SAM Analytical Model", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooAirHandlingUnitParam() { Name = "airHandlingUnit", NickName = "airHandlingUnit", Description = "SAM Analytical AirHandlingUnit", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "summerSupplyTemperature", NickName = "summerSupplyTemperature", Description = "Summer Supply Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "winterSupplyTemperature", NickName = "winterSupplyTemperature", Description = "Winter Supply Temperature [C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
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

            index = Params.IndexOfInputParam("_airHandlingUnit");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            GH_ObjectWrapper objectWrapper = null;
            if (!dataAccess.GetData(index, ref objectWrapper) || objectWrapper == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            object @object = objectWrapper.Value;
            if (@object is IGH_Goo)
            {
                @object = (@object as dynamic).Value;
            }

            AdjacencyCluster adjacencyCluster = analyticalModel?.AdjacencyCluster;

            AirHandlingUnit airHandlingUnit = null;
            if (@object is string)
            {
                string name = (string)@object;
                airHandlingUnit = adjacencyCluster?.GetObjects((AirHandlingUnit x) => x.Name == name)?.FirstOrDefault();
                if(airHandlingUnit == null)
                {
                    airHandlingUnit = Create.AirHandlingUnit(name);
                }
            }
            else if(@object is AirHandlingUnit)
            {
                airHandlingUnit = adjacencyCluster?.GetObject<AirHandlingUnit>(((AirHandlingUnit)@object).Guid);
                if(airHandlingUnit == null)
                {
                    airHandlingUnit = (AirHandlingUnit)@object;
                }
            }

            if (airHandlingUnit == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not find or create Air Handling Unit");
                return;
            }

            double value = double.NaN;

            double summerSupplyTemperature = double.NaN;
            index = Params.IndexOfInputParam("summerSupplyTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                summerSupplyTemperature = value;
            }

            double winterSupplyTemperature = double.NaN;
            index = Params.IndexOfInputParam("winterSupplyTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                winterSupplyTemperature = value;
            }

            double frostCoilOffTemperature = double.NaN;
            index = Params.IndexOfInputParam("frostCoilOffTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                frostCoilOffTemperature = value;
            }

            double winterHeatRecoverySensibleEfficiency = double.NaN;
            index = Params.IndexOfInputParam("winterHeatRecoverySensibleEfficiency_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                winterHeatRecoverySensibleEfficiency = value;
            }

            double winterHeatRecoveryLatentEfficiency = double.NaN;
            index = Params.IndexOfInputParam("winterHeatRecoveryLatentEfficiency_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                winterHeatRecoveryLatentEfficiency = value;
            }

            double winterHeatRecoveryDryBulbTemperature = double.NaN;
            index = Params.IndexOfInputParam("winterHeatRecoveryDryBulbTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                winterHeatRecoveryDryBulbTemperature = value;
            }

            double winterHeatRecoveryRelativeHumidity = double.NaN;
            index = Params.IndexOfInputParam("winterHeatRecoveryRelativeHumidity_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                winterHeatRecoveryRelativeHumidity = value;
            }

            double summerHeatRecoverySensibleEfficiency = double.NaN;
            index = Params.IndexOfInputParam("summerHeatRecoverySensibleEfficiency_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                summerHeatRecoverySensibleEfficiency = value;
            }

            double summerHeatRecoveryLatentEfficiency = double.NaN;
            index = Params.IndexOfInputParam("summerHeatRecoveryLatentEfficiency_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                summerHeatRecoveryLatentEfficiency = value;
            }

            double summerHeatRecoveryDryBulbTemperature = double.NaN;
            index = Params.IndexOfInputParam("summerHeatRecoveryDryBulbTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                summerHeatRecoveryDryBulbTemperature = value;
            }

            double summerHeatRecoveryRelativeHumidity = double.NaN;
            index = Params.IndexOfInputParam("summerHeatRecoveryRelativeHumidity_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                summerHeatRecoveryRelativeHumidity = value;
            }

            double coolingCoilFluidFluidTemperature = double.NaN;
            index = Params.IndexOfInputParam("coolingCoilFluidFlowTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                coolingCoilFluidFluidTemperature = value;
            }

            double coolingCoilFluidReturnTemperature = double.NaN;
            index = Params.IndexOfInputParam("coolingCoilFluidReturnTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                coolingCoilFluidReturnTemperature = value;
            }

            double heatingCoilFluidFlowTemperature = double.NaN;
            index = Params.IndexOfInputParam("heatingCoilFluidFlowTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                heatingCoilFluidFlowTemperature = value;
            }

            double heatingCoilFluidReturnTemperature = double.NaN;
            index = Params.IndexOfInputParam("heatingCoilFluidReturnTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                heatingCoilFluidReturnTemperature = value;
            }

            double winterHeatingCoilSupplyTemperature = double.NaN;
            index = Params.IndexOfInputParam("winterHeatingCoilSupplyTemperature_");
            if (index != -1 && dataAccess.GetData(index, ref value) && !double.IsNaN(value))
            {
                winterHeatingCoilSupplyTemperature = value;
            }

            bool @bool = false;

            bool? summerHeatingCoil = null;
            index = Params.IndexOfInputParam("summerHeatingCoil_");
            if (index != -1 && dataAccess.GetData(index, ref @bool))
            {
                summerHeatingCoil = @bool;
            }

            if (!double.IsNaN(summerSupplyTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.SummerSupplyTemperature, summerSupplyTemperature);
            }

            if (!double.IsNaN(winterSupplyTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.WinterSupplyTemperature, winterSupplyTemperature);
            }

            if (!double.IsNaN(frostCoilOffTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.FrostCoilOffTemperature, frostCoilOffTemperature);
            }

            if (!double.IsNaN(winterHeatRecoverySensibleEfficiency))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.WinterHeatRecoverySensibleEfficiency, winterHeatRecoverySensibleEfficiency);
            }

            if (!double.IsNaN(winterHeatRecoveryLatentEfficiency))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.WinterHeatRecoveryLatentEfficiency, winterHeatRecoveryLatentEfficiency);
            }

            if (!double.IsNaN(winterHeatRecoverySensibleEfficiency))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.WinterHeatRecoverySensibleEfficiency, winterHeatRecoverySensibleEfficiency);
            }

            if (!double.IsNaN(winterHeatRecoveryLatentEfficiency))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.WinterHeatRecoveryLatentEfficiency, winterHeatRecoveryLatentEfficiency);
            }

            if (!double.IsNaN(summerHeatRecoverySensibleEfficiency))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.SummerHeatRecoverySensibleEfficiency, summerHeatRecoverySensibleEfficiency);
            }

            if (!double.IsNaN(summerHeatRecoveryLatentEfficiency))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.SummerHeatRecoveryLatentEfficiency, summerHeatRecoveryLatentEfficiency);
            }

            if (!double.IsNaN(coolingCoilFluidFluidTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.CoolingCoilFluidFlowTemperature, coolingCoilFluidFluidTemperature);
            }

            if (!double.IsNaN(coolingCoilFluidReturnTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.CoolingCoilFluidReturnTemperature, coolingCoilFluidReturnTemperature);
            }

            if (!double.IsNaN(heatingCoilFluidFlowTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.HeatingCoilFluidFlowTemperature, heatingCoilFluidFlowTemperature);
            }

            if (!double.IsNaN(heatingCoilFluidReturnTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.HeatingCoilFluidReturnTemperature, heatingCoilFluidReturnTemperature);
            }

            if (!double.IsNaN(winterHeatingCoilSupplyTemperature))
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.WinterHeatingCoilSupplyTemperature, winterHeatingCoilSupplyTemperature);
            }

            if (summerHeatingCoil != null && summerHeatingCoil.HasValue)
            {
                airHandlingUnit.SetValue(AirHandlingUnitParameter.SummerHeatingCoil, summerHeatingCoil);
            }

            adjacencyCluster.AddObject(airHandlingUnit);
            analyticalModel = new AnalyticalModel(analyticalModel, adjacencyCluster);

            AirHandlingUnitResult airHandlingUnitResult = Analytical.Mollier.Create.AirHandlingUnitResult(analyticalModel, airHandlingUnit.Name);
            if (airHandlingUnit != null && airHandlingUnitResult != null)
            {
                adjacencyCluster = analyticalModel.AdjacencyCluster;
                adjacencyCluster.AddObject(airHandlingUnitResult);
                adjacencyCluster.AddRelation(airHandlingUnit, airHandlingUnitResult);
                analyticalModel = new AnalyticalModel(analyticalModel, adjacencyCluster);
            }

            index = Params.IndexOfOutputParam("analyticalModel");
            if (index != -1)
            {
                dataAccess.SetData(index, analyticalModel);
            }

            index = Params.IndexOfOutputParam("airHandlingUnit");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooAirHandlingUnit(airHandlingUnit));
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
                    winterSpaceTemperature = double.NaN;
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
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceRelativeHumidty, out double winterSpaceHumidty))
                {
                    winterSpaceHumidty = double.NaN;
                }

                dataAccess.SetData(index, winterSpaceHumidty);
            }

            index = Params.IndexOfOutputParam("summerSpaceHumidity");
            if (index != -1)
            {
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceRelativeHumidty, out double summerSpaceHumidty))
                {
                    summerSpaceHumidty = double.NaN;
                }

                dataAccess.SetData(index, summerSpaceHumidty);
            }

            index = Params.IndexOfOutputParam("summerSupplyTemperature");
            if (index != -1)
            {
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSupplyTemperature, out summerSupplyTemperature))
                {
                    summerSupplyTemperature = double.NaN;
                }

                dataAccess.SetData(index, summerSupplyTemperature);
            }

            index = Params.IndexOfOutputParam("winterSupplyTemperature");
            if (index != -1)
            {
                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSupplyTemperature, out winterSupplyTemperature))
                {
                    winterSupplyTemperature = double.NaN;
                }

                dataAccess.SetData(index, winterSupplyTemperature);
            }
        }
    }
}