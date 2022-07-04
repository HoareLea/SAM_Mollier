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
        [ParameterProperties("Winter Design Temperature", "Winter Design Temperature [C]"), ParameterValue(Core.ParameterType.Double)] WinterDesignTemperature,
        [ParameterProperties("Winter Design Relative Humidity", "Winter Design Relative Humidity [%]"), ParameterValue(Core.ParameterType.Double)] WinterDesignRelativeHumidity,
        [ParameterProperties("Winter Design Day Name", "Winter Design Day Name"), ParameterValue(Core.ParameterType.String)] WinterDesignDayName,
        [ParameterProperties("Winter Design Day Index", "Winter Design Day Index"), ParameterValue(Core.ParameterType.Integer)] WinterDesignDayIndex,
        [ParameterProperties("Supply Air Flow", "Supply Air Flow [m3/s]"), ParameterValue(Core.ParameterType.Double)] SupplyAirFlow,
        [ParameterProperties("Outside Supply Air Flow", "Outside Supply Air Flow [m3/s]"), ParameterValue(Core.ParameterType.Double)] OutsideSupplyAirFlow,
        [ParameterProperties("Exhaust Air Flow", "Exhaust Air Flow [m3/s]"), ParameterValue(Core.ParameterType.Double)] ExhaustAirFlow,
        [ParameterProperties("Winter Space Temperature", "Winter Space Temperature [C]"), ParameterValue(Core.ParameterType.Double)] WinterSpaceTemperature,
        [ParameterProperties("Summer Space Temperature", "Summer Space Temperature [C]"), ParameterValue(Core.ParameterType.Double)] SummerSpaceTemperature,
        [ParameterProperties("Winter Space Relative Humidity", "Winter Space Relative Humidty [%]"), ParameterValue(Core.ParameterType.Double)] WinterSpaceRelativeHumidty,
        [ParameterProperties("Summer Space Relative Humidity", "Summer Space Relative Humidty [%]"), ParameterValue(Core.ParameterType.Double)] SummerSpaceRelativeHumidty,
        [ParameterProperties("Summer Supply Temperature", "Summer Supply Temperature [C]"), ParameterValue(Core.ParameterType.Double)] SummerSupplyTemperature,
        [ParameterProperties("Winter Supply Temperature", "Winter Supply Temperature [C]"), ParameterValue(Core.ParameterType.Double)] WinterSupplyTemperature,
    }
}