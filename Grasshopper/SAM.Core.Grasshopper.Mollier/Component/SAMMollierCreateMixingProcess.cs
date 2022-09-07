using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierCreateMixingProcess : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("2de5bbe0-38f0-4df7-8d64-dea4dac70ca6");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

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
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_point_1", NickName = "_point_1", Description = "MollierPoint for first air flow", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_airflow_1", NickName = "_airflow_1", Description = "First Airflow [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_point_2", NickName = "_point_2", Description = "MollierPoint for second air flow", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_airflow_2", NickName = "_airflow_2", Description = "Second Airflow [m3/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                global::Grasshopper.Kernel.Parameters.Param_Colour param_Colour = null;
                param_Colour = new global::Grasshopper.Kernel.Parameters.Param_Colour() { Name = "_color_", NickName = "_color_", Description = "Colour RGB", Access = GH_ParamAccess.item, Optional = true };
                result.Add(new GH_SAMParam(param_Colour, ParamVisibility.Voluntary));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "mixingProcess", NickName = "mixingProcess", Description = "Mixing Process", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "end", NickName = "end", Description = "End", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Colour() { Name = "color", NickName = "color", Description = "Color", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateMixingProcess()
          : base("SAMMollier.CreateMixingProcess", "SAMMollier.CreateMixingProcess",
              "Creates MixingProcess",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_point_1");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            MollierPoint point_1 = null;
            if (!dataAccess.GetData(index, ref point_1) || point_1 == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_point_2");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            MollierPoint point_2 = null;
            if (!dataAccess.GetData(index, ref point_2) || point_1 == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_airflow_1");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double airflow_1 = double.NaN;
            if (!dataAccess.GetData(index, ref airflow_1) || double.IsNaN(airflow_1))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_airflow_2");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double airflow_2 = double.NaN;
            if (!dataAccess.GetData(index, ref airflow_2) || double.IsNaN(airflow_2))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            Color color = Color.Empty;

            index = Params.IndexOfInputParam("_color_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref color);
            }

            MixingProcess mixingProcess = Core.Mollier.Create.MixingProcess(point_1, point_2, airflow_1, airflow_2);


            index = Params.IndexOfOutputParam("mixingProcess");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierProcess(mixingProcess, color));
            }
            else
            {
                return;
            }
            MollierPoint end = new MollierPoint(mixingProcess.End);
            index = Params.IndexOfOutputParam("end");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierPoint(end));
            }
            index = Params.IndexOfOutputParam("color");
            if (index != -1)
            {
                dataAccess.SetData(index, color);
            }
        }
    }
}