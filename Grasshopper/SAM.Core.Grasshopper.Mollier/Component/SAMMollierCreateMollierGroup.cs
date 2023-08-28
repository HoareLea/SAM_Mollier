using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierCreateMollierGroup : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("f5c44450-5d9e-4f7c-8a65-c8be36da7cd2");

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
                GooMollierObjectParam gooMollierObjectParam = new GooMollierObjectParam() { Name = "_mollierObjects", NickName = "_mollierObjects", Description = "Mollier Objects", Access = GH_ParamAccess.list, Optional = false };
                gooMollierObjectParam.DataMapping = GH_DataMapping.Flatten;
                result.Add(new GH_SAMParam(gooMollierObjectParam, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_String param_String = new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "_name_", NickName = "_name_", Description = "Group Name", Access = GH_ParamAccess.item, Optional = true };
                param_String.SetPersistentData("New Group");
                result.Add(new GH_SAMParam(param_String, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {

                List<GH_SAMParam> result = new List<GH_SAMParam>();

                result.Add(new GH_SAMParam(new GooMollierGroupParam() { Name = "mollierGroup", NickName = "mollierGroup", Description = "Mollier Group", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateMollierGroup()
          : base("SAMMollier.CreateMollierGroup", "SAMMollier.CreateMollierGroup",
              "Create Mollier Group \n *Merges all object into one Mollier Group",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            List<IMollierObject> mollierObjects = new List<IMollierObject>();
            index = Params.IndexOfInputParam("_mollierObjects");
            if (index == -1 || !dataAccess.GetDataList(index, mollierObjects))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            string name = "New Group";
            index = Params.IndexOfInputParam("_name_");
            if (index == -1 || !dataAccess.GetData(index, ref name))
            {
                name = "New Group";
            }

            MollierGroup mollierGroup = new MollierGroup(name);
            foreach(IMollierObject mollierObject in mollierObjects)
            {
                IMollierGroupable mollierGroupable = (mollierObject as IMollierGroupable)?.Clone();
                if(mollierGroupable == null)
                {
                    continue;
                }

                mollierGroup.Add(mollierGroupable);
            }


            index = Params.IndexOfOutputParam("mollierGroup");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierGroup(mollierGroup));
            }
        }

    }
}