using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Modify
    {
        public static MollierGroup UpdateProcesses(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if (airHandlingUnitResult == null)
            {
                return null;
            }

            double pressure = Standard.Pressure;
            double spf = 1.2;

            MollierGroup mollierGroup_Winter = new MollierGroup("Winter");

            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SupplyAirFlow, out double supplyAirFlow);

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
                        mollierGroup_Winter.Add(heatingProcess);
                        start = heatingProcess.End;
                    }

                    if (!double.IsNaN(supplyAirFlow))
                    {
                        double frostCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(heatingProcess, supplyAirFlow);
                        if(!double.IsNaN(frostCoilSensibleLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.FrostCoilSensibleLoad, frostCoilSensibleLoad);
                        }
                        
                        double frostCoilTotalLoad = Core.Mollier.Query.TotalLoad(heatingProcess, supplyAirFlow);
                        if(!double.IsNaN(frostCoilTotalLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.FrostCoilTotalLoad, frostCoilTotalLoad);
                        }
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
                            mollierGroup_Winter.Add(heatRecoveryProcess);
                            start = heatRecoveryProcess.End;
                        }
                    }
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceTemperature, out double winterSpaceTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceRelativeHumidty, out double winterSpaceRelativeHumidity);

                MollierPoint room_Winter = !double.IsNaN(winterSpaceTemperature) && !double.IsNaN(winterSpaceRelativeHumidity) ? Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterSpaceTemperature, winterSpaceRelativeHumidity, pressure) : null;

                //MIXING
                if (room_Winter != null)
                {
                    airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, out double outsideSupplyAirFlow);

                    if (!double.IsNaN(outsideSupplyAirFlow) && !double.IsNaN(supplyAirFlow))
                    {
                        double returnAirFlow = System.Math.Abs(supplyAirFlow - outsideSupplyAirFlow);
                        if(returnAirFlow > Core.Tolerance.Distance)
                        {
                            MixingProcess mixingProcess = Core.Mollier.Create.MixingProcess(start, room_Winter, supplyAirFlow, returnAirFlow);
                            if (mixingProcess != null)
                            {
                                mollierGroup_Winter.Add(mixingProcess);
                                start = mixingProcess.End;
                            }
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
                        mollierGroup_Winter.Add(heatingProcess);
                        start = heatingProcess.End;
                    }

                    if (!double.IsNaN(supplyAirFlow))
                    {
                        double heatingCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(heatingProcess, supplyAirFlow);
                        if(!double.IsNaN(heatingCoilSensibleLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.HeatingCoilSensibleLoad, heatingCoilSensibleLoad);
                        }

                        double heatingCoilTotalLoad = Core.Mollier.Query.TotalLoad(heatingProcess, supplyAirFlow);
                        if(!double.IsNaN(heatingCoilTotalLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.HeatingCoilTotalLoad, heatingCoilTotalLoad);
                        }
                    }
                }

                //HUMIDIFICATION (STEAM HUMIDIFIER)
                if (room_Winter != null)
                {
                    SteamHumidificationProcess steamHumidificationProcess = Core.Mollier.Create.SteamHumidificationProcess_ByRelativeHumidity(start, room_Winter.RelativeHumidity);
                    if (steamHumidificationProcess != null)
                    {
                        mollierGroup_Winter.Add(steamHumidificationProcess);
                        start = steamHumidificationProcess.End;
                    }

                    double humidificationDuty = Core.Mollier.Query.Duty(steamHumidificationProcess, supplyAirFlow);
                    if(!double.IsNaN(humidificationDuty))
                    {
                        airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.HumidificationDuty, humidificationDuty);
                    }
                }

                //HEATING (FAN)
                double dryBulbTemperature_Fan = start.DryBulbTemperature + Query.PickupTemperature(start, spf);

                HeatingProcess heatingProcess_Fan = Core.Mollier.Create.HeatingProcess(start, dryBulbTemperature_Fan);
                if (heatingProcess_Fan != null)
                {
                    mollierGroup_Winter.Add(heatingProcess_Fan);
                    start = heatingProcess_Fan.End;
                }

                if(start != null)
                {
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.WinterSupplyFanTemperature, start.DryBulbTemperature);
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.WinterSupplyFanRelativeHumidty, start.RelativeHumidity);
                }

                //TO ROOM
                if (room_Winter != null)
                {
                    UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, room_Winter);
                    if (undefinedProcess != null)
                    {
                        mollierGroup_Winter.Add(undefinedProcess);
                        start = undefinedProcess.End;
                    }
                }

            }

            MollierGroup mollierGroup_Summer = new MollierGroup("Summer");

            //SUMMER
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignTemperature, out double summerDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignRelativeHumidity, out double summerDesignRelativeHumidity);
            if (!double.IsNaN(summerDesignTemperature) && !double.IsNaN(summerDesignRelativeHumidity))
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
                            mollierGroup_Summer.Add(heatRecoveryProcess);
                            start = heatRecoveryProcess.End;
                        }
                    }
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceTemperature, out double summerSpaceTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceRelativeHumidty, out double summerSpaceRelativeHumidity);

                MollierPoint room_Summer = !double.IsNaN(summerSpaceTemperature) && !double.IsNaN(summerSpaceRelativeHumidity) ? Core.Mollier.Create.MollierPoint_ByRelativeHumidity(summerSpaceTemperature, summerSpaceRelativeHumidity, pressure) : null;

                //MIXING
                if (room_Summer != null)
                {
                    airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, out double outsideSupplyAirFlow);

                    double returnAirFlow = System.Math.Abs(supplyAirFlow - outsideSupplyAirFlow);
                    if (returnAirFlow > Core.Tolerance.Distance)
                    {
                        MixingProcess mixingProcess = Core.Mollier.Create.MixingProcess(start, room_Summer, supplyAirFlow, returnAirFlow);
                        if (mixingProcess != null)
                        {
                            mollierGroup_Summer.Add(mixingProcess);
                            start = mixingProcess.End;
                        }
                    }
                }

                //COOLING (COOLING COIL)
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSupplyTemperature, out double summerSupplyTempearture);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.CoolingCoilContactFactor, out double coolingCoilContactFactor);
                if (!double.IsNaN(summerSupplyTempearture) && !double.IsNaN(coolingCoilContactFactor))
                {
                    double dryBulbTemperature = summerSupplyTempearture - Query.PickupTemperature(summerSupplyTempearture, spf);

                    CoolingProcess coolingProcess = Core.Mollier.Create.CoolingProcess(start, dryBulbTemperature, coolingCoilContactFactor);
                    if (coolingProcess != null)
                    {
                        mollierGroup_Summer.Add(coolingProcess);
                        start = coolingProcess.End;

                        if (!double.IsNaN(coolingProcess.Efficiency))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.CoolingCoilContactFactor, coolingProcess.Efficiency);
                        }
                    }

                    if (!double.IsNaN(supplyAirFlow))
                    {
                        double coolingCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(coolingProcess, supplyAirFlow);
                        if (!double.IsNaN(coolingCoilSensibleLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.CoolingCoilSensibleLoad, coolingCoilSensibleLoad);
                        }

                        double coolingCoilTotalLoad = Core.Mollier.Query.TotalLoad(coolingProcess, supplyAirFlow);
                        if (!double.IsNaN(coolingCoilTotalLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.CoolingCoilTotalLoad, coolingCoilTotalLoad);
                        }
                    }
                }

                if (airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatingCoil, out bool summerHeatingCoil) && summerHeatingCoil)
                {
                    double temperatureDifference = summerSpaceTemperature - start.DryBulbTemperature;
                    HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess_ByTemperatureDifference(start, temperatureDifference);
                    if (heatingProcess != null)
                    {
                        mollierGroup_Summer.Add(heatingProcess);
                        start = heatingProcess.End;

                        double heatingCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(heatingProcess, supplyAirFlow);
                        if (!double.IsNaN(heatingCoilSensibleLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerHeatingCoilSensibleLoad, heatingCoilSensibleLoad);
                        }

                        double heatingCoilTotalLoad = Core.Mollier.Query.TotalLoad(heatingProcess, supplyAirFlow);
                        if (!double.IsNaN(heatingCoilTotalLoad))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerHeatingCoilTotalLoad, heatingCoilTotalLoad);
                        }
                    }
                }

                //HEATING (FAN)
                double dryBulbTemperature_Fan = start.DryBulbTemperature + Query.PickupTemperature(summerSupplyTempearture, spf);

                HeatingProcess heatingProcess_Fan = Core.Mollier.Create.HeatingProcess(start, dryBulbTemperature_Fan);
                if (heatingProcess_Fan != null)
                {
                    mollierGroup_Summer.Add(heatingProcess_Fan);
                    start = heatingProcess_Fan.End;
                }

                if (start != null)
                {
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerSupplyFanTemperature, start.DryBulbTemperature);
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerSupplyFanRelativeHumidty, start.RelativeHumidity);
                }

                //TO ROOM
                if (room_Summer != null)
                {
                    UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, room_Summer);
                    if (undefinedProcess != null)
                    {
                        mollierGroup_Summer.Add(undefinedProcess);
                        start = undefinedProcess.End;
                    }
                }
            }

            MollierGroup result = new MollierGroup(airHandlingUnitResult.Name);
            result.Add(mollierGroup_Winter);
            result.Add(mollierGroup_Summer);

            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.Processes, result);

            return result;
        }
    }
}

