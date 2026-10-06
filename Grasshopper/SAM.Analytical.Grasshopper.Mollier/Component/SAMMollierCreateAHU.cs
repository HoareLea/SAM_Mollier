// SPDX-License-Identifier: LGPL-3.0-or-later
// Copyright (c) 2020-2026 Michal Dengusiak & Jakub Ziolkowski and contributors

using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Grasshopper;
using SAM.Core.Grasshopper.Mollier;
using SAM.Core.Mollier;
using SAM.Geometry.Grasshopper.Mollier;

namespace SAM.Analytical.Grasshopper.Mollier
{
    public class SAMMollierCreateAHU : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("901de5c5-7c1d-4cc1-acf0-2ad609bdce24");

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
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "supplyMollierProcesses_", NickName = "supplyMollierProcesses_", Description = "Ordered supply-side chain of Mollier processes; each is mapped to the matching equipment and added to the AHU in order.", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "extractMollierProcesses_", NickName = "extractMollierProcesses_", Description = "Ordered extract-side chain of Mollier processes; each is mapped to the matching equipment on the extract path.", Access = GH_ParamAccess.list, Optional = true }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooAirHandlingUnitParam() { Name = "airHandlingUnit", NickName = "airHandlingUnit", Description = "The assembled SAM.Analytical AirHandlingUnit.", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateAHU()
          : base("SAMMollier.CreateAHU", "SAMMollier.CreateAHU",
              "Assembles an AirHandlingUnit from chains of Mollier processes.\n" +
              "\n" +
              "Each supply (and optional extract) process is mapped to the matching equipment and added to the\n" +
              "AHU in order. This builds the older SAM.Analytical AirHandlingUnit model; for a connected,\n" +
              "simulatable SAM_Systems energy centre use SAMSystems.CreateEnergyCentreByMollier instead.",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            List<IMollierProcess> mollierProcesses_Supply = new List<IMollierProcess>();
            index = Params.IndexOfInputParam("supplyMollierProcesses_");
            if (index != -1)
            {
                dataAccess.GetDataList(index, mollierProcesses_Supply);
            }

            List<IMollierProcess> mollierProcesses_Extract = new List<IMollierProcess>();
            index = Params.IndexOfInputParam("extractMollierProcesses_");
            if (index != -1)
            {
                dataAccess.GetDataList(index, mollierProcesses_Extract);
            }

            AirHandlingUnit airHandlingUnit = Analytical.Mollier.Create.AirHandlingUnit(mollierProcesses_Supply, mollierProcesses_Extract);

            index = Params.IndexOfOutputParam("airHandlingUnit");
            if (index != -1)
            {
                dataAccess.SetData(index, airHandlingUnit);
            }
        }
    }
}