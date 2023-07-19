using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierPsychrometrics : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("9617a5cf-dff2-4c64-8721-85452dc0e820");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.12";

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
                result.Add(new GH_SAMParam(new GooMollierPointParam() { Name = "mollierPoint_", NickName = "mollierPoint_", Description = "MollierPoint", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dryBulbTemperature", NickName = "dryBulbTemperature", Description = "Dry bulb temperature t [°C]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "enthalpy", NickName = "enthalpy", Description = "Enthalpy h [kJ/kg]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "relativeHumidity", NickName = "relativeHumidity", Description = "Relative humidity (0 - 100) φ [%] \n Connect only one humidity indication \n relativeHumidity or wetBulbTemperature or dewPointTemperature", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "humidityRatio", NickName = "humidityRatio", Description = "Humidty Ratio x [g/kg]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "wetBulbTemperature", NickName = "wetBulbTemperature", Description = "Wet bulb temperature t_wb [°C] \n Connect only one humidity indication \n relativeHumidity or wetBulbTemperature or dewPointTemperature", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dewPointTemperature_", NickName = "dewPointTemperature", Description = "Dew Point Temperature tτ[°C] \n Connect only one humidity indication \n relativeHumidity or wetBulbTemperature or dewPointTemperature", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));

                global::Grasshopper.Kernel.Parameters.Param_Number param_Number = null;

                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_pressure_", NickName = "_pressure_", Description = "Atmospheric pressure [Pa]", Access = GH_ParamAccess.item, Optional = true };
                //param_Number.SetPersistentData(Standard.Pressure);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Binding));

                param_Number = new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_elevation_", NickName = "_elevation_", Description = "Elevation [m]", Access = GH_ParamAccess.item, Optional = true };
                //param_Number.SetPersistentData(double.NaN);
                result.Add(new GH_SAMParam(param_Number, ParamVisibility.Voluntary));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierPointParam { Name = "mollierPoint", NickName = "mollierPoint", Description = "SAM Core Mollier MollierPoint", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dryBulbTemperature", NickName = "dryBulbTemperature", Description = "Dry bulb temperature t [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "relativeHumidity", NickName = "relativeHumidity", Description = "Relative humidity (0 - 100) φ [%]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "humidityRatio", NickName = "humidityRatio", Description = "Humidty Ratio x [g/kg]\nWassergehalt x [g/kg]\nWilgotność bezwzględna x [g/kg]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "saturationHumidityRatio", NickName = "saturationHumidityRatio", Description = "Saturation Humidity Ratio xs [g/kg] \nWassergehalt im Sättigungszustand xs [g/kg]\n\n*Humidity ratio at saturation line φ=100%", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "saturationTemperature", NickName = "saturationTemperature", Description = "Saturation temperature ts [°C]\nSättigungstemperatur ts [°C]\nTemperatura nasycenia ts [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "wetBulbTemperature", NickName = "wetBulbTemperature", Description = "Wet bulb temperature t_wb [°C] \nFeuchtkugeltemperatur tf [°C]\nTemperatura termometru mokrego t_wb[°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dewPointTemperature", NickName = "dewPointTemperature", Description = "Dew  Temperature tτ [°C] \nTaupunkttemperatur tτ [°C]\nTemperatura punktu rosy tτ [°C]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "saturationVapourPressure", NickName = "saturationVapourPressure", Description = "Saturation Vapour Pressure  pS [Pa] \nSättigungsdruck pS [Pa]\nCiśnienie nasycenia pS [Pa]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "partialVapourPressure", NickName = "partialVapourPressure", Description = "Partial Vapour Pressure pW [Pa] \nPartialdruck des Wasserdampfes (absolut) pW [Pa]\nCiśnienie cząstkowe (udziałowe) pary wodnej pW [Pa]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "partialDryAirPressure", NickName = "partialDryAirPressure", Description = "Partial Dry Air Pressure pL [Pa]\nPartialdruck der trockenen Luft (absolut) pL [Pa]\nCiśnienie cząstkowe (udziałowe) powietrza suchego pL [Pa]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "enthalpy", NickName = "enthalpy", Description = "Enthalpy h [kJ/kg dry air]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "specificVolume", NickName = "specificVolume", Description = "Specific Volume v [m³/kg dry air]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "density", NickName = "density", Description = "Density ρ [kg moist air/m³]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "degreeSaturation", NickName = "degreeSaturation", Description = "Degree of saturation [unitless]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "pressure", NickName = "pressure", Description = "Atmospheric pressure p [Pa]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "waterHeatCapacity", NickName = "waterHeatCapacity", Description = "Heat Capacity of Water cpw [kJ/kgK]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "airHeatCapacity", NickName = "airHeatCapacity", Description = "Heat Capacity of Air cp [kJ/kg dir air K]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "thermalConductivity", NickName = "thermalConductivity", Description = "Thermal Conductivity λ [W/(mK)]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "dynamicViscosity", NickName = "dynamicViscosity", Description = "Dynamic Viscosity η[Pa s]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "kinematicViscosity", NickName = "kinematicViscosity", Description = "Kinematic Viscosity v [m²/s]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "temperatureConductivity", NickName = "temperatureConductivity", Description = "Temperature Conductivity a [m²/s]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "prandtlNumber", NickName = "prandtlNumber", Description = "Prandtl Number Pr [-]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "vapourDensity", NickName = "vapourDensity", Description = "Vapour Density [kg/m³]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "specificHeat", NickName = "specificHeat", Description = "Specific Heat [kJ/kgK]\n*Moist Air if x>0\n Calculated from (cpL+x * cpW)/(1 + x) ", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "specificHeat_DryAir", NickName = "specificHeat_DryAir", Description = "Dry Air Specific Heat cpL [kJ/kgK]", Access = GH_ParamAccess.item, Optional = true }, ParamVisibility.Voluntary));
                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierPsychrometrics()
          : base("SAMMollier.Psychrometrics", "SAMMollier.Psychrometrics",
              "Utility function to calculate relative humidity, humidity ratio, wet - bulb temperature, dew - point temperature, \nvapour pressure, moist air enthalpy, moist air volume, and degree of saturation of air given \ndry-bulb temperature, (relative humidity or humidity ratio or wet bulb temperature) and pressure. \n*The degree of saturation (i.e humidity ratio of the air / humidity ratio of the air at saturationat the same temperature and pressure)\n Connect only one humidity indication",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            bool maximum_connected_nodes = false;//true wheter there is connected mollierpoint or dew point or 2 of other, otherwise false

            double dryBulbTemperature = double.NaN;
            double relativeHumidity = double.NaN;
            double humidityRatio = double.NaN;
            double wetBulbTemperature = double.NaN;
            double saturationVapourPressure = double.NaN;
            double saturationHumidityRatio = double.NaN;
            double partialVapourPressure = double.NaN;
            double enthalpy = double.NaN;
            double specificVolume = double.NaN;
            double degreeSaturation = double.NaN;
            double density = double.NaN;
            double partialDryAirPressure = double.NaN;
            double waterHeatCapacity = double.NaN;
            double airHeatCapacity = double.NaN;
            double thermalConductivity = double.NaN;
            double dynamicViscosity = double.NaN;
            double kinematicViscosity = double.NaN;
            double temperatureConductivity = double.NaN;
            double prandtlNumber = double.NaN;
            IMollierPoint mollierPoint = null;
            double dewPointTemperature = double.NaN;
            double vapourDensity = double.NaN;
            double specificHeat = double.NaN;
            double specificHeat_DryAir = double.NaN;
            double saturationTemperature = double.NaN;

            //mollierpoint check
            index = Params.IndexOfInputParam("mollierPoint_");
            if(index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            if (dataAccess.GetData(index, ref mollierPoint) && mollierPoint != null)
            {
                maximum_connected_nodes = true;
            }

            MollierPoint mollierPoint_Temp = mollierPoint is UIMollierPoint ? ((UIMollierPoint)mollierPoint).MollierPoint : mollierPoint as MollierPoint;

            double pressure = Standard.Pressure;
            if (mollierPoint != null)
            {
                pressure = mollierPoint.Pressure;
            }

            index = Params.IndexOfInputParam("_pressure_");
            if (index != -1)
            {
                double pressure_Temp = double.NaN;
                if(dataAccess.GetData(index, ref pressure_Temp) && !double.IsNaN(pressure_Temp))
                {
                    pressure = pressure_Temp;
                }
            }

            index = Params.IndexOfInputParam("_elevation_");
            if (index != -1)
            {
                double elevation = double.NaN;
                if (dataAccess.GetData(index, ref elevation) && !double.IsNaN(elevation))
                {
                    pressure = Core.Mollier.Query.Pressure(elevation);
                }
            }

            //dewpoint check
            index = Params.IndexOfInputParam("dewPointTemperature_");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            if (dataAccess.GetData(index, ref dewPointTemperature) && !double.IsNaN(dewPointTemperature))
            {
                if (maximum_connected_nodes)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
                maximum_connected_nodes = true;
            }

            int numberOfConnected = maximum_connected_nodes ? 2 : 0;

            //dry bulb temperature check
            index = Params.IndexOfInputParam("dryBulbTemperature");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            if (dataAccess.GetData(index, ref dryBulbTemperature) && !double.IsNaN(dryBulbTemperature))
            {
                if(numberOfConnected == 2)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
                numberOfConnected++;
            }
            //relative humidity check
            index = Params.IndexOfInputParam("relativeHumidity");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            if (dataAccess.GetData(index, ref relativeHumidity) && !double.IsNaN(relativeHumidity))
            {
                if (numberOfConnected == 2 || relativeHumidity > 100 || relativeHumidity < 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
                numberOfConnected++;
            }
            //humidity ratio check
            index = Params.IndexOfInputParam("humidityRatio");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            if (dataAccess.GetData(index, ref humidityRatio) && !double.IsNaN(humidityRatio))
            {
                if (numberOfConnected == 2 || humidityRatio < 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
                //2023-06-07 Special case when we load in g/kg as whole engine in kg/kg
                humidityRatio = humidityRatio / 1000;
                numberOfConnected++;
            }
            //wet bulb temperature check
            index = Params.IndexOfInputParam("wetBulbTemperature");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            if (dataAccess.GetData(index, ref wetBulbTemperature) && !double.IsNaN(wetBulbTemperature))
            {
                if (numberOfConnected == 2)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                    return;
                }
                numberOfConnected++;
            }

            //enthalpy check
            index = Params.IndexOfInputParam("enthalpy");
            if (index != -1)
            {
                if (dataAccess.GetData(index, ref enthalpy) && !double.IsNaN(enthalpy))
                {
                    enthalpy *= 1000;
                    if (numberOfConnected == 2 || double.IsNaN(humidityRatio))
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                        return;
                    }
                    numberOfConnected++;
                }
            }


            //checking if all data are correct
            if (numberOfConnected != 2)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Invalid data, there should be connected mollier point or dew point or 2 of the other inputs");
                return;
            }
            //calculate other variables by input
            if(mollierPoint != null)
            {
                dryBulbTemperature = mollierPoint_Temp.DryBulbTemperature;
                relativeHumidity = mollierPoint_Temp.RelativeHumidity;
                humidityRatio = mollierPoint_Temp.HumidityRatio;
            }
            else if(!double.IsNaN(dewPointTemperature))
            {
                dryBulbTemperature = dewPointTemperature;
                relativeHumidity = 100;
                humidityRatio = Core.Mollier.Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
            }
            else if(!double.IsNaN(dryBulbTemperature) && !double.IsNaN(humidityRatio))
            {
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                relativeHumidity = mollierPoint_Temp.RelativeHumidity;
            }
            else if(!double.IsNaN(dryBulbTemperature) && !double.IsNaN(relativeHumidity))
            {
                humidityRatio = Core.Mollier.Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
            }
            else if(!double.IsNaN(dryBulbTemperature) && !double.IsNaN(wetBulbTemperature))
            {
                humidityRatio = Core.Mollier.Query.HumidityRatio_ByWetBulbTemperature(dryBulbTemperature, wetBulbTemperature, pressure);
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                relativeHumidity = mollierPoint_Temp.RelativeHumidity;
            }
            else if (!double.IsNaN(humidityRatio) && !double.IsNaN(relativeHumidity))
            {
                dryBulbTemperature = Core.Mollier.Query.DryBulbTemperature_ByHumidityRatio(humidityRatio, relativeHumidity, pressure);
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
            }
            else if(!double.IsNaN(humidityRatio) && !double.IsNaN(wetBulbTemperature))
            {
                dryBulbTemperature = Core.Mollier.Query.DryBulbTemperature_ByWetBulbTemperatureAndHumidityRatio(wetBulbTemperature, humidityRatio, pressure);
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                relativeHumidity = mollierPoint_Temp.RelativeHumidity;
            }
            else if(!double.IsNaN(relativeHumidity) && !double.IsNaN(wetBulbTemperature))
            {
                dryBulbTemperature = Core.Mollier.Query.DryBulbTemperature_ByWetBulbTemperature(wetBulbTemperature, relativeHumidity, pressure);
                humidityRatio = Core.Mollier.Query.HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
            }
            else if (!double.IsNaN(enthalpy) && !double.IsNaN(humidityRatio))
            {
                dryBulbTemperature = Core.Mollier.Query.DryBulbTemperature(enthalpy, humidityRatio, pressure);
                mollierPoint_Temp = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                relativeHumidity = mollierPoint_Temp.RelativeHumidity;
            }

            wetBulbTemperature = mollierPoint_Temp.WetBulbTemperature();
            dewPointTemperature = mollierPoint_Temp.DewPointTemperature();
            degreeSaturation = mollierPoint_Temp.SaturationDegree();
            saturationHumidityRatio = mollierPoint_Temp.SaturationHumidityRatio();
            saturationTemperature = mollierPoint_Temp.SaturationTemperature();
            vapourDensity = mollierPoint_Temp.VapourDensity();
            specificHeat = mollierPoint_Temp.SpecificHeat();

            density = Core.Mollier.Query.Density_ByHumidityRatio(dryBulbTemperature, humidityRatio, pressure);
            specificVolume = Core.Mollier.Query.SpecificVolume(dryBulbTemperature, humidityRatio, pressure);
            saturationVapourPressure = Core.Mollier.Query.SaturationVapourPressure(dryBulbTemperature);
            partialVapourPressure = Core.Mollier.Query.PartialVapourPressure_ByHumidityRatio(humidityRatio, dryBulbTemperature, pressure);
            enthalpy = Core.Mollier.Query.Enthalpy(dryBulbTemperature, humidityRatio, pressure); 
            partialDryAirPressure = Core.Mollier.Query.PartialDryAirPressure(pressure, partialVapourPressure);
            airHeatCapacity = Core.Mollier.Query.SpecificHeatCapacity_Air(dryBulbTemperature, humidityRatio);
            waterHeatCapacity = Core.Mollier.Query.SpecificHeatCapacity_Water(dryBulbTemperature);
            thermalConductivity = Core.Mollier.Query.ThermalConductivity(dryBulbTemperature, humidityRatio);
            dynamicViscosity = Core.Mollier.Query.DynamicViscosity(dryBulbTemperature, humidityRatio);
            kinematicViscosity = Core.Mollier.Query.KinematicViscosity(dryBulbTemperature, humidityRatio, pressure);
            temperatureConductivity = Core.Mollier.Query.TemperatureConductivity(dryBulbTemperature, humidityRatio, pressure);
            prandtlNumber = Core.Mollier.Query.PrandtlNumber(dryBulbTemperature, humidityRatio, pressure);
            specificHeat_DryAir = Core.Mollier.Query.SpecificHeat_Air(dryBulbTemperature);

            relativeHumidity = Core.Query.Round(relativeHumidity, 0.1);


            index = Params.IndexOfOutputParam("mollierPoint");
            if (index != -1)
            {
                dataAccess.SetData(index, new GooMollierPoint(mollierPoint_Temp));
            }

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
                dataAccess.SetData(index, humidityRatio * 1000); //Change to g/kg
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

            index = Params.IndexOfOutputParam("saturationHumidityRatio");
            if (index != -1)
            {
                dataAccess.SetData(index, saturationHumidityRatio * 1000); //Change to g/kg
            }

            index = Params.IndexOfOutputParam("saturationTemperature");
            if (index != -1)
            {
                dataAccess.SetData(index, saturationTemperature);
            }

            index = Params.IndexOfOutputParam("partialVapourPressure");
            if (index != -1)
            {
                dataAccess.SetData(index, partialVapourPressure);
            }

            index = Params.IndexOfOutputParam("partialDryAirPressure");
            if (index != -1)
            {
                dataAccess.SetData(index, partialDryAirPressure);
            }

            index = Params.IndexOfOutputParam("enthalpy");
            if (index != -1)
            {
                dataAccess.SetData(index, enthalpy/1000);//   change to kJ/kg
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

            index = Params.IndexOfOutputParam("airHeatCapacity");
            if (index != -1)
            {
                dataAccess.SetData(index, airHeatCapacity);
            }

            index = Params.IndexOfOutputParam("waterHeatCapacity");
            if (index != -1)
            {
                dataAccess.SetData(index, waterHeatCapacity);
            }

            index = Params.IndexOfOutputParam("thermalConductivity");
            if (index != -1)
            {
                dataAccess.SetData(index, thermalConductivity);
            }

            index = Params.IndexOfOutputParam("dynamicViscosity");
            if (index != -1)
            {
                dataAccess.SetData(index, dynamicViscosity);
            }

            index = Params.IndexOfOutputParam("kinematicViscosity");
            if (index != -1)
            {
                dataAccess.SetData(index, kinematicViscosity);
            }

            index = Params.IndexOfOutputParam("temperatureConductivity");
            if (index != -1)
            {
                dataAccess.SetData(index, temperatureConductivity);
            }

            index = Params.IndexOfOutputParam("prandtlNumber");
            if (index != -1)
            {
                dataAccess.SetData(index, prandtlNumber);
            }

            index = Params.IndexOfOutputParam("vapourDensity");
            if (index != -1)
            {
                dataAccess.SetData(index, vapourDensity);
            }

            index = Params.IndexOfOutputParam("specificHeat");
            if (index != -1)
            {
                dataAccess.SetData(index, specificHeat / 1000);
            }
            index = Params.IndexOfOutputParam("specificHeat_DryAir");
            if (index != -1)
            {
                dataAccess.SetData(index, specificHeat_DryAir / 1000);
            }
        }
    }
}