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
        [ParameterProperties("Winter Design Temperature", "Winter Design Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] WinterDesignTemperature,
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
        [ParameterProperties("Summer Supply Temperature", "Summer Supply Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] SummerSupplyTemperature,
        [ParameterProperties("Winter Supply Temperature", "Winter Supply Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] WinterSupplyTemperature,
        [ParameterProperties("Frost Coil Off Temperature", "Frost Coil Off Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] FrostCoilOffTemperature,
        [ParameterProperties("Winter Heat Recovery Sensible Efficiency", "Winter Heat Recovery Sensible Efficiency [%]"), DoubleParameterValue(0, 100)] WinterHeatRecoverySensibleEfficiency,
        [ParameterProperties("Winter Heat Recovery Latent Efficiency", "Winter Heat Recovery Latent Efficiency [%]"), DoubleParameterValue(0, 100)] WinterHeatRecoveryLatentEfficiency,
        [ParameterProperties("Summer Heat Recovery Sensible Efficiency", "Summer Heat Recovery Sensible Efficiency [%]"), DoubleParameterValue(0, 100)] SummerHeatRecoverySensibleEfficiency,
        [ParameterProperties("Summer Heat Recovery Latent Efficiency", "Summer Heat Recovery Latent Efficiency [%]"), DoubleParameterValue(0, 100)] SummerHeatRecoveryLatentEfficiency,
        [ParameterProperties("Cooling Coil Fluid Flow Temperature", "Cooling Coil Fluid Flow Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] CoolingCoilFluidFlowTemperature,
        [ParameterProperties("Cooling Coil Fluid Return Temperature", "Cooling Coil Fluid Return Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] CoolingCoilFluidReturnTemperature,
        [ParameterProperties("Winter Heating Coil Supply Temperature", "Winter Heating Coil Supply Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] WinterHeatingCoilSupplyTemperature,
        [ParameterProperties("Winter Heat Recovery Dry Bulb Temperature", "Winter Heat Recovery Dry Bulb Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] WinterHeatRecoveryDryBulbTemperature,
        [ParameterProperties("Winter Heat Recovery Relative Humidity", "Winter Heat Recovery Relative Humidity [%]"), DoubleParameterValue(0, 100)] WinterHeatRecoveryRelativeHumidity,
        [ParameterProperties("Summer Heat Recovery Dry Bulb Temperature", "Summer Heat Recovery Dry Bulb Temperature [°C]"), ParameterValue(Core.ParameterType.Double)] SummerHeatRecoveryDryBulbTemperature,
        [ParameterProperties("Summer Heat Recovery Relative Humidity", "Summer Heat Recovery Relative Humidity [%]"), DoubleParameterValue(0, 100)] SummerHeatRecoveryRelativeHumidity,
    }
}