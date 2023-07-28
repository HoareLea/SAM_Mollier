using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class MollierSettings : IJSAMObject
    {
        public ChartType ChartType { get; set; } = ChartType.Mollier;
        public double Pressure { get; set; } = Standard.Pressure;


        //Dry Bulb Temperature and Humidity Ratio
        public MollierRange MollierRange { get; set; } = Create.MollierRange();
        public double HumidityRatio_Interval { get; set; } = Default.HumidityRatio_Interval / 1000;
        public double DryBulbTemperature_Interval { get; set; } = Default.DryBulbTemperature_Interval;

        //Density
        public Range<double> DensityRange { get; set; } = new Range<double>(Default.Density_Min, Default.Density_Max);
        public double Density_Interval { get; set; } = Default.Density_Interval;

        //Enthalpy
        public Range<double> EnthalpyRange { get; set; } = new Range<double>(Default.Enthalpy_Min * 1000, Default.Enthalpy_Max * 1000);
        public double Enthalpy_Interval { get; set; } = Default.Enthalpy_Interval * 1000;


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

        public MollierSettings(JObject jObject)
        {
            FromJObject(jObject);
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject.ContainsKey("ChartType"))
            {
                ChartType = Core.Query.Enum<ChartType>(jObject.Value<string>("ChartType"));
            }

            if (jObject.ContainsKey("Pressure"))
            {
                Pressure = jObject.Value<double>("Pressure");
            }

            if (jObject.ContainsKey("MollierRange"))
            {
                MollierRange = new MollierRange(jObject.Value<JObject>("MollierRange"));
            }

            if (jObject.ContainsKey("HumidityRatio_Interval"))
            {
                HumidityRatio_Interval = jObject.Value<double>("HumidityRatio_Interval");
            }

            if (jObject.ContainsKey("DryBulbTemperature_Interval"))
            {
                DryBulbTemperature_Interval = jObject.Value<double>("DryBulbTemperature_Interval");
            }

            if (jObject.ContainsKey("DensityRange"))
            {
                DensityRange = new Range<double>(jObject.Value<JObject>("DensityRange"));
            }

            if (jObject.ContainsKey("Density_Interval"))
            {
                Density_Interval = jObject.Value<double>("Density_Interval");
            }

            if (jObject.ContainsKey("EnthalpyRange"))
            {
                EnthalpyRange = new Range<double>(jObject.Value<JObject>("EnthalpyRange"));
            }

            if (jObject.ContainsKey("Enthalpy_Interval"))
            {
                Enthalpy_Interval = jObject.Value<double>("Enthalpy_Interval");
            }

            if (jObject.ContainsKey("SpecificVolumeRange"))
            {
                SpecificVolumeRange = new Range<double>(jObject.Value<JObject>("SpecificVolumeRange"));
            }

            if (jObject.ContainsKey("SpecificVolume_Interval"))
            {
                SpecificVolume_Interval = jObject.Value<double>("SpecificVolume_Interval");
            }

            if (jObject.ContainsKey("WetBulbTemperatureRange"))
            {
                WetBulbTemperatureRange = new Range<double>(jObject.Value<JObject>("WetBulbTemperatureRange"));
            }

            if (jObject.ContainsKey("WetBulbTemperature_Interval"))
            {
                WetBulbTemperature_Interval = jObject.Value<double>("WetBulbTemperature_Interval");
            }

            return true;
        }

        public JObject ToJObject()
        {
            JObject result = new JObject();
            result.Add("_type", Core.Query.FullTypeName(this));

            result.Add("ChartType", ChartType.ToString());

            if (!double.IsNaN(Pressure))
            {
                result.Add("Pressure", Pressure);
            }

            if(MollierRange != null)
            {
                result.Add("MollierRange", MollierRange.ToJObject());
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
                result.Add("DensityRange", DensityRange.ToJObject());
            }

            if (!double.IsNaN(Density_Interval))
            {
                result.Add("Density_Interval", Density_Interval);
            }

            if (EnthalpyRange != null)
            {
                result.Add("EnthalpyRange", EnthalpyRange.ToJObject());
            }

            if (!double.IsNaN(Enthalpy_Interval))
            {
                result.Add("Enthalpy_Interval", Enthalpy_Interval);
            }

            if (SpecificVolumeRange != null)
            {
                result.Add("SpecificVolumeRange", SpecificVolumeRange.ToJObject());
            }

            if (!double.IsNaN(SpecificVolume_Interval))
            {
                result.Add("SpecificVolume_Interval", SpecificVolume_Interval);
            }

            if (WetBulbTemperatureRange != null)
            {
                result.Add("WetBulbTemperatureRange", WetBulbTemperatureRange.ToJObject());
            }

            if (!double.IsNaN(WetBulbTemperature_Interval))
            {
                result.Add("WetBulbTemperature_Interval", WetBulbTemperature_Interval);
            }

            return result;
        }
    }
}
