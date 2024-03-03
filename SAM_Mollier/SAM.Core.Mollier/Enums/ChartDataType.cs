using System.ComponentModel;

namespace SAM.Core.Mollier
{
    public enum ChartDataType
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
        [Description("Dry Bulb Temperature")] DryBulbTemperature,
        [Description("Heating Process")] HeatingProcess,
        [Description("Cooling Process")] CoolingProcess,
        [Description("Heat Recovery Process")] HeatRecoveryProcess,
        [Description("Mixing Process")] MixingProcess,
        [Description("Steam Humidification Process")] SteamHumidificationProcess,
        [Description("Adiabatic Humidification Process")] AdiabaticHumidificationProcess,
        [Description("Isotermic Humidification Process")] IsotermicHumidificationProcess,
        [Description("Undefined Process")] UndefinedProcess,
        [Description("Room Process")] RoomProcess,
        [Description("Sensible Heat Ratio")] SensibleHeatRatio,
    }
}
