using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierHourlyValues : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("331e89c1-a787-4694-b732-5e8cbba07256");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.1";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Mollier;

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public SAMMollierHourlyValues()
          : base("SAMMollier.HourlyValues", "SAMMollier.HourlyValues", "Gets hourly values by MollierPointProperty enum", "SAM", "Analytical")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_mollierPoints", NickName = "_mollierPoints", Description = "SAM Mollier Points", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_String { Name = "_mollierPointProperty", NickName = "_mollierPointProperty", Description = "Mollier Point Property", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Integer { Name = "hoursOfYear_", NickName = "hoursOfYear_", Description = "Hours of year [0-8759]", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));


                return result.ToArray();
            }
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooIndexedObjectsParam() { Name = "values", NickName = "values", Description = "Values", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="dataAccess">
        /// The DA object is used to retrieve from inputs and store in outputs.
        /// </param>
        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index = -1;

            index = Params.IndexOfInputParam("_mollierPoints");
            List<MollierPoint> mollierPoints = new List<MollierPoint>();
            if (index == -1 || !dataAccess.GetDataList(index, mollierPoints) || mollierPoints == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            string text = null;
            index = Params.IndexOfInputParam("_mollierPointProperty");
            if (index == -1 || !dataAccess.GetData(index, ref text) || text == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            if(!Core.Query.TryGetEnum(text, out MollierPointProperty mollierPointProperty))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            List<int> indexes = new List<int>();
            index = Params.IndexOfInputParam("hoursOfYear_");
            if (index != -1)
            {
                dataAccess.GetDataList(index, indexes);
            }

            if (indexes == null || indexes.Count == 0)
            {
                indexes = new List<int>();
                for (int i = 0; i < mollierPoints.Count; i++)
                {
                    indexes.Add(i);
                }
            }

            IndexedDoubles indexDoubles = new IndexedDoubles();
            foreach(int index_Temp in indexes)
            {
                indexDoubles.Add(index_Temp, mollierPoints[index_Temp][mollierPointProperty]);
            }

            if (indexDoubles == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfOutputParam("values");
            if (index != -1)
            {
                dataAccess.SetData(index, indexDoubles);
            }
        }
    }
}