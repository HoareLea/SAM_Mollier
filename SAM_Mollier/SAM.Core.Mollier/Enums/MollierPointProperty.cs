using System.ComponentModel;

namespace SAM.Core.Mollier
{
    public enum MollierPointProperty
    {
        [Description("Undefined")] Undefined,
        [Description("Relative Humidity")] RelativeHumidity,
        [Description("Diagram Temperature")] DiagramTemperature,
        [Description("Specific Volume")] SpecificVolume,
        [Description("Density")] Density,
        [Description("Enthalpy")] Enthalpy,
        [Description("Wet Bulb Temperature")] WetBulbTemperature,
        [Description("Dew Point Temperature")] DewPointTemperature,
        [Description("Humidity Ratio")] HumidityRatio,
        [Description("Dry Bulb Temperature")] DryBulbTemperature
    }
}
