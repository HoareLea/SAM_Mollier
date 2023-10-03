using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Grasshopper;
using SAM.Core.Grasshopper.Mollier;
using SAM.Core.Mollier;

namespace SAM.Analytical.Grasshopper.Mollier
{
    public class SAMMollierCalculateLoads : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("8bf5f832-a140-4963-9bee-83df944f4f23");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.2";

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
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "outside_", NickName = "outside_", Description = "Outside condition as MollierPoint\n *used for infiltration calculation", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "inside_", NickName = "inside_", Description = "Room inside condition as MollierPoint\n *used for infiltration calculation", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooSpaceParam() { Name = "_space", NickName = "_space", Description = "SAM Analytical Space", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "supplyAirFlow", NickName = "supplyAirFlow", Description = "Supply Air Flow [m3/s]", Access = GH_ParamAccess.item}, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "exhaustAirFlow", NickName = "exhaustAirFlow", Description = "Exhaust Air Flow [m3/s]", Access = GH_ParamAccess.item}, ParamVisibility.Binding));

                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "sensibleLoad", NickName = "sensibleLoad", Description = "Sensible Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "equipmentSensibleLoad", NickName = "equipmentSensibleLoad", Description = "Equipment Sensible Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "occupancySensibleLoad", NickName = "occupancySensibleLoad", Description = "Occupancy Sensible Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "infiltrationSensibleLoad", NickName = "infiltrationSensibleLoad", Description = "Infiltration Sensible Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "lightingLoad", NickName = "lightingLoad", Description = "Lighting Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "latentLoad", NickName = "latentLoad", Description = "Latent Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "equipmentLatentLoad", NickName = "equipmentLatentLoad", Description = "Equipment Latent Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "occupancyLatentLoad", NickName = "occupancyLatentLoad", Description = "Occupancy Latent Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "infiltrationLatentLoad", NickName = "infiltrationLatentLoad", Description = "Infiltration Latent Load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));

                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "sensibleHeatRatio", NickName = "sensibleHeatRatio", Description = "Sensible Heat Ratio [-]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCalculateLoads()
          : base("SAMMollier.CalculateLoads", "SAMMollier.CalculateLoads",
              "Calculate space loads, including infiltration, by providing input values for inside and outside conditions",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_space");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            Space space = null;
            if (!dataAccess.GetData(index, ref space) || space == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            MollierPoint inside = null;
            index = Params.IndexOfInputParam("_inside");
            if (index == -1 || !dataAccess.GetData(index, ref inside) || inside == null)
            {
                inside = null;
            }

            MollierPoint outside = null;
            index = Params.IndexOfInputParam("_outside");
            if (index == -1 || !dataAccess.GetData(index, ref outside) || outside == null)
            {
                outside = null;
            }

            double infiltrationLatentGain = 0;
            double infiltrationSensibleGain = 0;

            if(inside != null && outside != null)
            {
                Analytical.Mollier.Query.TryGetInfiltrationGains(space, outside, inside, out infiltrationLatentGain, out infiltrationSensibleGain);
            }

            double equipmentLatentGain = Analytical.Query.CalculatedEquipmentLatentGain(space);
            double occupancyLatentGain = Analytical.Query.OccupancyLatentGain(space);

            double latentGain = infiltrationLatentGain + equipmentLatentGain + occupancyLatentGain;

            double equipmentSensibleGain = Analytical.Query.CalculatedEquipmentSensibleGain(space);
            double lightingGain = Analytical.Query.CalculatedLightingGain(space);
            double occupancySensibleGain = Analytical.Query.OccupancySensibleGain(space);

            double sensibleGain = infiltrationSensibleGain + equipmentSensibleGain + lightingGain + occupancySensibleGain;

            double supplyAirFlow = Analytical.Query.CalculatedSupplyAirFlow(space);
            double exhaustAirFlow = Analytical.Query.CalculatedExhaustAirFlow(space);

            index = Params.IndexOfOutputParam("supplyAirFlow");
            if (index != -1)
            {
                dataAccess.SetData(index, supplyAirFlow);
            }

            index = Params.IndexOfOutputParam("exhaustAirFlow");
            if (index != -1)
            {
                dataAccess.SetData(index, exhaustAirFlow);
            }

            index = Params.IndexOfOutputParam("latentLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, latentGain);
            }

            index = Params.IndexOfOutputParam("sensibleLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, sensibleGain);
            }

            index = Params.IndexOfOutputParam("equipmentLatentLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, equipmentLatentGain);
            }

            index = Params.IndexOfOutputParam("occupancyLatentLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, occupancyLatentGain);
            }

            index = Params.IndexOfOutputParam("infiltrationLatentLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, infiltrationLatentGain);
            }

            index = Params.IndexOfOutputParam("equipmentSensibleLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, equipmentSensibleGain);
            }

            index = Params.IndexOfOutputParam("occupancySensibleLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, occupancySensibleGain);
            }

            index = Params.IndexOfOutputParam("infiltrationSensibleLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, infiltrationSensibleGain);
            }

            index = Params.IndexOfOutputParam("lightingLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, lightingGain);
            }

            index = Params.IndexOfOutputParam("sensibleHeatRatio");
            if (index != -1)
            {
                dataAccess.SetData(index, Core.Mollier.Query.SensibleHeatRatio(sensibleGain, latentGain));
            }
        }
    }
}