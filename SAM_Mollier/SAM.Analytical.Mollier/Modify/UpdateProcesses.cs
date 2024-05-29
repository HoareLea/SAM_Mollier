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
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, out double outsideSupplyAirFlow);

            //WINTER
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignTemperature, out double winterDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignRelativeHumidity, out double winterDesignRelativeHumidity);

            if (!double.IsNaN(winterDesignTemperature) && !double.IsNaN(winterDesignRelativeHumidity))
            {
                MollierPoint start = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterDesignTemperature, winterDesignRelativeHumidity, pressure);

                //HEATING (FROST COIL)
                if (airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.FrostCoilOffTemperature, out double winterFrostOffCoilTemperature) 
                    && !double.IsNaN(winterFrostOffCoilTemperature) 
                    && winterFrostOffCoilTemperature > winterDesignTemperature)
                {
                    HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess(start, winterFrostOffCoilTemperature);
                    if (heatingProcess != null && !heatingProcess.Start.AlmostEqual(heatingProcess.End))
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

                //airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryDryBulbTemperature, out double winterHeatRecoveryDryBulbTemperature);
                //airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryRelativeHumidity, out double winterHeatRecoveryRelativeHumidity);

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoverySensibleEfficiency, out double winterHeatRecoverySensibleEfficiency) || double.IsNaN(winterHeatRecoverySensibleEfficiency))
                {
                    winterHeatRecoverySensibleEfficiency = 0;
                }

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryLatentEfficiency, out double winterHeatRecoveryLatentEfficiency) || double.IsNaN(winterHeatRecoveryLatentEfficiency))
                {
                    winterHeatRecoveryLatentEfficiency = 0;
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceTemperature, out double winterSpaceTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceRelativeHumidty, out double winterSpaceRelativeHumidity);

                MollierPoint room_Winter = !double.IsNaN(winterSpaceTemperature) && !double.IsNaN(winterSpaceRelativeHumidity) ? Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterSpaceTemperature, winterSpaceRelativeHumidity, pressure) : null;

                //HEAT RECOVERY
                if (room_Winter != null)
                {
                    HeatRecoveryProcess heatRecoveryProcess = Core.Mollier.Create.HeatRecoveryProcess_Supply(start, room_Winter, winterHeatRecoverySensibleEfficiency, winterHeatRecoveryLatentEfficiency);
                    if (heatRecoveryProcess != null && !heatRecoveryProcess.Start.AlmostEqual(heatRecoveryProcess.End))
                    {
                        mollierGroup_Winter.Add(heatRecoveryProcess);
                        start = heatRecoveryProcess.End;
                    }
                }

                    


                //MIXING
                if (room_Winter != null)
                {
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

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterSensibleLoad, out double winterSensibleLoad);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterLatentLoad, out double winterLatentLoad);

                //HEATING (HEATING COIL)
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatingCoilSupplyTemperature, out double winterHeatingCoilSupplyTemperature);
                if (!double.IsNaN(winterHeatingCoilSupplyTemperature))
                {
                    double temperatureDifference = 0;
                    if(supplyAirFlow == outsideSupplyAirFlow && winterHeatingCoilSupplyTemperature == winterSpaceTemperature)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, supplyAirFlow, winterSensibleLoad, winterLatentLoad);
                        temperatureDifference = (undefinedProcess.End.DryBulbTemperature - undefinedProcess.Start.DryBulbTemperature);
                    }

                    double dryBulbTemperature = winterHeatingCoilSupplyTemperature - Core.Mollier.Query.PickupTemperature(spf) + temperatureDifference;

                    HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess(start, dryBulbTemperature);
                    if (heatingProcess != null && !heatingProcess.Start.AlmostEqual(heatingProcess.End))
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
                    UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, supplyAirFlow, winterSensibleLoad, winterLatentLoad);
                    IsothermalHumidificationProcess isothermalHumidificationProcess = Core.Mollier.Create.IsothermalHumidificationProcess_ByHumidityRatioDifference(start, room_Winter.HumidityRatio - start.HumidityRatio + (undefinedProcess.Start.HumidityRatio - undefinedProcess.End.HumidityRatio));
                    if (isothermalHumidificationProcess != null && !isothermalHumidificationProcess.Start.AlmostEqual(isothermalHumidificationProcess.End))
                    {
                        mollierGroup_Winter.Add(isothermalHumidificationProcess);
                        start = isothermalHumidificationProcess.End;
                    }

                    double humidificationDuty = Core.Mollier.Query.Duty(isothermalHumidificationProcess, supplyAirFlow);
                    if(!double.IsNaN(humidificationDuty))
                    {
                        airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.HumidificationDuty, humidificationDuty);
                    }
                }

                //HEATING (FAN)
                double dryBulbTemperature_Fan = start.DryBulbTemperature + Core.Mollier.Query.PickupTemperature(spf);

                FanProcess fanProcess = Core.Mollier.Create.FanProcess_ByDryBulbTemperature(start, dryBulbTemperature_Fan);
                if (fanProcess != null && !fanProcess.Start.AlmostEqual(fanProcess.End))
                {
                    mollierGroup_Winter.Add(fanProcess);
                    start = fanProcess.End;
                }

                if(start != null)
                {
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.WinterSupplyFanTemperature, start.DryBulbTemperature);
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.WinterSupplyFanRelativeHumidty, start.RelativeHumidity);

                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.WinterSupplyTemperature, start.DryBulbTemperature);
                }

                //TO ROOM
                if (!double.IsNaN(winterLatentLoad) && !double.IsNaN(winterSensibleLoad))
                {
                    RoomProcess roomProcess_Room = Core.Mollier.Create.RoomProcess(start, supplyAirFlow, -winterSensibleLoad, winterLatentLoad);
                    if (roomProcess_Room != null && !roomProcess_Room.Start.AlmostEqual(roomProcess_Room.End))
                    {
                        mollierGroup_Winter.Add(roomProcess_Room);
                        start = roomProcess_Room.End;
                    }
                }

                if (room_Winter != null)
                {
                    mollierGroup_Winter.Add(room_Winter);
                }

            }

            MollierGroup mollierGroup_Summer = new MollierGroup("Summer");

            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSensibleLoad, out double summerSensibleLoad);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerLatentLoad, out double summerLatentLoad);

            //SUMMER
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignTemperature, out double summerDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerDesignRelativeHumidity, out double summerDesignRelativeHumidity);
            if (!double.IsNaN(summerDesignTemperature) && !double.IsNaN(summerDesignRelativeHumidity))
            {
                MollierPoint start = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(summerDesignTemperature, summerDesignRelativeHumidity, pressure);

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatRecoverySensibleEfficiency, out double summerHeatRecoverySensibleEfficiency) || double.IsNaN(summerHeatRecoverySensibleEfficiency))
                {
                    summerHeatRecoverySensibleEfficiency = 0;
                }

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryLatentEfficiency, out double summerHeatRecoveryLatentEfficiency) || double.IsNaN(summerHeatRecoveryLatentEfficiency))
                {
                    summerHeatRecoveryLatentEfficiency = 0;
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceTemperature, out double summerSpaceTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceRelativeHumidty, out double summerSpaceRelativeHumidity);

                MollierPoint room_Summer = !double.IsNaN(summerSpaceTemperature) && !double.IsNaN(summerSpaceRelativeHumidity) ? Core.Mollier.Create.MollierPoint_ByRelativeHumidity(summerSpaceTemperature, summerSpaceRelativeHumidity, pressure) : null;

                //HEAT RECOVERY
                if (room_Summer != null)
                {
                    HeatRecoveryProcess heatRecoveryProcess = Core.Mollier.Create.HeatRecoveryProcess_Supply(start, room_Summer, summerHeatRecoverySensibleEfficiency, summerHeatRecoveryLatentEfficiency);
                    if (heatRecoveryProcess != null && !heatRecoveryProcess.Start.AlmostEqual(heatRecoveryProcess.End))
                    {
                        mollierGroup_Summer.Add(heatRecoveryProcess);
                        start = heatRecoveryProcess.End;
                    }
                }

                //MIXING
                if (room_Summer != null)
                {
                    double returnAirFlow = System.Math.Abs(supplyAirFlow - outsideSupplyAirFlow);
                    if (returnAirFlow > Core.Tolerance.Distance)
                    {
                        MixingProcess mixingProcess = Core.Mollier.Create.MixingProcess(start, room_Summer, supplyAirFlow, returnAirFlow);
                        if (mixingProcess != null && !mixingProcess.Start.AlmostEqual(mixingProcess.End))
                        {
                            mollierGroup_Summer.Add(mixingProcess);
                            start = mixingProcess.End;
                        }
                    }
                }

                //COOLING (COOLING COIL)
                double summerCoolingCoilOffTemperature = double.NaN;
                if(airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerCoolingCoilOffTemperature, out double summerCoolingCoilOffTemperature_Temp))
                {
                    summerCoolingCoilOffTemperature = summerCoolingCoilOffTemperature_Temp;
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.CoolingCoilContactFactor, out double coolingCoilContactFactor);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.CoolingCoilFluidReturnTemperature, out double coolingCoilFluidReturnTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.CoolingCoilFluidFlowTemperature, out double coolingCoilFluidFlowTemperature);
                if (!double.IsNaN(coolingCoilContactFactor))
                {
                    //double temperatureDifference = 0;
                    //if (supplyAirFlow == outsideSupplyAirFlow)
                    //{
                    //    UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, supplyAirFlow, summerSensibleLoad, summerLatentLoad);
                    //    temperatureDifference = System.Math.Abs(undefinedProcess.Start.DryBulbTemperature - undefinedProcess.End.DryBulbTemperature);
                    //}

                    //double dryBulbTemperature = summerSupplyTempearture - Query.PickupTemperature(summerSupplyTempearture, spf) - temperatureDifference;

                    double dryBulbTemperature = !double.IsNaN(summerCoolingCoilOffTemperature) ? summerCoolingCoilOffTemperature : room_Summer.DewPointTemperature();

                    CoolingProcess coolingProcess = Core.Mollier.Create.CoolingProcess(start, dryBulbTemperature, coolingCoilContactFactor);
                    //CoolingProcess coolingProcess = Core.Mollier.Create.CoolingProcess_ByMediumAndDryBulbTemperature(start, coolingCoilFluidFlowTemperature, coolingCoilFluidReturnTemperature, dryBulbTemperature);
                    if (coolingProcess != null && !coolingProcess.Start.AlmostEqual(coolingProcess.End))
                    {
                        mollierGroup_Summer.Add(coolingProcess);
                        start = coolingProcess.End;

                        if (!double.IsNaN(coolingProcess.Efficiency))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.CoolingCoilContactFactor, coolingProcess.Efficiency);
                        }

                        MollierPoint mollierPoint_ApparatusDewPoint = coolingProcess.ApparatusDewPoint();
                        if(mollierPoint_ApparatusDewPoint != null)
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.CoolingCoilApparatusDewPoint, mollierPoint_ApparatusDewPoint);
                        }

                        airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerCoolingCoilOffTemperature, coolingProcess.End.DryBulbTemperature);
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

                //HEATING COIL SUMMER
                if (airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatingCoil, out bool summerHeatingCoil) && summerHeatingCoil)
                {
                    UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, supplyAirFlow, summerSensibleLoad, summerLatentLoad);
                    double temperatureDifference = System.Math.Abs(undefinedProcess.Start.DryBulbTemperature - undefinedProcess.End.DryBulbTemperature);

                    temperatureDifference = summerSpaceTemperature - start.DryBulbTemperature - Core.Mollier.Query.PickupTemperature(spf) - temperatureDifference;
                    if(temperatureDifference > 0)
                    {
                        HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess_ByTemperatureDifference(start, temperatureDifference);
                        if (heatingProcess != null && !heatingProcess.Start.AlmostEqual(heatingProcess.End))
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
                }

                //airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSupplyTemperature, out double summerSupplyTemperature);

                //HEATING (FAN)
                double dryBulbTemperature_Fan = start.DryBulbTemperature + Core.Mollier.Query.PickupTemperature(spf);

                FanProcess fanProcess = Core.Mollier.Create.FanProcess_ByDryBulbTemperature(start, dryBulbTemperature_Fan);
                if (fanProcess != null && !fanProcess.Start.AlmostEqual(fanProcess.End))
                {
                    mollierGroup_Summer.Add(fanProcess);
                    start = fanProcess.End;
                }

                if (start != null)
                {
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerSupplyFanTemperature, start.DryBulbTemperature);
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerSupplyFanRelativeHumidty, start.RelativeHumidity);
                }

                if (airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerSensibleLoad, out double sensibleHeatGain) && !double.IsNaN(sensibleHeatGain))
                {

                }

                //TO ROOM

                if(!double.IsNaN(summerLatentLoad) && !double.IsNaN(summerSensibleLoad))
                {
                    RoomProcess roomProcess = Core.Mollier.Create.RoomProcess(start, supplyAirFlow, summerSensibleLoad, summerLatentLoad);
                    if (roomProcess != null && !roomProcess.Start.AlmostEqual(roomProcess.End))
                    {
                        mollierGroup_Summer.Add(roomProcess);
                        start = roomProcess.End;
                    }
                }
               
                if (room_Summer != null)
                {
                    mollierGroup_Summer.Add(room_Summer);
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

