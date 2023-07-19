using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierCreateDiagram : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("95611e60-2b7d-42d5-85b1-853fb8872b16");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.8";

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

                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = null;
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_temperature_Min_", NickName = "_temperature_Min_", Description = "Minimal value of temperature axis - [°C]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(-20);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_temperature_Max_", NickName = "_temperature_Max_", Description = "Maximal value of temperature axis - [°C]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(50);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatio_Min_", NickName = "_humidityRatio_Min_", Description = "Minimal value of humidity ratio axis - [g/kg]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(0);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatio_Max_", NickName = "_humidityRatio_Max_", Description = "Maximal value of humidity ratio axis - [g/kg]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(35);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                global::Grasshopper.Kernel.Parameters.Param_Boolean param_Bool = null;
                param_Bool = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_chartType_", NickName = "_chartType_", Description = "Type of the chart: true - Mollier Chart, false - Psychrometric Chart", Access = GH_ParamAccess.item, Optional = true };
                param_Bool.SetPersistentData(true);
                result.Add(new GH_SAMParam(param_Bool, ParamVisibility.Binding));

                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_pressure_", NickName = "_pressure_", Description = "Pressure [Pa]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(Standard.Pressure);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {

                List<GH_SAMParam> result = new List<GH_SAMParam>();

                result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Relative Humidity Lines", NickName = "relativeHumidityLines", Description = "Contains relative humidity lines as curves", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Relative Humidity Values", NickName = "relativeHumidities", Description = "Values of relative humidity lines", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Relative Humidity Points", NickName = "relativeHumidityPoints", Description = "MollierPoints used to create relative humidity lines", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Dry Bulb Temperature Lines", NickName = "dryBulbTemperatureLines", Description = "Contains dry bulb temperature lines as curves", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Dry Bulb Temperature Values", NickName = "dryBulbTemperatures", Description = "Values of dry bulb temperature lines", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Dry Bulb Temperature Points", NickName = "dryBulbTemperaturePoints", Description = "MollierPoints used to create dry bulb temperature lines", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

                //result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Diagram Temperature Lines", NickName = "DiagramTemperatureLines", Description = "Contains diagram temperature lines as curves used ONLY to construct Mollier diagram", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                //result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Diagram Temperature Values", NickName = "diagramTemperatures", Description = "Values of diagram temperature lines used ONLY to construct Mollier diagram", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                //result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Diagram Temperature Points", NickName = "diagramTemperaturePoints", Description = "MollierPoints used to create diagram temperature lines used ONLY to construct Mollier diagram", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Density Lines", NickName = "densityLines", Description = "Contains density lines as curves", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Density Values", NickName = "densities", Description = "Values of density lines", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Density Points", NickName = "densityPoints", Description = "MollierPoints used to create density lines", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Enthalpy Lines", NickName = "enthalpyLines", Description = "Contains enthalpy lines as curves", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Enthalpy Values", NickName = "enthalpies", Description = "Values of enthalpy lines", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Enthalpy Points", NickName = "enthalpyPoints", Description = "MollierPoints used to create enthalpy lines", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Specific Volume Lines", NickName = "specificVolumeLines", Description = "Contains specific volume lines as curves", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Specific Volume Values", NickName = "specificVolumes", Description = "Values of specific volume lines", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Specific Volume Points", NickName = "specificVolumePoints", Description = "MollierPoints used to create specific volume lines", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Wet Bulb Temperature Lines", NickName = "wetBulbTemperatureLines", Description = "Contains wet bulb temperature lines as curves", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Wet Bulb Temperature Values", NickName = "wetBulbTemperatures", Description = "Values of wet bulb temperature lines", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Wet Bulb Temperature Points", NickName = "wetBulbTemperaturePoints", Description = "MollierPoints used to create wet bulb temperature lines", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_chartType_", NickName = "_chartType_", Description = "Type of the chart: true - Mollier Chart, false - Psychrometric Chart", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateDiagram()
          : base("SAMMollier.CreateDiagram", "SAMMollier.CreateDiagram",
              "Create Mollier or Psychrometric Diagram",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            //PROCESSING INPUT
            int index;

            double temperature_Min = double.NaN;
            index = Params.IndexOfInputParam("_temperature_Min_");
            if (index == -1 || !dataAccess.GetData(index, ref temperature_Min) || double.IsNaN(temperature_Min))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double temperature_Max = double.NaN;
            index = Params.IndexOfInputParam("_temperature_Max_");
            if (index == -1 || !dataAccess.GetData(index, ref temperature_Max) || double.IsNaN(temperature_Max))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double humidityRatio_Min = double.NaN;
            index = Params.IndexOfInputParam("_humidityRatio_Min_");
            if (index == -1 || !dataAccess.GetData(index, ref humidityRatio_Min) || double.IsNaN(humidityRatio_Min))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double humidityRatio_Max = double.NaN;
            index = Params.IndexOfInputParam("_humidityRatio_Max_");
            if (index == -1 || !dataAccess.GetData(index, ref humidityRatio_Max) || double.IsNaN(humidityRatio_Max))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            bool isMollier = true;
            index = Params.IndexOfInputParam("_chartType_");
            if (index == -1 || !dataAccess.GetData(index, ref isMollier))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            ChartType chartType = isMollier == true ? ChartType.Mollier : ChartType.Psychrometric;

            double pressure = Standard.Pressure;
            index = Params.IndexOfInputParam("_pressure_");
            if(index != -1)
            {
                if(!dataAccess.GetData(index, ref pressure))
                {
                    pressure = Standard.Pressure;
                }
            }

            List<double> values;
            DataTree<GooMollierPoint> dataTree;
            List<GooMollierChartObject> gooMollierChartObjects = null;

            List<ConstantValueCurve> constantValueCurves = null;

            index = Params.IndexOfOutputParam("_chartType_");
            if (index != -1)
            {
                dataAccess.SetData(index, isMollier);
            }

            Range<double> humidityRatioRange = new Range<double>(humidityRatio_Min / 1000, humidityRatio_Max / 1000);
            Range<double> dryBulbTemperatureRange = new Range<double>(temperature_Min, temperature_Max);


            //CREATING DENSITY OUTPUT

            values = null;
            dataTree = null;
            gooMollierChartObjects = null;

            constantValueCurves = Core.Mollier.Create.ConstantValueCurves_Density(new Range<double>(Default.Density_Min, Default.Density_Max), Default.Density_Interval, pressure);
            if(constantValueCurves != null)
            {
                constantValueCurves = constantValueCurves.ConvertAll(x => x.Clamp(humidityRatioRange, dryBulbTemperatureRange));
                constantValueCurves.RemoveAll(x => x == null || double.IsNaN(x.Value));

                System.Drawing.Color color = Default.Density_Color;

                values = constantValueCurves.ConvertAll(x => x.Value);

                dataTree = new DataTree<GooMollierPoint>();
                gooMollierChartObjects = new List<GooMollierChartObject>();
                for (int i = 0; i < constantValueCurves.Count; i++)
                {
                    List<MollierPoint> mollierPoints = constantValueCurves[i]?.MollierPoints;
                    if(mollierPoints == null || mollierPoints.Count == 0)
                    {
                        gooMollierChartObjects.Add(null);
                        continue;
                    }

                    GH_Path path = new GH_Path(i);
                    mollierPoints?.ForEach(x => dataTree.Add(new GooMollierPoint(x), path));

                    gooMollierChartObjects.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierCurve(constantValueCurves[i], color), chartType, 0)));
                }
            }

            index = Params.IndexOfOutputParam("Density Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree);
            }

            index = Params.IndexOfOutputParam("Density Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, values);
            }

            index = Params.IndexOfOutputParam("Density Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, gooMollierChartObjects);
            }

            //CREATING ENTHALPY OUTPUT

            values = null;
            dataTree = null;
            gooMollierChartObjects = null;

            constantValueCurves = Core.Mollier.Create.ConstantValueCurves_Enthalpy(new Range<double>(Default.Enthalpy_Min * 1000, Default.Enthalpy_Max * 1000), Default.Enthalpy_Interval * 1000, pressure);
            if (constantValueCurves != null)
            {
                constantValueCurves = constantValueCurves.ConvertAll(x => x.Clamp(humidityRatioRange, dryBulbTemperatureRange));
                constantValueCurves.RemoveAll(x => x == null || double.IsNaN(x.Value));

                System.Drawing.Color color = Default.Enthalpy_Color;

                values = constantValueCurves.ConvertAll(x => x.Value);

                dataTree = new DataTree<GooMollierPoint>();
                gooMollierChartObjects = new List<GooMollierChartObject>();
                for (int i = 0; i < constantValueCurves.Count; i++)
                {
                    List<MollierPoint> mollierPoints = constantValueCurves[i]?.MollierPoints;
                    if (mollierPoints == null || mollierPoints.Count == 0)
                    {
                        gooMollierChartObjects.Add(null);
                        continue;
                    }

                    GH_Path path = new GH_Path(i);
                    mollierPoints?.ForEach(x => dataTree.Add(new GooMollierPoint(x), path));

                    gooMollierChartObjects.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierCurve(constantValueCurves[i], color), chartType, 0)));
                }
            }

            index = Params.IndexOfOutputParam("Enthalpy Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree);
            }

            index = Params.IndexOfOutputParam("Enthalpy Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, values);
            }

            index = Params.IndexOfOutputParam("Enthalpy Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, gooMollierChartObjects);
            }

            //CREATING SPECIFIC VOLUME OUTPUT

            values = null;
            dataTree = null;
            gooMollierChartObjects = null;

            constantValueCurves = Core.Mollier.Create.ConstantValueCurves_SpecificVolume(new Range<double>(Default.SpecificVolume_Min, Default.SpecificVolume_Max), Default.SpecificVolume_Interval, pressure);
            if (constantValueCurves != null)
            {
                constantValueCurves = constantValueCurves.ConvertAll(x => x.Clamp(humidityRatioRange, dryBulbTemperatureRange));
                constantValueCurves.RemoveAll(x => x == null || double.IsNaN(x.Value));

                System.Drawing.Color color = Default.SpecificVolume_Color;

                values = constantValueCurves.ConvertAll(x => x.Value);

                dataTree = new DataTree<GooMollierPoint>();
                gooMollierChartObjects = new List<GooMollierChartObject>();
                for (int i = 0; i < constantValueCurves.Count; i++)
                {
                    List<MollierPoint> mollierPoints = constantValueCurves[i]?.MollierPoints;
                    if (mollierPoints == null || mollierPoints.Count == 0)
                    {
                        gooMollierChartObjects.Add(null);
                        continue;
                    }

                    GH_Path path = new GH_Path(i);
                    mollierPoints?.ForEach(x => dataTree.Add(new GooMollierPoint(x), path));

                    gooMollierChartObjects.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierCurve(constantValueCurves[i], color), chartType, 0)));
                }
            }

            index = Params.IndexOfOutputParam("Specific Volume Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree);
            }

            index = Params.IndexOfOutputParam("Specific Volume Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, values);
            }

            index = Params.IndexOfOutputParam("Specific Volume Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, gooMollierChartObjects);
            }


            //CREATING WET BULB TEMPERATURE OUTPUT

            values = null;
            dataTree = null;
            gooMollierChartObjects = null;

            constantValueCurves = Core.Mollier.Create.ConstantValueCurves_WetBulbTemperature(new Range<double>(Default.DryBulbTemperature_Min, Default.DryBulbTemperature_Max), Default.DryBulbTemperature_Interval, pressure);
            if (constantValueCurves != null)
            {
                constantValueCurves = constantValueCurves.ConvertAll(x => x.Clamp(humidityRatioRange, dryBulbTemperatureRange));
                constantValueCurves.RemoveAll(x => x == null || double.IsNaN(x.Value));

                System.Drawing.Color color = Default.WetBulbTemperature_Color;

                values = constantValueCurves.ConvertAll(x => x.Value);

                dataTree = new DataTree<GooMollierPoint>();
                gooMollierChartObjects = new List<GooMollierChartObject>();
                for (int i = 0; i < constantValueCurves.Count; i++)
                {
                    List<MollierPoint> mollierPoints = constantValueCurves[i]?.MollierPoints;
                    if (mollierPoints == null || mollierPoints.Count == 0)
                    {
                        gooMollierChartObjects.Add(null);
                        continue;
                    }

                    GH_Path path = new GH_Path(i);
                    mollierPoints?.ForEach(x => dataTree.Add(new GooMollierPoint(x), path));

                    gooMollierChartObjects.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierCurve(constantValueCurves[i], color), chartType, 0)));
                }
            }

            index = Params.IndexOfOutputParam("Wet Bulb Temperature Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree);
            }

            index = Params.IndexOfOutputParam("Wet Bulb Temperature Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, values);
            }

            index = Params.IndexOfOutputParam("Wet Bulb Temperature Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, gooMollierChartObjects);
            }


            //CREATING RELATIVE HUMIDITY OUTPUT

            values = null;
            dataTree = null;
            gooMollierChartObjects = null;

            constantValueCurves = Core.Mollier.Create.ConstantValueCurves_RelativeHumidity(new Range<double>(10, 100), 10, pressure, new Range<double>(Default.DryBulbTemperature_Min, Default.DryBulbTemperature_Max));
            if (constantValueCurves != null)
            {
                constantValueCurves = constantValueCurves.ConvertAll(x => x.Clamp(humidityRatioRange, dryBulbTemperatureRange));
                constantValueCurves.RemoveAll(x => x == null || double.IsNaN(x.Value));

                System.Drawing.Color color = Default.RelativeHumidity_Color;

                values = constantValueCurves.ConvertAll(x => x.Value);

                dataTree = new DataTree<GooMollierPoint>();
                gooMollierChartObjects = new List<GooMollierChartObject>();
                for (int i = 0; i < constantValueCurves.Count; i++)
                {
                    List<MollierPoint> mollierPoints = constantValueCurves[i]?.MollierPoints;
                    if (mollierPoints == null || mollierPoints.Count == 0)
                    {
                        gooMollierChartObjects.Add(null);
                        continue;
                    }

                    GH_Path path = new GH_Path(i);
                    mollierPoints?.ForEach(x => dataTree.Add(new GooMollierPoint(x), path));

                    gooMollierChartObjects.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierCurve(constantValueCurves[i], color), chartType, 0)));
                }
            }

            index = Params.IndexOfOutputParam("Relative Humidity Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree);
            }

            index = Params.IndexOfOutputParam("Relative Humidity Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, values);
            }

            index = Params.IndexOfOutputParam("Relative Humidity Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, gooMollierChartObjects);
            }


            //CREATING DRY BULB TEMPERATURE OUTPUT

            values = null;
            dataTree = null;
            gooMollierChartObjects = null;

            constantValueCurves = Core.Mollier.Create.ConstantTemperatureCurves_DryBulbTemperature(new Range<double>(Default.DryBulbTemperature_Min, Default.DryBulbTemperature_Max), Default.DryBulbTemperature_Interval, pressure)?.ConvertAll(x => x as ConstantValueCurve);
            if (constantValueCurves != null)
            {
                constantValueCurves = constantValueCurves.ConvertAll(x => x.Clamp(humidityRatioRange, dryBulbTemperatureRange));
                constantValueCurves.RemoveAll(x => x == null || double.IsNaN(x.Value));

                System.Drawing.Color color = Default.DryBulbTemperature_Color;

                values = constantValueCurves.ConvertAll(x => x.Value);

                dataTree = new DataTree<GooMollierPoint>();
                gooMollierChartObjects = new List<GooMollierChartObject>();
                for (int i = 0; i < constantValueCurves.Count; i++)
                {
                    List<MollierPoint> mollierPoints = constantValueCurves[i]?.MollierPoints;
                    if (mollierPoints == null || mollierPoints.Count == 0)
                    {
                        gooMollierChartObjects.Add(null);
                        continue;
                    }

                    GH_Path path = new GH_Path(i);
                    mollierPoints?.ForEach(x => dataTree.Add(new GooMollierPoint(x), path));

                    gooMollierChartObjects.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierCurve(constantValueCurves[i], color), chartType, 0)));
                }
            }

            index = Params.IndexOfOutputParam("Dry Bulb Temperature Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree);
            }

            index = Params.IndexOfOutputParam("Dry Bulb Temperature Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, values);
            }

            index = Params.IndexOfOutputParam("Dry Bulb Temperature Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, gooMollierChartObjects);
            }
        }

    }
}