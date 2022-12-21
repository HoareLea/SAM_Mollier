using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;
using Grasshopper;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMGeometry : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("a0b93650-1575-4985-b0f2-640afce6aa13");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

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

                result.Add(new GH_SAMParam(new GooMollierGeometryParam() { Name = "Mollier Chart", NickName = "Inspect Mollier Lines", Description = "MollierGeometry, Base of Chart - output from InspectMollierDiagram output ", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));

               // global::Grasshopper.Kernel.Parameters.Param_Curve curves = null;
                //curves = new global::Grasshopper.Kernel.Parameters.Param_Curve() { Name = "Mollier Chart", NickName = "Inspect Mollier Lines", Description = "Base of Chart - output from InspectMollierDiagram output ", Access = GH_ParamAccess.list, Optional = true };
               // result.Add(new GH_SAMParam(curves, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Boolean param_Bool = null;
                param_Bool = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_chartType_", NickName = "_chartType_", Description = "Type of the chart: true - Mollier Chart, false - Psychrometric Chart", Access = GH_ParamAccess.item, Optional = true };
                param_Bool.SetPersistentData(true);
                result.Add(new GH_SAMParam(param_Bool, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "Mollier Points", NickName = "Mollier Points", Description = "Mollier Points", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "Mollier Processes", NickName = "Mollier Processes", Description = "Mollier Processes", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));


                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {

                List<GH_SAMParam> result = new List<GH_SAMParam>();

                result.Add(new GH_SAMParam(new GooMollierGeometryParam() { Name = "MollierGeometry", NickName = "MollierGeometry", Description = "Capable to contain chart lines, points and processes connected as geometry", Access = GH_ParamAccess.tree }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMGeometry()
          : base("SAMMollier.Geometry ", "SAMGeometry ",
              "Connects points, lines and processes",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {

            int index = Params.IndexOfInputParam("Mollier Chart");
            List<GooMollierGeometry> curves = new List<GooMollierGeometry>();
            if (index != -1)
            { 
                dataAccess.GetDataList(index, curves);
            }

            index = Params.IndexOfInputParam("Mollier Points");
            List<MollierPoint> points = new List<MollierPoint>();
            if (index != -1)
            {
                dataAccess.GetDataList(index, points);
            }

            index = Params.IndexOfInputParam("Mollier Processes");
            List<GooMollierProcess> processes = new List<GooMollierProcess>();
            if (index != -1)
            {
                dataAccess.GetDataList(index, processes);
            }   

            index = Params.IndexOfInputParam("_chartType_");
            bool chartType_input = true;
            if (index != -1)
            {
                dataAccess.GetData(index, ref chartType_input);
            }

            ChartType chartType = chartType_input == true ? ChartType.Mollier : ChartType.Psychrometric;

            List<GooMollierGeometry> processesLines = new List<GooMollierGeometry>();
            foreach (GooMollierProcess process in processes)
            {
                if (process == null)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
                System.Drawing.Color color = System.Drawing.Color.DarkGreen;
                if(process.Value is HeatingProcess)
                {
                    color = System.Drawing.Color.Red;
                }
                else if(process.Value is CoolingProcess)
                {
                    color = System.Drawing.Color.Blue;
                }
                if (process.Value is UIMollierProcess)
                {
                    UIMollierProcess process1 = (UIMollierProcess)process.Value;
                    color = process1.Color;
                }
                MollierPoint point_1 = process.Value.Start;
                MollierPoint point_2 = process.Value.End;
                double x1 = chartType == ChartType.Mollier ? point_1.HumidityRatio * 1000 : point_1.DryBulbTemperature;
                double y1 = chartType == ChartType.Mollier ? point_1.DryBulbTemperature : point_1.HumidityRatio * 1000;
                double x2 = chartType == ChartType.Mollier ? point_2.HumidityRatio * 1000 : point_2.DryBulbTemperature;
                double y2 = chartType == ChartType.Mollier ? point_2.DryBulbTemperature : point_2.HumidityRatio * 1000;
                Rhino.Geometry.Polyline polyLine = new Rhino.Geometry.Polyline();
                polyLine.Add(x1, y1, 0);
                polyLine.Add(x2, y2, 0);

                processesLines.Add(new GooMollierGeometry(new GH_MollierGeometry(new Rhino.Geometry.PolylineCurve(polyLine), color)));
            }

            DataTree<GH_Point> dataTree_Points = new DataTree<GH_Point>();
            foreach (MollierPoint mollierPoint in points)
            {
                if (mollierPoint == null)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
                double X = chartType == ChartType.Mollier ? mollierPoint.HumidityRatio * 1000 : mollierPoint.DryBulbTemperature; ;
                double Y = chartType == ChartType.Mollier ? mollierPoint.DryBulbTemperature : mollierPoint.HumidityRatio * 1000;
                GH_Point point = new GH_Point(new Rhino.Geometry.Point3d(X, Y, 0));
                dataTree_Points.Add(point);
            }
            //List<GooMollierGeometry> curvesList = new List<GooMollierGeometry>();
            //foreach (GH_MollierGeometry curve in curves)
            //{
            //    if (curve == null)
            //    {
            //        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
            //        return;
            //    }
            //    curvesList.Add(new GooMollierGeometry(curve));
            //}


            index = Params.IndexOfOutputParam("MollierGeometry");

            if (index != -1)
            {
                dataAccess.SetDataList(index, curves);
               // dataAccess.SetDataTree(index, dataTree_Points);
                dataAccess.SetDataList(index, processesLines);
            }

        }

    }
}