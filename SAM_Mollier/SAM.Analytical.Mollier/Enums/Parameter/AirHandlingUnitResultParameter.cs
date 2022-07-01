using System.ComponentModel;
using SAM.Core.Attributes;

namespace SAM.Analytical.Mollier
{
    [AssociatedTypes(typeof(AirHandlingUnitResult)), Description("AirHandlingUnitResult Parameter")]
    public enum AirHandlingUnitResultParameter
    {
        [ParameterProperties("Sensible Heat Loss", "Sensible Heat Loss [W]"), ParameterValue(Core.ParameterType.Double)] SensibleHeatLoss,
        [ParameterProperties("Senisble Heat Gain", "Sensible Heat Gain [W]"), ParameterValue(Core.ParameterType.Double)] SensibleHeatGain,
        [ParameterProperties("Summer Design Temperature", "Summer Design Temperature [C]"), ParameterValue(Core.ParameterType.Double)] SummerDesignTemperature,
        [ParameterProperties("Summer Design Relative Humidity", "Summer Design Relative Humidity [%]"), ParameterValue(Core.ParameterType.Double)] SummerDesignRelativeHumidity,
        [ParameterProperties("Summer Design Day Name", "Summer Design Day Name"), ParameterValue(Core.ParameterType.String)] SummerDesignDayName,
        [ParameterProperties("Summer Design Day Index", "Summer Design Day Index"), ParameterValue(Core.ParameterType.Integer)] SummerDesignDayIndex,
    }
}