using Grasshopper.Kernel;
using SAM.Analytical.Grasshopper.Mollier.Properties;
using SAM.Core.Grasshopper;
using SAM.Weather;
using SAM.Weather.Grasshopper;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.Grasshopper.Mollier
{
    public class SAMWeatherMollierPoints : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("b80fc9e8-f940-4c53-b0dc-05127cf5941d");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.4";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Initializes a new instance of the SAM_point3D class.
        /// </summary>
        public SAMWeatherMollierPoints()
          : base("SAMWeather.MollierPoints", "SAMWeather.MollierPoints",
              "Gets WeatherMollierPoints from WeatherData",
              "SAM", "Mollier")
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
                GooWeatherObjectParam gooWeatherObjectParam = new GooWeatherObjectParam() { Name = "_weatherObject", NickName = "_weatherObject", Description = "SAM Weather IWeatherObject such as WeatherData, WeatherYear, WeatherDay and WeatherHour", Access = GH_ParamAccess.item };
                result.Add(new GH_SAMParam(gooWeatherObjectParam, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Number number = null;
                number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "pressure_", NickName = "pressure_", Description = "Pressure", Optional = true, Access = GH_ParamAccess.item };
                result.Add(new GH_SAMParam(number, ParamVisibility.Voluntary));

                global::Grasshopper.Kernel.Parameters.Param_Boolean @boolean = null;
                @boolean = new global::Grasshopper.Kernel.Parameters.Param_Boolean() { Name = "_run", NickName = "_run", Description = "Connect a boolean toggle to run.", Access = GH_ParamAccess.item };
                @boolean.SetPersistentData(false);
                result.Add(new GH_SAMParam(@boolean, ParamVisibility.Binding));

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
                result.Add(new GH_SAMParam(new Geometry.Grasshopper.Mollier.GooMollierPointParam() { Name = "mollierPoints", NickName = "mollierPoints", Description = "SAM Core MollierPoints", Access = GH_ParamAccess.list }, ParamVisibility.Binding));
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
            int index_successful = Params.IndexOfOutputParam("successful");
            if (index_successful != -1)
            {
                dataAccess.SetData(index_successful, false);
            }

            int index;

            bool run = false;
            index = Params.IndexOfInputParam("_run");
            if (index == -1 || !dataAccess.GetData(index, ref run))
                run = false;

            if (!run)
                return;

            IWeatherObject weatherObject = null;
            index = Params.IndexOfInputParam("_weatherObject");
            if (index == -1 || !dataAccess.GetData(index, ref weatherObject) || weatherObject == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double pressure = double.NaN;
            index = Params.IndexOfInputParam("pressure_");
            if(index != -1)
            {
                if(!dataAccess.GetData(index, ref pressure))
                {
                    pressure = double.NaN;
                }
            }

            List<Core.Mollier.MollierPoint> mollierPoints = null;
            if (weatherObject is WeatherData)
            {
                mollierPoints = Weather.Mollier.Query.WeatherMollierPoints((WeatherData)weatherObject, pressure)?.ConvertAll(x => x as Core.Mollier.MollierPoint);
            }
            else if (weatherObject is WeatherYear)
            {
                mollierPoints = Weather.Mollier.Query.WeatherMollierPoints((WeatherYear)weatherObject, pressure)?.ConvertAll(x => x as Core.Mollier.MollierPoint);
            }
            else if (weatherObject is WeatherDay)
            {
                mollierPoints = Weather.Mollier.Query.MollierPoints((WeatherDay)weatherObject, pressure);
            }
            else if (weatherObject is WeatherHour)
            {
                mollierPoints = new List<Core.Mollier.MollierPoint>() { Weather.Mollier.Query.MollierPoint((WeatherHour)weatherObject, pressure) };
            }

            index = Params.IndexOfOutputParam("mollierPoints");
            if (index != -1)
            {
                List<Geometry.Grasshopper.Mollier.GooMollierPoint> gooMollierPoints = mollierPoints?.ConvertAll(x => new Geometry.Grasshopper.Mollier.GooMollierPoint(x));

                dataAccess.SetDataList(index, gooMollierPoints);
            }
        }
    }
}