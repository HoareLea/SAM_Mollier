﻿using Grasshopper.Kernel;
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
        public override string LatestComponentVersion => "1.0.6";

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

                result.Add(new GH_SAMParam(new GooMollierChartObjectParam() { Name = "Diagram Temperature Lines", NickName = "DiagramTemperatureLines", Description = "Contains diagram temperature lines as curves used ONLY to construct Mollier diagram", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "Diagram Temperature Values", NickName = "diagramTemperatures", Description = "Values of diagram temperature lines used ONLY to construct Mollier diagram", Access = GH_ParamAccess.list }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Diagram Temperature Points", NickName = "diagramTemperaturePoints", Description = "MollierPoints used to create diagram temperature lines used ONLY to construct Mollier diagram", Access = GH_ParamAccess.tree }, ParamVisibility.Voluntary));

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
          : base("SAMMollier.CreateDiagram ", "SAMMollier.CreateDiagram ",
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

            //CREATING DENSITY OUTPUT
            Dictionary<double, List<MollierPoint>> dictionary_Density = Core.Mollier.Query.ConstantDensityPoints(humidityRatio_Max: humidityRatio_Max, humidityRatio_Min: humidityRatio_Min, dryBulbTemperature_Min: temperature_Min, dryBulbTemperature_Max: temperature_Max);
            List<double> densities = new List<double>(dictionary_Density.Keys);

            DataTree<GooMollierPoint> dataTree_Densities = new DataTree<GooMollierPoint>();
            List<GooMollierChartObject> densityLines = new List<GooMollierChartObject>();
            for (int i = 0; i < densities.Count; i++)
            {
                GH_Path path = new GH_Path(i);
                List<MollierPoint> mollierPoints = dictionary_Density[densities[i]];

                if (mollierPoints == null || mollierPoints.Count == 0)
                {
                    continue;
                }

                System.Drawing.Color color = System.Drawing.Color.LightBlue;

                UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(mollierPoints[0], mollierPoints[1]);
                densityLines.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierProcess(undefinedProcess, color), chartType, 0)));

                //global::Rhino.Geometry.Polyline polyLine = new global::Rhino.Geometry.Polyline();
                //if (mollierPoints != null)
                //{
                //    double x1 = chartType == ChartType.Mollier ? mollierPoints[0].HumidityRatio * 1000 : mollierPoints[0].DryBulbTemperature;
                //    double y1 = chartType == ChartType.Mollier ? mollierPoints[0].DryBulbTemperature : mollierPoints[0].HumidityRatio * 1000;
                //    double x2 = chartType == ChartType.Mollier ? mollierPoints[1].HumidityRatio * 1000 : mollierPoints[1].DryBulbTemperature;
                //    double y2 = chartType == ChartType.Mollier ? mollierPoints[1].DryBulbTemperature : mollierPoints[1].HumidityRatio * 1000;

                //    if (double.IsNaN(x1) || double.IsNaN(x2) || double.IsNaN(y1) || double.IsNaN(y2) || (y1 == y2 && x1 == x2))
                //    {
                //        continue;
                //    }
                //    polyLine.Add(x1, y1, 0);
                //    polyLine.Add(x2, y2, 0);
                //}
                //global::Rhino.Geometry.PolylineCurve polyLineCurve = new global::Rhino.Geometry.PolylineCurve(polyLine);
                //if (polyLineCurve.PointAtStart == polyLineCurve.PointAtEnd)
                //{
                //    continue;
                //}
                //mollierPoints?.ForEach(x => dataTree_Densities.Add(new GooMollierPoint(x), path));
                //densityLines.Add(new GooMollierChartObject(new MollierGeometry(polyLineCurve, color)));
            }
            index = Params.IndexOfOutputParam("Density Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_Densities);
            }
            index = Params.IndexOfOutputParam("Density Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, densities);
            }
            index = Params.IndexOfOutputParam("Density Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, densityLines);
            }

            //CREATING ENTHALPY OUTPUT
            Dictionary<double, List<MollierPoint>> dictionary_Enthalpy = Core.Mollier.Query.ConstantEnthalpyPoints(humidityRatio_Max: humidityRatio_Max, humidityRatio_Min: humidityRatio_Min, dryBulbTemperature_Min: temperature_Min, dryBulbTemperature_Max: temperature_Max);
            List<double> enthalpies = new List<double>(dictionary_Enthalpy.Keys);

            DataTree<GooMollierPoint> dataTree_Enthalpies = new DataTree<GooMollierPoint>();
            List<GooMollierChartObject> enthalpyLines = new List<GooMollierChartObject>();
            for (int i = 0; i < enthalpies.Count; i++)
            {
                GH_Path path = new GH_Path(i);
                List<MollierPoint> mollierPoints = dictionary_Enthalpy[enthalpies[i]];
                System.Drawing.Color color = System.Drawing.Color.LightBlue;
                if (mollierPoints != null)
                {
                    for(int j = 0; j < mollierPoints.Count - 1; j++)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(mollierPoints[j], mollierPoints[j + 1]);
                        enthalpyLines.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierProcess(undefinedProcess, color), chartType, 0)));
                    }

                    //global::Rhino.Geometry.Polyline polyLine = new global::Rhino.Geometry.Polyline();
                    //foreach (MollierPoint mollierPoint in mollierPoints)
                    //{
                    //    double X = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature;
                    //    double Y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                    //    global::Rhino.Geometry.Point3d point3D = new global::Rhino.Geometry.Point3d(X, Y, 0);
                    //    polyLine.Add(point3D);
                    //}
                    //global::Rhino.Geometry.PolylineCurve polyLineCurve = new global::Rhino.Geometry.PolylineCurve(polyLine);
                    //if (polyLineCurve.PointAtStart == polyLineCurve.PointAtEnd)
                    //{
                    //    continue;
                    //}
                    //mollierPoints?.ForEach(x => dataTree_Enthalpies.Add(new GooMollierPoint(x), path));
                    //enthalpyLines.Add(new GooMollierObject(new MollierGeometry(polyLineCurve, color)));
                }
            }
            index = Params.IndexOfOutputParam("Enthalpy Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_Enthalpies);
            }
            index = Params.IndexOfOutputParam("Enthalpy Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, enthalpies);
            }
            index = Params.IndexOfOutputParam("Enthalpy Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, enthalpyLines);
            }

            //CREATING SPECIFIC VOLUME OUTPUT
            Dictionary<double, List<MollierPoint>> dictionary_SpecificVolume = Core.Mollier.Query.ConstantSpecificVolumePoints(humidityRatio_Max: humidityRatio_Max, humidityRatio_Min: humidityRatio_Min, dryBulbTemperature_Min: temperature_Min, dryBulbTemperature_Max: temperature_Max);
            List<double> specificVolumes = new List<double>(dictionary_SpecificVolume.Keys);

            DataTree<GooMollierPoint> dataTree_SpecificVolumes = new DataTree<GooMollierPoint>();
            List<GooMollierChartObject> specificVolumeLines = new List<GooMollierChartObject>();
            for (int i = 0; i < specificVolumes.Count; i++)
            {
                GH_Path path = new GH_Path(i);
                List<MollierPoint> mollierPoints = dictionary_SpecificVolume[specificVolumes[i]];

                System.Drawing.Color color = System.Drawing.Color.LightBlue;
                if (mollierPoints != null)
                {
                    for (int j = 0; j < mollierPoints.Count - 1; j++)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(mollierPoints[j], mollierPoints[j + 1]);
                        specificVolumeLines.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierProcess(undefinedProcess, color), chartType, 0)));
                    }

                    //global::Rhino.Geometry.Polyline polyLine = new global::Rhino.Geometry.Polyline();
                    //foreach (MollierPoint mollierPoint in mollierPoints)
                    //{
                    //    double X = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature;
                    //    double Y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                    //    global::Rhino.Geometry.Point3d point3D = new global::Rhino.Geometry.Point3d(X, Y, 0);
                    //    polyLine.Add(point3D);
                    //}
                    //global::Rhino.Geometry.PolylineCurve polyLineCurve = new global::Rhino.Geometry.PolylineCurve(polyLine);
                    //if (polyLineCurve.PointAtStart == polyLineCurve.PointAtEnd)
                    //{
                    //    continue;
                    //}
                    //mollierPoints?.ForEach(x => dataTree_SpecificVolumes.Add(new GooMollierPoint(x), path));
                    //specificVolumeLines.Add(new GooMollierObject(new MollierGeometry(polyLineCurve, color)));
                }
            }
            index = Params.IndexOfOutputParam("Specific Volume Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_SpecificVolumes);
            }
            index = Params.IndexOfOutputParam("Specific Volume Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, specificVolumes);
            }
            index = Params.IndexOfOutputParam("Specific Volume Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, specificVolumeLines);
            }

            //CREATING WET BULB TEMPERATURE OUTPUT
            Dictionary<double, List<MollierPoint>> dictionary_WetBulbTemperature = Core.Mollier.Query.ConstantWetBulbTemperaturePoints(humidityRatio_Max: humidityRatio_Max, humidityRatio_Min: humidityRatio_Min, dryBulbTemperature_Min: temperature_Min, dryBulbTemperature_Max: temperature_Max);
            List<double> wetBulbTemperatures = new List<double>(dictionary_WetBulbTemperature.Keys);

            DataTree<GooMollierPoint> dataTree_WetBulbTemperature = new DataTree<GooMollierPoint>();
            List<GooMollierChartObject> wetBulbTemperatureLines = new List<GooMollierChartObject>();
            for (int i = 0; i < wetBulbTemperatures.Count; i++)
            {
                GH_Path path = new GH_Path(i);
                List<MollierPoint> mollierPoints = dictionary_WetBulbTemperature[wetBulbTemperatures[i]];

                System.Drawing.Color color = System.Drawing.Color.LightBlue;
                if (mollierPoints != null)
                {
                    for (int j = 0; j < mollierPoints.Count - 1; j++)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(mollierPoints[j], mollierPoints[j + 1]);
                        wetBulbTemperatureLines.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierProcess(undefinedProcess, color), chartType, 0)));
                    }


                    //global::Rhino.Geometry.Polyline polyLine = new global::Rhino.Geometry.Polyline();
                    //foreach (MollierPoint mollierPoint in mollierPoints)
                    //{
                    //    double X = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature;
                    //    double Y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                    //    global::Rhino.Geometry.Point3d point3D = new global::Rhino.Geometry.Point3d(X, Y, 0);
                    //    polyLine.Add(point3D);
                    //}
                    //global::Rhino.Geometry.PolylineCurve polyLineCurve = new global::Rhino.Geometry.PolylineCurve(polyLine);
                    //if (polyLineCurve.PointAtStart == polyLineCurve.PointAtEnd)
                    //{
                    //    continue;
                    //}
                    //mollierPoints?.ForEach(x => dataTree_WetBulbTemperature.Add(new GooMollierPoint(x), path));
                    //wetBulbTemperatureLines.Add(new GooMollierObject(new MollierGeometry(polyLineCurve, color)));
                }
            }
            index = Params.IndexOfOutputParam("Wet Bulb Temperature Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_WetBulbTemperature);
            }
            index = Params.IndexOfOutputParam("Wet Bulb Temperature Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, wetBulbTemperatures);
            }
            index = Params.IndexOfOutputParam("Wet Bulb Temperature Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, wetBulbTemperatureLines);
            }

            //CREATING RELATIVE HUMIDITY OUTPUT
            Dictionary<double, List<MollierPoint>> dictionary_relativeHumidity = Core.Mollier.Query.ConstantRelativeHumidityPoints((int)temperature_Min, (int)temperature_Max, Standard.Pressure, humidityRatio_Min: humidityRatio_Min, humidityRatio_Max: humidityRatio_Max); ;
            List<double> relativeHumidities = new List<double>(dictionary_relativeHumidity.Keys);

            DataTree<GooMollierPoint> dataTree_RelativeHumidity = new DataTree<GooMollierPoint>();
            List<GooMollierChartObject> relativeHumidityLines = new List<GooMollierChartObject>();
            for (int i = 0; i < relativeHumidities.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_relativeHumidity[relativeHumidities[i]];
                mollierPoints?.ForEach(x => dataTree_RelativeHumidity.Add(new GooMollierPoint(x), path));

                System.Drawing.Color color = System.Drawing.Color.LightBlue;

                if (mollierPoints != null)
                {
                    for (int j = 0; j < mollierPoints.Count - 1; j++)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(mollierPoints[j], mollierPoints[j + 1]);
                        relativeHumidityLines.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierProcess(undefinedProcess, color), chartType, 0)));
                    }

                    //global::Rhino.Geometry.Polyline polyLine = new global::Rhino.Geometry.Polyline();
                    //foreach (MollierPoint mollierPoint in mollierPoints)
                    //{
                    //    double X = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature;
                    //    double Y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                    //    global::Rhino.Geometry.Point3d point3D = new global::Rhino.Geometry.Point3d(X, Y, 0);
                    //    polyLine.Add(point3D);
                    //}
                    //global::Rhino.Geometry.PolylineCurve polyLineCurve = new global::Rhino.Geometry.PolylineCurve(polyLine);
                    //relativeHumidityLines.Add(new GooMollierObject(new MollierGeometry(polyLineCurve, color)));
                }
            }
            index = Params.IndexOfOutputParam("Relative Humidity Points");
            if (index != -1)
            {
                dataAccess.SetDataTree(index, dataTree_RelativeHumidity);
            }
            index = Params.IndexOfOutputParam("Relative Humidity Values");
            if (index != -1)
            {
                dataAccess.SetDataList(index, relativeHumidities);
            }
            index = Params.IndexOfOutputParam("Relative Humidity Lines");
            if (index != -1)
            {
                dataAccess.SetDataList(index, relativeHumidityLines);
            }

            //CREATING DIAGRAM TEMPERATURE OUTPUT
            Dictionary<double, List<MollierPoint>> dictionary_diagramTemperature = Core.Mollier.Query.ConstantDiagramTemperaturePoints((int)temperature_Min, (int)temperature_Max, Standard.Pressure, humidityRatio_Min: humidityRatio_Min, humidityRatio_Max: humidityRatio_Max);
            List<double> diagramTemperatures = new List<double>(dictionary_diagramTemperature.Keys);

            DataTree<GooMollierPoint> dataTree_DiagramTemperature = new DataTree<GooMollierPoint>();
            List<GooMollierChartObject> diagramTemperatureLines = new List<GooMollierChartObject>();
            for (int i = 0; i < diagramTemperatures.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_diagramTemperature[diagramTemperatures[i]];
                mollierPoints?.ForEach(x => dataTree_DiagramTemperature.Add(new GooMollierPoint(x), path));

                System.Drawing.Color color = System.Drawing.Color.LightBlue;

                if (mollierPoints != null)
                {
                    for (int j = 0; j < mollierPoints.Count - 1; j++)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(mollierPoints[j], mollierPoints[j + 1]);
                        diagramTemperatureLines.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierProcess(undefinedProcess, color), chartType, 0)));
                    }

                    //global::Rhino.Geometry.Polyline polyLine = new global::Rhino.Geometry.Polyline();
                    //foreach (MollierPoint mollierPoint in mollierPoints)
                    //{
                    //    double X = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature;
                    //    double Y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                    //    global::Rhino.Geometry.Point3d point3D = new global::Rhino.Geometry.Point3d(X, Y, 0);
                    //    polyLine.Add(point3D);
                    //}
                    //global::Rhino.Geometry.PolylineCurve polyLineCurve = new global::Rhino.Geometry.PolylineCurve(polyLine);
                    //diagramTemperatureLines.Add(new GooMollierObject(new MollierGeometry(polyLineCurve, color)));
                }
            }

            index = Params.IndexOfOutputParam("Diagram Temperature Points");
            if (index != -1)//&& ChartType == ChartType.Mollier
            {
                dataAccess.SetDataTree(index, dataTree_DiagramTemperature);
            }
            index = Params.IndexOfOutputParam("Diagram Temperature Values");
            if (index != -1)//&& ChartType == ChartType.Mollier
            {
                dataAccess.SetDataList(index, diagramTemperatures);
            }
            index = Params.IndexOfOutputParam("Diagram Temperature Lines");
            if (index != -1)//&& ChartType == ChartType.Mollier
            {
                dataAccess.SetDataList(index, diagramTemperatureLines);
            }

            index = Params.IndexOfOutputParam("_chartType_");
            if (index != -1)
            {
                dataAccess.SetData(index, isMollier);
            }



            //---
            //CREATING DRY BULB TEMPERATURE OUTPUT
            Dictionary<double, List<MollierPoint>> dictionary_dryBulbTemperature = Core.Mollier.Query.ConstantDryBulbTemperaturePoints((int)temperature_Min, (int)temperature_Max, Standard.Pressure, humidityRatio_Min: humidityRatio_Min, humidityRatio_Max: humidityRatio_Max);
            List<double> dryBulbTemperatures = new List<double>(dictionary_diagramTemperature.Keys);

            DataTree<GooMollierPoint> dataTree_DryBulbTemperature = new DataTree<GooMollierPoint>();
            List<GooMollierChartObject> dryBulbTemperatureLines = new List<GooMollierChartObject>();
            for (int i = 0; i < dryBulbTemperatures.Count; i++)
            {
                GH_Path path = new GH_Path(i);

                List<MollierPoint> mollierPoints = dictionary_dryBulbTemperature[dryBulbTemperatures[i]];
                mollierPoints?.ForEach(x => dataTree_DryBulbTemperature.Add(new GooMollierPoint(x), path));

                System.Drawing.Color color = System.Drawing.Color.LightBlue;

                if (mollierPoints != null)
                {

                    for (int j = 0; j < mollierPoints.Count - 1; j++)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(mollierPoints[j], mollierPoints[j + 1]);
                        dryBulbTemperatureLines.Add(new GooMollierChartObject(new MollierChartObject(new UIMollierProcess(undefinedProcess, color), chartType, 0)));
                    }

                    //global::Rhino.Geometry.Polyline polyLine = new global::Rhino.Geometry.Polyline();
                    //foreach (MollierPoint mollierPoint in mollierPoints)
                    //{
                    //    double X = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature;
                    //    double Y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                    //    global::Rhino.Geometry.Point3d point3D = new global::Rhino.Geometry.Point3d(X, Y, 0);
                    //    polyLine.Add(point3D);
                    //}
                    //global::Rhino.Geometry.PolylineCurve polyLineCurve = new global::Rhino.Geometry.PolylineCurve(polyLine);
                    //dryBulbTemperatureLines.Add(new GooMollierObject(new MollierGeometry(polyLineCurve, color)));
                }
            }

            index = Params.IndexOfOutputParam("Dry Bulb Temperature Points");
            if (index != -1)//&& ChartType == ChartType.Mollier
            {
                dataAccess.SetDataTree(index, dataTree_DryBulbTemperature);
            }
            index = Params.IndexOfOutputParam("Dry Bulb Temperature Values");
            if (index != -1)//&& ChartType == ChartType.Mollier
            {
                dataAccess.SetDataList(index, dryBulbTemperatures);
            }
            index = Params.IndexOfOutputParam("Dry Bulb Temperature Lines");
            if (index != -1)//&& ChartType == ChartType.Mollier
            {
                dataAccess.SetDataList(index, dryBulbTemperatureLines);
            }

            index = Params.IndexOfOutputParam("_chartType_");
            if (index != -1)
            {
                dataAccess.SetData(index, isMollier);
            }
            //--
        }

    }
}