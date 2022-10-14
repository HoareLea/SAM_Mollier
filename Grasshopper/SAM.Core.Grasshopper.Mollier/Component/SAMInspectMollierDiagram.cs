using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMInspectMollierDiagram : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("95611e60-2b7d-42d5-85b1-853fb8872b16");

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

                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = null;
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_temperature_Min_", NickName = "_temperature_Min_", Description = "Minimal value of temperature axis", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(-20);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_temperature_Max_", NickName = "_temperature_Max_", Description = "Maximal value of temperature axis", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(50);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatio_Min_", NickName = "_humidityRatio_Min_", Description = "Minimal value of humidity ratio axis", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(0);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatio_Max_", NickName = "_humidityRatio_Max_", Description = "Maximal value of humidity ratio axis", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(35);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));
                global::Grasshopper.Kernel.Parameters.Param_Boolean param_Bool = null;
                param_Bool = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_chartType_", NickName = "_chartType_", Description = "Type of the chart: true - Mollier Chart, false - Psychrometric Chart", Access = GH_ParamAccess.item, Optional = true };
                param_Bool.SetPersistentData(true);
                result.Add(new GH_SAMParam(param_Bool, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {

                List<GH_SAMParam> result = new List<GH_SAMParam>();
                
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "relativeHumidities", NickName = "relativeHumidities", Description = "TODO", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "relativeHumidityPoints", NickName = "realtiveHumidityPoints", Description = "TODO", Access = GH_ParamAccess.tree }, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "diagramTemperatures", NickName = "diagramTemperatures", Description = "TODO", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "diagramTemperaturePoints", NickName = "diagramTemperaturePoints", Description = "TODO", Access = GH_ParamAccess.tree }, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "densities", NickName = "densities", Description = "TODO", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "densityPoints", NickName = "densityPoints", Description = "TODO", Access = GH_ParamAccess.tree }, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "enthalpies", NickName = "enthalpies", Description = "TODO", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "enthalpyPoints", NickName = "enthalpyPoints", Description = "TODO", Access = GH_ParamAccess.tree }, ParamVisibility.Binding));
               
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "specificVolumes", NickName = "specificVolumes", Description = "TODO", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "specificVolumePoints", NickName = "specificVolumePoints", Description = "TODO", Access = GH_ParamAccess.tree }, ParamVisibility.Binding));
                
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "wetBulbTemperatures", NickName = "wetBulbTemperatures", Description = "TODO", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "wetBulbTemperaturePoints", NickName = "wetBulbTemperaturePoints", Description = "TODO", Access = GH_ParamAccess.tree }, ParamVisibility.Binding));


                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMInspectMollierDiagram()
          : base("SAMMollier.InspectMollierDiagram ", "SAMInspectMollierDiagram ",
              "Inspects Mollier Diagram",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            double temperature_Min = double.NaN;
            index = Params.IndexOfInputParam("_temperature_Min_");
            if(index == -1 || !dataAccess.GetData(index, ref temperature_Min) || double.IsNaN(temperature_Min))
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

            bool chartType = true;
            index = Params.IndexOfInputParam("_chartType_");
            if (index == -1 || !dataAccess.GetData(index, ref chartType) || chartType == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            ChartType ChartType = chartType == true ? ChartType.Mollier : ChartType.Psychrometric;

            Dictionary<double, List<MollierPoint>> dictionary_Density = Core.Mollier.Query.DensityLine();

            List<double> densities = new List<double>(dictionary_Density.Keys);
            ////TODO: Maciek to fill densities

            DataTree<GooMollierPoint> dataTree_Densities = new DataTree<GooMollierPoint>();
            for (int i = 0; i < densities.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_Density[densities[i]];
                mollierPoints = CutLine(mollierPoints, ChartType, humidityRatio_Min, humidityRatio_Max, temperature_Max, temperature_Min);
                mollierPoints?.ForEach(x => dataTree_Densities.Add(new GooMollierPoint(x), path));
            }

            Dictionary<double, List<MollierPoint>> dictionary_Enthalpy = Core.Mollier.Query.EnthalpyLine(chartType: ChartType);

            List<double> enthalpies = new List<double>(dictionary_Enthalpy.Keys);
            ////TODO: Maciek to fill densities

            DataTree<GooMollierPoint> dataTree_Enthalpies = new DataTree<GooMollierPoint>();
            for (int i = 0; i < enthalpies.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_Enthalpy[enthalpies[i]];
                mollierPoints = CutLine(mollierPoints, ChartType, humidityRatio_Min, humidityRatio_Max, temperature_Max, temperature_Min);

                mollierPoints?.ForEach(x => dataTree_Enthalpies.Add(new GooMollierPoint(x), path));
            }

            Dictionary<double, List<MollierPoint>> dictionary_SpecificVolume = Core.Mollier.Query.SpecificVolumeLine();

            List<double> specificVolumes = new List<double>(dictionary_SpecificVolume.Keys);
            ////TODO: Maciek to fill densities

            DataTree<GooMollierPoint> dataTree_SpecificVolumes = new DataTree<GooMollierPoint>();
            for (int i = 0; i < specificVolumes.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_SpecificVolume[specificVolumes[i]];
                mollierPoints = CutLine(mollierPoints, ChartType, humidityRatio_Min, humidityRatio_Max, temperature_Max, temperature_Min);

                mollierPoints?.ForEach(x => dataTree_SpecificVolumes.Add(new GooMollierPoint(x), path));
            }

            Dictionary<double, List<MollierPoint>> dictionary_WetBulbTemperature = Core.Mollier.Query.WetBulbTemperatureLine();

            List<double> wetBulbTemperatures = new List<double>(dictionary_WetBulbTemperature.Keys);

            DataTree<GooMollierPoint> dataTree_WetBulbTemperature = new DataTree<GooMollierPoint>();
            for (int i = 0; i < wetBulbTemperatures.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_WetBulbTemperature[wetBulbTemperatures[i]];
                mollierPoints = CutLine(mollierPoints, ChartType, humidityRatio_Min, humidityRatio_Max, temperature_Max, temperature_Min);

                mollierPoints?.ForEach(x => dataTree_WetBulbTemperature.Add(new GooMollierPoint(x), path));
            }

            Dictionary<double, List<MollierPoint>> dictionary_relativeHumidity = Core.Mollier.Query.RelativeHumidityLine(-20, 50, Standard.Pressure);

            List<double> relativeHumidities = new List<double>(dictionary_relativeHumidity.Keys);

            DataTree<GooMollierPoint> dataTree_RelativeHumidity= new DataTree<GooMollierPoint>();
            for (int i = 0; i < relativeHumidities.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_relativeHumidity[relativeHumidities[i]];
                mollierPoints?.ForEach(x => dataTree_RelativeHumidity.Add(new GooMollierPoint(x), path));
            }

            Dictionary<double, List<MollierPoint>> dictionary_diagramTemperature = Core.Mollier.Query.DiagramTemperatureLine((int)temperature_Min, (int)temperature_Max, Standard.Pressure);

            List<double> diagramTemperatures = new List<double>(dictionary_diagramTemperature.Keys);

            DataTree<GooMollierPoint> dataTree_DiagramTemperature = new DataTree<GooMollierPoint>();
            for (int i = 0; i < diagramTemperatures.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_diagramTemperature[diagramTemperatures[i]];
                mollierPoints?.ForEach(x => dataTree_DiagramTemperature.Add(new GooMollierPoint(x), path));
            }



            index = Params.IndexOfOutputParam("densityPoints");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_Densities);
            }
            index = Params.IndexOfOutputParam("densities");
            if (index != -1)
            {
                dataAccess.SetDataList(index, densities);
            }
            index = Params.IndexOfOutputParam("enthalpyPoints");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_Enthalpies);
            }
            index = Params.IndexOfOutputParam("enthalpies");
            if (index != -1)
            {
                dataAccess.SetDataList(index, enthalpies);
            }
            index = Params.IndexOfOutputParam("specificVolumePoints");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_SpecificVolumes);
            }
            index = Params.IndexOfOutputParam("specificVolumes");
            if (index != -1)
            {
                dataAccess.SetDataList(index, specificVolumes);
            }
            index = Params.IndexOfOutputParam("wetBulbTemperaturePoints");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_WetBulbTemperature);
            }
            index = Params.IndexOfOutputParam("wetBulbTemperatures");
            if (index != -1)
            {
                dataAccess.SetDataList(index, wetBulbTemperatures);
            }
            index = Params.IndexOfOutputParam("relativeHumidityPoints");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_RelativeHumidity);
            }
            index = Params.IndexOfOutputParam("relativeHumidities");
            if (index != -1)
            {
                dataAccess.SetDataList(index, relativeHumidities);
            }
            index = Params.IndexOfOutputParam("diagramTemperaturePoints");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_DiagramTemperature);
            }
            index = Params.IndexOfOutputParam("diagramTemperatures");
            if (index != -1)
            {
                dataAccess.SetDataList(index, diagramTemperatures);
            }
        }

        private List<MollierPoint> CutLine(List<MollierPoint> mollierPoints, ChartType chartType, double humidityRatio_Min, double humidityRatio_Max, double temperature_Max, double temperature_Min)
        {
            if(mollierPoints == null || mollierPoints.Count < 2)
            {
                return null;
            }
            List<MollierPoint> result = new List<MollierPoint>();
            //MollierPoint point_1 = new MollierPoint(mollierPoints[0].DryBulbTemperature, mollierPoints[0].HumidityRatio * 1000, mollierPoints[0].Pressure);
            //MollierPoint point_2 = new MollierPoint(mollierPoints[1].DryBulbTemperature, mollierPoints[1].HumidityRatio * 1000, mollierPoints[1].Pressure);
            ////tu stworz wzor - a i b z y = ax + b
            ///
            MollierPoint point_1 = mollierPoints[0];
            MollierPoint point_2 = mollierPoints[1];
            humidityRatio_Max /= 1000;
            humidityRatio_Min /= 1000;
            double a = 0;
            double b = 0;
            if(chartType == ChartType.Mollier)
            {
                a = (point_1.DryBulbTemperature - point_2.DryBulbTemperature)/(point_1.HumidityRatio - point_2.HumidityRatio);
                b = point_1.DryBulbTemperature - a * point_1.HumidityRatio;
                if(point_1.HumidityRatio < humidityRatio_Min)
                {
                    point_1 = new MollierPoint(a * humidityRatio_Min + b, humidityRatio_Min, point_1.Pressure);
                }
                else if(point_1.HumidityRatio > humidityRatio_Max)
                {
                    point_1 = new MollierPoint(a * humidityRatio_Max + b, humidityRatio_Max, point_1.Pressure);
                }
                if(point_1.DryBulbTemperature < temperature_Min)
                {
                    point_1 = new MollierPoint(temperature_Min, (temperature_Min - b) / a, point_1.Pressure);
                }
                else if (point_1.DryBulbTemperature > temperature_Max)
                {
                    point_1 = new MollierPoint(temperature_Max, (temperature_Max - b) / a, point_1.Pressure);
                }

                if (point_2.HumidityRatio < humidityRatio_Min)
                {
                    point_2 = new MollierPoint(a * humidityRatio_Min + b, humidityRatio_Min, point_2.Pressure);
                }
                else if (point_2.HumidityRatio > humidityRatio_Max)
                {
                    point_2 = new MollierPoint(a * humidityRatio_Max + b, humidityRatio_Max, point_2.Pressure);
                }
                if (point_2.DryBulbTemperature < temperature_Min)
                {
                    point_2 = new MollierPoint(temperature_Min, (temperature_Min - b) / a, point_2.Pressure);
                }
                else if (point_2.DryBulbTemperature > temperature_Max)
                {
                    point_2 = new MollierPoint(temperature_Max, (temperature_Max - b) / a, point_2.Pressure);
                }
            }
            else
            {
                a = (point_1.HumidityRatio - point_2.HumidityRatio) / (point_1.DryBulbTemperature - point_2.DryBulbTemperature);
                b = point_1.HumidityRatio - a * point_1.DryBulbTemperature;
            }


            result.Add(point_1);
            result.Add(point_2);

            return result;
        }
    }
}