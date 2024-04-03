using System.ComponentModel;

namespace SAM.Core.Mollier
{
    public enum MollierProcessType
    {
        [Description("Undefined")] Undefined,
        [Description("Heating")] Heating,
        [Description("Cooling")] Cooling,
        [Description("Isothermic Humidification (by steam)")] IsothermicHumidification,
        [Description("Adiabatic Humidification (by water spray)")] AdiabaticHumidification,
        [Description("Heat Recovery")] HeatRecovery,
        [Description("Mixing")] Mixing,
    }
}
