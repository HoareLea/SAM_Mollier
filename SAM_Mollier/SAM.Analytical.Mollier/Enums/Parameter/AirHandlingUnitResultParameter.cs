using System.ComponentModel;
using SAM.Core.Attributes;

namespace SAM.Analytical.Mollier
{
    [AssociatedTypes(typeof(AirHandlingUnitResult)), Description("AirHandlingUnitResult Parameter")]
    public enum AirHandlingUnitResultParameter
    {
        [ParameterProperties("Sensible Heat Loss", "Sensible Heat Loss [W]"), ParameterValue(Core.ParameterType.Double)] SensibleHeatLoss,
        [ParameterProperties("Senisble Heat Gain", "Sensible Heat Gain [W]"), ParameterValue(Core.ParameterType.Double)] SensibleHeatGain,
    }
}