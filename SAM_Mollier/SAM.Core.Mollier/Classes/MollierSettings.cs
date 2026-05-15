using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class MollierSettings : IJSAMObject
    {
        public ChartType ChartType { get; set; } = ChartType.Mollier;
        public double Pressure { get; set; } = Standard.Pressure;


        //Dry Bulb Temperature and Humidity Ratio
        public MollierRange MollierRange { get; set; } = Create.MollierRange();
        public double HumidityRatio_Interval { get; set; } = Default.HumidityRatio_Interval;
        public double DryBulbTemperature_Interval { get; set; } = Default.DryBulbTemperature_Interval;

        //Density
        public Range<double> DensityRange { get; set; } = new Range<double>(Default.Density_Min, Default.Density_Max);
        public double Density_Interval { get; set; } = Default.Density_Interval;

        //Enthalpy
        public Range<double> EnthalpyRange { get; set; } = new Range<double>(Default.Enthalpy_Min, Default.Enthalpy_Max);
        public double Enthalpy_Interval { get; set; } = Default.Enthalpy_Interval;


        //Specific Volume
        public Range<double> SpecificVolumeRange { get; set; } = new Range<double>(Default.SpecificVolume_Min, Default.SpecificVolume_Max);
        public double SpecificVolume_Interval { get; set; } = Default.SpecificVolume_Interval;


        //Wet Bulb Temperature
        public Range<double> WetBulbTemperatureRange { get; set; } = new Range<double>(Default.WetBulbTemperature_Min, Default.WetBulbTemperature_Max);
        public double WetBulbTemperature_Interval { get; set; } = Default.WetBulbTemperature_Interval;

        public MollierSettings()
        {

        }

        public MollierSettings(MollierSettings mollierSettings)
        {
            if (mollierSettings != null)
            {
                ChartType = mollierSettings.ChartType;
                Pressure = mollierSettings.Pressure;

                MollierRange = mollierSettings.MollierRange == null ? null : new MollierRange(mollierSettings.MollierRange);
                HumidityRatio_Interval = mollierSettings.HumidityRatio_Interval;
                DryBulbTemperature_Interval = mollierSettings.DryBulbTemperature_Interval;

                DensityRange = mollierSettings.DensityRange == null ? null : new Range<double>(mollierSettings.DensityRange);
                Density_Interval = mollierSettings.Density_Interval;

                EnthalpyRange = mollierSettings.EnthalpyRange == null ? null : new Range<double>(mollierSettings.EnthalpyRange);
                Enthalpy_Interval = mollierSettings.Enthalpy_Interval;

                SpecificVolumeRange = mollierSettings.SpecificVolumeRange == null ? null : new Range<double>(mollierSettings.SpecificVolumeRange);
                SpecificVolume_Interval = mollierSettings.SpecificVolume_Interval;

                WetBulbTemperatureRange = mollierSettings.WetBulbTemperatureRange == null ? null : new Range<double>(mollierSettings.WetBulbTemperatureRange);
                WetBulbTemperature_Interval = mollierSettings.WetBulbTemperature_Interval;
            }
        }

        public MollierSettings(JsonObject jObject)
        {
            FromJsonObject(jObject);
        }

        public bool FromJsonObject(JsonObject jObject)
        {
            if (jObject.ContainsKey("ChartType"))
            {
                ChartType = Core.Query.Enum<ChartType>(jObject["ChartType"]?.GetValue<string>() ?? null);
            }

            if (jObject.ContainsKey("Pressure"))
            {
                Pressure = jObject["Pressure"]?.GetValue<double>() ?? default(double);
            }

            if (jObject.ContainsKey("MollierRange"))
            {
                MollierRange = new MollierRange(jObject["MollierRange"] as JsonObject);
            }

            if (jObject.ContainsKey("HumidityRatio_Interval"))
            {
                HumidityRatio_Interval = jObject["HumidityRatio_Interval"]?.GetValue<double>() ?? default(double);
            }

            if (jObject.ContainsKey("DryBulbTemperature_Interval"))
            {
                DryBulbTemperature_Interval = jObject["DryBulbTemperature_Interval"]?.GetValue<double>() ?? default(double);
            }

            if (jObject.ContainsKey("DensityRange"))
            {
                DensityRange = new Range<double>(jObject["DensityRange"] as JsonObject);
            }

            if (jObject.ContainsKey("Density_Interval"))
            {
                Density_Interval = jObject["Density_Interval"]?.GetValue<double>() ?? default(double);
            }

            if (jObject.ContainsKey("EnthalpyRange"))
            {
                EnthalpyRange = new Range<double>(jObject["EnthalpyRange"] as JsonObject);
            }

            if (jObject.ContainsKey("Enthalpy_Interval"))
            {
                Enthalpy_Interval = jObject["Enthalpy_Interval"]?.GetValue<double>() ?? default(double);
            }

            if (jObject.ContainsKey("SpecificVolumeRange"))
            {
                SpecificVolumeRange = new Range<double>(jObject["SpecificVolumeRange"] as JsonObject);
            }

            if (jObject.ContainsKey("SpecificVolume_Interval"))
            {
                SpecificVolume_Interval = jObject["SpecificVolume_Interval"]?.GetValue<double>() ?? default(double);
            }

            if (jObject.ContainsKey("WetBulbTemperatureRange"))
            {
                WetBulbTemperatureRange = new Range<double>(jObject["WetBulbTemperatureRange"] as JsonObject);
            }

            if (jObject.ContainsKey("WetBulbTemperature_Interval"))
            {
                WetBulbTemperature_Interval = jObject["WetBulbTemperature_Interval"]?.GetValue<double>() ?? default(double);
            }

            return true;
        }

        public JsonObject ToJsonObject()
        {
            JsonObject result = new JsonObject();
            result.Add("_type", Core.Query.FullTypeName(this));

            result.Add("ChartType", ChartType.ToString());

            if (!double.IsNaN(Pressure))
            {
                result.Add("Pressure", Pressure);
            }

            if(MollierRange != null)
            {
                result.Add("MollierRange", MollierRange.ToJsonObject());
            }

            if (!double.IsNaN(HumidityRatio_Interval))
            {
                result.Add("HumidityRatio_Interval", HumidityRatio_Interval);
            }

            if (!double.IsNaN(DryBulbTemperature_Interval))
            {
                result.Add("DryBulbTemperature_Interval", DryBulbTemperature_Interval);
            }

            if (DensityRange != null)
            {
                result.Add("DensityRange", DensityRange.ToJsonObject());
            }

            if (!double.IsNaN(Density_Interval))
            {
                result.Add("Density_Interval", Density_Interval);
            }

            if (EnthalpyRange != null)
            {
                result.Add("EnthalpyRange", EnthalpyRange.ToJsonObject());
            }

            if (!double.IsNaN(Enthalpy_Interval))
            {
                result.Add("Enthalpy_Interval", Enthalpy_Interval);
            }

            if (SpecificVolumeRange != null)
            {
                result.Add("SpecificVolumeRange", SpecificVolumeRange.ToJsonObject());
            }

            if (!double.IsNaN(SpecificVolume_Interval))
            {
                result.Add("SpecificVolume_Interval", SpecificVolume_Interval);
            }

            if (WetBulbTemperatureRange != null)
            {
                result.Add("WetBulbTemperatureRange", WetBulbTemperatureRange.ToJsonObject());
            }

            if (!double.IsNaN(WetBulbTemperature_Interval))
            {
                result.Add("WetBulbTemperature_Interval", WetBulbTemperature_Interval);
            }

            return result;
        }
    }
}
