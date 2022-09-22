using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierCreateAdiabaticHumidificationProcessByRelativeHumidity : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("4aaf217e-1bdb-4383-ac66-c199adc5dbfa");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.5";

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
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "_start", NickName = "_start", Description = "Start Mollier Point for MollierProcess", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_relativeHumidity", NickName = "_relativeHumidity", Description = "Relative Humidity RH[0-100]%", Access = GH_ParamAccess.item}, ParamVisibility.Binding));
                global::Grasshopper.Kernel.Parameters.Param_Colour param_Colour = null;
                param_Colour = new global::Grasshopper.Kernel.Parameters.Param_Colour() { Name = "_color_", NickName = "_color_", Description = "Colour RGB", Access = GH_ParamAccess.item, Optional = true };
                result.Add(new GH_SAMParam(param_Colour, ParamVisibility.Voluntary));
                global::Grasshopper.Kernel.Parameters.Param_String param_Label = null;
                param_Label = new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "startLabel_", NickName = "startLabel_", Description = "Start Label", Access = GH_ParamAccess.item, Optional = true };
                result.Add(new GH_SAMParam(param_Label, ParamVisibility.Voluntary));

                param_Label = new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "processLabel_", NickName = "processLabel_", Description = "Process Label", Access = GH_ParamAccess.item, Optional = true };
                result.Add(new GH_SAMParam(param_Label, ParamVisibility.Voluntary));

                param_Label = new global::Grasshopper.Kernel.Parameters.Param_String() { Name = "endLabel_", NickName = "endLabel_", Description = "End Label", Access = GH_ParamAccess.item, Optional = true };
                result.Add(new GH_SAMParam(param_Label, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "adiabaticHumidificationProcess", NickName = "adiabaticHumidificationProcess", Description = "AdiabaticHumidificationProcess", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "end", NickName = "end", Description = "End", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Colour() { Name = "color", NickName = "color", Description = "Color", Access = GH_ParamAccess.item }, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierCreateAdiabaticHumidificationProcessByRelativeHumidity()
          : base("SAMMollier.CreateAdiabaticHumidificationProcessByRelativeHumidity", "SAMMollier.CreateAdiabaticHumidificationProcessByRelativeHumidity",
              "Creates AdiabaticHumidificationProcess\nHumidification can generally divided into three physical methods: Vaporization, atomization and evaporation. Vaporization is an isothermal process while atomization and evaporation are adiabatic processes.\nIn adiabatic humidification, water is provided to air in liquid form and must therefore still achieve a gaseous state. Energy is required for this purpose, and is drawn from the surrounding air in the form of heat. Since a decrease in temperature also takes place in this case, the process is also called an adiabatic cooling effect.",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_start");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            MollierPoint start = null;
            if (!dataAccess.GetData(index, ref start) || start == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_relativeHumidity");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double relativeHumidity = double.NaN;
            if (!dataAccess.GetData(index, ref relativeHumidity) || double.IsNaN(relativeHumidity))
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

            string startLabel = null;
            index = Params.IndexOfInputParam("startLabel_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref startLabel);
            }
            string processLabel = null;
            index = Params.IndexOfInputParam("processLabel_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref processLabel);
            }
            string endLabel = null;
            index = Params.IndexOfInputParam("endLabel_");
            if (index != -1)
            {
                dataAccess.GetData(index, ref endLabel);
            }

            if (relativeHumidity > 100)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Your target RH is above 100%, range for Relative Humidity is 0-100 %");
            }

            AdiabaticHumidificationProcess adiabaticHumidificationProcess = Core.Mollier.Create.AdiabaticHumidificationProcess_ByRelativeHumidity(start, relativeHumidity);

            if (relativeHumidity < start.RelativeHumidity)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Your target RH is below starting, Please increaste RH to be higher than start point RH");
            }

            index = Params.IndexOfOutputParam("adiabaticHumidificationProcess");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierProcess(adiabaticHumidificationProcess, color, startLabel, processLabel, endLabel));
            }
            else
            {
                return;
            }
            
            if(adiabaticHumidificationProcess != null)
            {
                MollierPoint end = new MollierPoint(adiabaticHumidificationProcess.End);
                index = Params.IndexOfOutputParam("end");
                if (index != -1)
                {
                    dataAccess.SetData(index, new GooMollierPoint(end));
                }
            }

            index = Params.IndexOfOutputParam("color");
            if (index != -1)
            {
                dataAccess.SetData(index, color);
            }
        }
    }
}