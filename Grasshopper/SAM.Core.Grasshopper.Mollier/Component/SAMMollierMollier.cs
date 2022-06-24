using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Grasshopper;
using System;
using System.Collections.Generic;

namespace SAM.Analytical.Grasshopper
{
    public class SAMMollierMollier : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9617a5cf-dff2-4c64-8721-85452dc0e820");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

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
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_dryBulbTemperature", NickName = "_dryBulbTemperature", Description = "Dry bulb temperature [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_relativeHumidity_", NickName = "_relativeHumidity_", Description = "Relative humidity (0 - 100) [%] \n Connect only one humidity indication \n relativeHumidity or wetBulbTemperature or dewPointTemperature", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_humidityRatio_", NickName = "_humidityRatio_", Description = "Humidty Ratio [kg/kg]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_wetBulbTemperature_", NickName = "_wetBulbTemperature_", Description = "Wet bulb temperature [°C] \n Connect only one humidity indication \n relativeHumidity or wetBulbTemperature or dewPointTemperature", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_dewPointTemperature_", NickName = "_dewPointTemperature_", Description = "Dew Point Temperature [°C] \n Connect only one humidity indication \n relativeHumidity or wetBulbTemperature or dewPointTemperature", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_pressure_", NickName = "_pressure_", Description = "Atmospheric pressure [Pa]", Access = GH_ParamAccess.item, Optional = true };
                param_Number.SetPersistentData(101325);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dryBulbTemperature", NickName = "dryBulbTemperature", Description = "Dry bulb temperature [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "relativeHumidity", NickName = "relativeHumidity", Description = "Relative humidity (0 - 100) [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "humidityRatio", NickName = "humidityRatio", Description = "Humidty Ratio [kg/kg]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "wetBulbTemperature", NickName = "wetBulbTemperature", Description = "Wet bulb temperature [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dewPointTemperature", NickName = "dewPointTemperature", Description = "Dew Point Temperature [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "saturationVapourPressure", NickName = "saturationVapourPressure", Description = "Saturation Vapour Pressure [Pa]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "enthalpy", NickName = "enthalpy", Description = "Enthalpy [J/kg]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "specificVolume", NickName = "specificVolume", Description = "Specific Volume [m³/kg]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "density", NickName = "density", Description = "Density [kg/m3]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "degreeSaturation", NickName = "degreeSaturation", Description = "Degree of saturation [unitless]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "pressure", NickName = "pressure", Description = "Atmospheric pressure [Pa]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierMollier()
          : base("SAMMollier.Mollier", "SAMMollier.Mollier",
              "Utility function to calculate relative humidity, humidity ratio, wet - bulb temperature, dew - point temperature, \nvapour pressure, moist air enthalpy, moist air volume, and degree of saturation of air given \ndry-bulb temperature, (relative humidity or humidity ratio or wet bulb temperature) and pressure. \n*The degree of saturation (i.e humidity ratio of the air / humidity ratio of the air at saturationat the same temperature and pressure)\n Connect only one humidity indication",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_dryBulbTemperature");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            double dryBulbTemperature = double.NaN;
            double relativeHumidity = double.NaN;
            double humidityRatio = double.NaN;
            double wetBulbTemperature = double.NaN;
            double dewPointTemperature = double.NaN;
            double saturationVapourPressure = double.NaN;
            double enthalpy = double.NaN;
            double specificVolume = double.NaN;
            double degreeSaturation = double.NaN;
            double pressure = double.NaN;
            double density = double.NaN;

            index = Params.IndexOfInputParam("_dryBulbTemperature");
            if (!dataAccess.GetData(index, ref dryBulbTemperature) || double.IsNaN(dryBulbTemperature))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_relativeHumidity_");
            if (index == -1 || !dataAccess.GetData(index, ref relativeHumidity) || double.IsNaN(relativeHumidity))
            {
                relativeHumidity = double.NaN;
            }

            if (!double.IsNaN(relativeHumidity))
            {
                if (relativeHumidity < 0 || relativeHumidity > 100)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
            }

            index = Params.IndexOfInputParam("_humidityRatio_");
            if (index == -1 || !dataAccess.GetData(index, ref humidityRatio) || double.IsNaN(humidityRatio))
            {
                relativeHumidity = double.NaN;
            }

            if (!double.IsNaN(humidityRatio) && !double.IsNaN(relativeHumidity))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_wetBulbTemperature_");
            if (index == -1 || !dataAccess.GetData(index, ref wetBulbTemperature) || double.IsNaN(wetBulbTemperature))
            {
                wetBulbTemperature = double.NaN;
            }

            index = Params.IndexOfInputParam("_dewPointTemperature_");
            if (index == -1 || !dataAccess.GetData(index, ref dewPointTemperature) || double.IsNaN(dewPointTemperature))
            {
                dewPointTemperature = double.NaN;
            }

            if (double.IsNaN(relativeHumidity) && double.IsNaN(wetBulbTemperature) && double.IsNaN(dewPointTemperature) && double.IsNaN(humidityRatio))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            index = Params.IndexOfInputParam("_pressure_");
            if (index == -1 || !dataAccess.GetData(index, ref pressure) || double.IsNaN(pressure))
            {
                pressure = 101325;
            }

            if (!double.IsNaN(relativeHumidity) && double.IsNaN(wetBulbTemperature) && double.IsNaN(dewPointTemperature))
            {
                humidityRatio = Core.Mollier.Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
            }
            else if (double.IsNaN(relativeHumidity) && !double.IsNaN(wetBulbTemperature) && double.IsNaN(dewPointTemperature))
            {
                humidityRatio = Core.Mollier.Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
            }
            else if (double.IsNaN(relativeHumidity) && double.IsNaN(wetBulbTemperature) && !double.IsNaN(dewPointTemperature))
            {
                humidityRatio = Core.Mollier.Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
            }
            else if (!double.IsNaN(humidityRatio) && double.IsNaN(wetBulbTemperature) && double.IsNaN(dewPointTemperature))
            {
                relativeHumidity = Core.Mollier.Query.RelativeHumidity(dryBulbTemperature, humidityRatio, pressure);
            }
            else if (double.IsNaN(humidityRatio) && !double.IsNaN(wetBulbTemperature) && double.IsNaN(dewPointTemperature))
            {
                relativeHumidity = Core.Mollier.Query.RelativeHumidity(dryBulbTemperature, humidityRatio, pressure);
            }
            else if (double.IsNaN(humidityRatio) && double.IsNaN(wetBulbTemperature) && !double.IsNaN(dewPointTemperature))
            {
                relativeHumidity = Core.Mollier.Query.RelativeHumidity(dryBulbTemperature, humidityRatio, pressure);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            wetBulbTemperature = Core.Mollier.Query.WetBulbTemperature(dryBulbTemperature, relativeHumidity, pressure);
            density = Core.Mollier.Query.Density(dryBulbTemperature, relativeHumidity, pressure);
            specificVolume = Core.Mollier.Query.SpecificVolume(dryBulbTemperature, humidityRatio, pressure);
            saturationVapourPressure = Core.Mollier.Query.SaturationVapourPressure(dryBulbTemperature, relativeHumidity);
            enthalpy = Core.Mollier.Query.Enthalpy(dryBulbTemperature, humidityRatio);

            index = Params.IndexOfOutputParam("dryBulbTemperature");
            if (index != -1)
            {
                dataAccess.SetData(index, dryBulbTemperature);
            }

            index = Params.IndexOfOutputParam("relativeHumidity");
            if (index != -1)
            {
                dataAccess.SetData(index, relativeHumidity);
            }

            index = Params.IndexOfOutputParam("humidityRatio");
            if (index != -1)
            {
                dataAccess.SetData(index, humidityRatio);
            }

            index = Params.IndexOfOutputParam("wetBulbTemperature");
            if (index != -1)
            {
                dataAccess.SetData(index, wetBulbTemperature);
            }

            index = Params.IndexOfOutputParam("dewPointTemperature");
            if (index != -1)
            {
                dataAccess.SetData(index, dewPointTemperature);
            }

            index = Params.IndexOfOutputParam("saturationVapourPressure");
            if (index != -1)
            {
                dataAccess.SetData(index, saturationVapourPressure);
            }

            index = Params.IndexOfOutputParam("enthalpy");
            if (index != -1)
            {
                dataAccess.SetData(index, enthalpy);
            }

            index = Params.IndexOfOutputParam("specificVolume");
            if (index != -1)
            {
                dataAccess.SetData(index, specificVolume);
            }

            index = Params.IndexOfOutputParam("density");
            if (index != -1)
            {
                dataAccess.SetData(index, density);
            }

            index = Params.IndexOfOutputParam("degreeSaturation");
            if (index != -1)
            {
                dataAccess.SetData(index, degreeSaturation);
            }

            index = Params.IndexOfOutputParam("pressure");
            if (index != -1)
            {
                dataAccess.SetData(index, pressure);
            }
        }
    }
}