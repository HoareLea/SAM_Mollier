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
            double spf = 1.2;

            List<IMollierProcess> result = new List<IMollierProcess>();

            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SupplyAirFlow, out double supplyAirFlow);

            double coolingCoilContactFactor = double.NaN;
            double coolingCoilSensibleLoad = double.NaN;
            double coolingCoilTotalLoad = double.NaN;
            double heatingCoilSensibleLoad = double.NaN;
            double heatingCoilTotalLoad = double.NaN;
            double frostCoilSensibleLoad = double.NaN;
            double frostCoilTotalLoad = double.NaN;
            double humidificationDuty = double.NaN;

            //WINTER
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignTemperature, out double winterDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignRelativeHumidity, out double winterDesignRelativeHumidity);
            if (!double.IsNaN(winterDesignTemperature) && !double.IsNaN(winterDesignRelativeHumidity))
            {
                MollierPoint start = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterDesignTemperature, winterDesignRelativeHumidity, pressure);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.FrostCoilOffTemperature, out double winterFrostOffCoilTemperature);

                //HEATING (FROST COIL)
                if (!double.IsNaN(winterFrostOffCoilTemperature) && winterFrostOffCoilTemperature > winterDesignTemperature)
                {
                    HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess(start, winterFrostOffCoilTemperature);
                    if (heatingProcess != null)
                    {
                        result.Add(heatingProcess);
                        start = heatingProcess.End;
                    }

                    if (!double.IsNaN(supplyAirFlow))
                    {
                        frostCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(heatingProcess, supplyAirFlow);
                        frostCoilTotalLoad = Core.Mollier.Query.TotalLoad(heatingProcess, supplyAirFlow);
                    }
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryDryBulbTemperature, out double winterHeatRecoveryDryBulbTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryRelativeHumidity, out double winterHeatRecoveryRelativeHumidity);

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoverySensibleEfficiency, out double winterHeatRecoverySensibleEfficiency) || double.IsNaN(winterHeatRecoverySensibleEfficiency))
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
                    if (@return != null)
                    {
                        HeatRecoveryProcess heatRecoveryProcess = Core.Mollier.Create.HeatRecoveryProcess(start, @return, winterHeatRecoverySensibleEfficiency, winterHeatRecoveryLatentEfficiency);
                        if (heatRecoveryProcess != null)
                        {
                            result.Add(heatRecoveryProcess);
                            start = heatRecoveryProcess.End;
                        }
                    }
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceTemperature, out double winterSpaceTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceRelativeHumidty, out double winterSpaceRelativeHumidity);

                MollierPoint room = !double.IsNaN(winterSpaceTemperature) && !double.IsNaN(winterSpaceRelativeHumidity) ? Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterSpaceTemperature, winterSpaceRelativeHumidity, pressure) : null;

                //MIXING
                if (room != null)
                {
                    airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, out double outsideSupplyAirFlow);

                    if (!double.IsNaN(outsideSupplyAirFlow) && !double.IsNaN(supplyAirFlow))
                    {
                        MixingProcess mixingProcess = Core.Mollier.Create.MixingProcess(start, room, outsideSupplyAirFlow, supplyAirFlow);
                        if (mixingProcess != null)
                        {
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
                    if (heatingProcess != null)
                    {
                        result.Add(heatingProcess);
                        start = heatingProcess.End;
                    }

                    if (double.IsNaN(supplyAirFlow))
                    {
                        heatingCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(heatingProcess, supplyAirFlow);
                        heatingCoilTotalLoad = Core.Mollier.Query.TotalLoad(heatingProcess, supplyAirFlow);
                    }
                }

                //HUMIDIFICATION (STEAM HUMIDIFIER)
                if (room != null)
                {
                    IsotermicHumidificationProcess isotermicHumidificationProcess = Core.Mollier.Create.IsotermicHumidificationProcess_ByRelativeHumidity(start, room.RelativeHumidity);
                    if (isotermicHumidificationProcess != null)
                    {
                        result.Add(isotermicHumidificationProcess);
                        start = isotermicHumidificationProcess.End;
                    }

                    humidificationDuty = Core.Mollier.Query.Duty(isotermicHumidificationProcess, supplyAirFlow);
                }

                //HEATING (FAN)
                double dryBulbTemperature_Fan = start.DryBulbTemperature + PickupTemperature(start, spf);

                HeatingProcess heatingProcess_Fan = Core.Mollier.Create.HeatingProcess(start, dryBulbTemperature_Fan);
                if (heatingProcess_Fan != null)
                {
                    result.Add(heatingProcess_Fan);
                    start = heatingProcess_Fan.End;
                }


                //TO ROOM
                if (room != null)
                {
                    UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, room);
                    if (undefinedProcess != null)
                    {
                        result.Add(undefinedProcess);
                        start = undefinedProcess.End;
                    }
                }

            }

            //SUMMER
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignTemperature, out double summerDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignRelativeHumidity, out double summerDesignRelativeHumidity);
            if (!double.IsNaN(winterDesignTemperature) && !double.IsNaN(winterDesignRelativeHumidity))
            {
                MollierPoint start = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(summerDesignTemperature, summerDesignRelativeHumidity, pressure);

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryDryBulbTemperature, out double summerHeatRecoveryDryBulbTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryRelativeHumidity, out double summerHeatRecoveryRelativeHumidity);

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatRecoverySensibleEfficiency, out double summerHeatRecoverySensibleEfficiency) || double.IsNaN(summerHeatRecoverySensibleEfficiency))
                {
                    summerHeatRecoverySensibleEfficiency = 0;
                }

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryLatentEfficiency, out double summerHeatRecoveryLatentEfficiency) || double.IsNaN(summerHeatRecoveryLatentEfficiency))
                {
                    summerHeatRecoveryLatentEfficiency = 0;
                }

                //HEAT RECOVERY
                if (!double.IsNaN(summerHeatRecoveryRelativeHumidity) && !double.IsNaN(summerHeatRecoveryDryBulbTemperature))
                {
                    MollierPoint @return = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(summerHeatRecoveryDryBulbTemperature, summerHeatRecoveryRelativeHumidity, pressure);
                    if (@return != null)
                    {
                        HeatRecoveryProcess heatRecoveryProcess = Core.Mollier.Create.HeatRecoveryProcess(start, @return, summerHeatRecoverySensibleEfficiency, summerHeatRecoveryLatentEfficiency);
                        if (heatRecoveryProcess != null)
                        {
                            result.Add(heatRecoveryProcess);
                            start = heatRecoveryProcess.End;
                        }
                    }
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceTemperature, out double summerSpaceTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceRelativeHumidty, out double summerSpaceRelativeHumidity);

                MollierPoint room = !double.IsNaN(summerSpaceTemperature) && !double.IsNaN(summerSpaceRelativeHumidity) ? Core.Mollier.Create.MollierPoint_ByRelativeHumidity(summerSpaceTemperature, summerSpaceRelativeHumidity, pressure) : null;

                //MIXING
                if (room != null)
                {
                    airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, out double outsideSupplyAirFlow);

                    if (!double.IsNaN(outsideSupplyAirFlow) && !double.IsNaN(supplyAirFlow))
                    {
                        MixingProcess mixingProcess = Core.Mollier.Create.MixingProcess(start, room, outsideSupplyAirFlow, supplyAirFlow);
                        if (mixingProcess != null)
                        {
                            result.Add(mixingProcess);
                            start = mixingProcess.End;
                        }
                    }
                }

                //COOLING (COOLING COIL)
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.CoolingCoilFluidFlowTemperature, out double coolingCoilFluidFlowTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.CoolingCoilFluidReturnTemperature, out double coolingCoilFluidReturnTemperature);
                if (!double.IsNaN(coolingCoilFluidReturnTemperature) && !double.IsNaN(coolingCoilFluidFlowTemperature))
                {
                    airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSupplyTemperature, out double summerSupplyTempearture);
                    if (!double.IsNaN(summerSupplyTempearture))
                    {
                        double pickupTemperature = PickupTemperature(start, spf);

                        CoolingProcess coolingProcess = Core.Mollier.Create.CoolingProcess(start, summerSupplyTempearture - pickupTemperature);
                        if (coolingProcess != null)
                        {
                            result.Add(coolingProcess);
                            start = coolingProcess.End;
                        }

                        coolingCoilContactFactor = coolingProcess.ContactFactor();

                        if(!double.IsNaN(supplyAirFlow))
                        {
                            coolingCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(coolingProcess, supplyAirFlow);
                            coolingCoilTotalLoad = Core.Mollier.Query.TotalLoad(coolingProcess, supplyAirFlow);
                        }
                    }

                    //MollierPoint mollierPoint_ADP = Core.Mollier.Create.MollierPoint_ByRelativeHumidity((coolingCoilFluidFlowTemperature + coolingCoilFluidReturnTemperature) / 2, 100, pressure);
                    //if(mollierPoint_ADP != null)
                    //{

                    //}
                }
            }

            return result;
        }
    }
}

