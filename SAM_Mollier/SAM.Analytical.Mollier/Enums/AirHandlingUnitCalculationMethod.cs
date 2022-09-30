using System.ComponentModel;

namespace SAM.Analytical.Mollier
{
    [Description("Air Handling Unit Calculation Method")]
    public enum AirHandlingUnitCalculationMethod
    {
        [Description("Undefined")] Undefined,
        [Description("Fixed Supply Temperature")] FixedSupplyTemperature,
        [Description("Match by Humidity Ratio with no limit")] HumidityRatio,
        [Description("Match by Humidity Ratio and Chilled Water Temperature")] HumidityRatioAndChilledWaterTemperature,
    }
}