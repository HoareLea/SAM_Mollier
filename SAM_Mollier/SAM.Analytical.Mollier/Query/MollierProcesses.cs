using SAM.Core.Mollier;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static List<IMollierProcess> MollierProcesses(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if (airHandlingUnitResult == null)
            {
                return null;
            }

            double pressure = 101325;

            List<IMollierProcess> result = new List<IMollierProcess>();

            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignTemperature, out double winterDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignRelativeHumidity, out double winterDesignRelativeHumidity);
            if(!double.IsNaN(winterDesignTemperature) && !double.IsNaN(winterDesignRelativeHumidity))
            {
                MollierPoint start = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterDesignTemperature, winterDesignRelativeHumidity, pressure);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.FrostCoilOffTemperature, out double winterFrostOffCoilTemperature);

                //HEATING (FROST COIL)
                if (!double.IsNaN(winterFrostOffCoilTemperature) && winterFrostOffCoilTemperature > winterDesignTemperature)
                {
                    HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess(start, winterFrostOffCoilTemperature);
                    result.Add(heatingProcess);

                    start = heatingProcess.End;
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryDryBulbTemperature, out double winterHeatRecoveryDryBulbTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryRelativeHumidity, out double winterHeatRecoveryRelativeHumidity);

                if(!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoverySensibleEfficiency, out double winterHeatRecoverySensibleEfficiency) || double.IsNaN(winterHeatRecoverySensibleEfficiency))
                {
                    winterHeatRecoverySensibleEfficiency = 0;
                }

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryLatentEfficiency, out double winterHeatRecoveryLatentEfficiency) || double.IsNaN(winterHeatRecoveryLatentEfficiency))
                {
                    winterHeatRecoveryLatentEfficiency = 0;
                }

                //HEAT RECOVERY
                if (!double.IsNaN(winterHeatRecoveryRelativeHumidity) && !double.IsNaN(winterHeatRecoveryDryBulbTemperature))
                {
                    MollierPoint @return = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterHeatRecoveryDryBulbTemperature, winterHeatRecoveryRelativeHumidity, pressure);
                    if(@return != null)
                    {
                        HeatRecoveryProcess heatRecoveryProcess = Core.Mollier.Create.HeatRecoveryProcess(start, @return, winterHeatRecoverySensibleEfficiency, winterHeatRecoveryLatentEfficiency);
                        result.Add(heatRecoveryProcess);

                        start = heatRecoveryProcess.End;
                    }
                }

                //MIXING
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceTemperature, out double winterSpaceTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceRelativeHumidty, out double winterSpaceRelativeHumidity);
                if(!double.IsNaN(winterSpaceTemperature) && !double.IsNaN(winterSpaceRelativeHumidity))
                {
                    MollierPoint room = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterSpaceTemperature, winterSpaceRelativeHumidity, pressure);
                    if(room != null)
                    {
                        airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, out double outsideSupplyAirFlow);
                        airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SupplyAirFlow, out double supplyAirFlow);

                        if (!double.IsNaN(outsideSupplyAirFlow) && !double.IsNaN(supplyAirFlow))
                        {
                            MixingProcess mixingProcess = Core.Mollier.Create.MixingProcess(start, room, outsideSupplyAirFlow, supplyAirFlow);
                            result.Add(mixingProcess);

                            start = mixingProcess.End;
                        }
                    }
                }

                //HEATING (HEATING COIL)
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatingCoilSupplyTemperature, out double winterHeatingCoilSupplyTemperature);
                if (!double.IsNaN(winterHeatingCoilSupplyTemperature))
                {
                    HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess(start, winterHeatingCoilSupplyTemperature);
                    result.Add(heatingProcess);

                    start = heatingProcess.End;
                }
            }

            return result;
        }
    }
}

