using SAM.Core.Mollier;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Modify
    {
        public static bool UpdateMollierProcesses(this AirHandlingUnitResult airHandlingUnitResult, out List<IMollierProcess> mollierProcesses)
        {
            mollierProcesses = null;

            if (airHandlingUnitResult == null)
            {
                return false;
            }

            double pressure = Standard.Pressure;
            double spf = 1.2;

            mollierProcesses = new List<IMollierProcess>();

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
                        mollierProcesses.Add(heatingProcess);
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
                            mollierProcesses.Add(heatRecoveryProcess);
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
                            mollierProcesses.Add(mixingProcess);
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
                        mollierProcesses.Add(heatingProcess);
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
                if (room != null)
                {
                    SteamHumidificationProcess steamHumidificationProcess = Core.Mollier.Create.SteamHumidificationProcess_ByRelativeHumidity(start, room.RelativeHumidity);
                    if (steamHumidificationProcess != null)
                    {
                        mollierProcesses.Add(steamHumidificationProcess);
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
                    mollierProcesses.Add(heatingProcess_Fan);
                    start = heatingProcess_Fan.End;
                }

                if(start != null)
                {
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.WinterSupplyFanTemperature, start.DryBulbTemperature);
                    airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.WinterSupplyFanRelativeHumidty, start.RelativeHumidity);
                }

                //TO ROOM
                if (room != null)
                {
                    UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, room);
                    if (undefinedProcess != null)
                    {
                        mollierProcesses.Add(undefinedProcess);
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
                            mollierProcesses.Add(heatRecoveryProcess);
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
                            mollierProcesses.Add(mixingProcess);
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
                        double pickupTemperature = Query.PickupTemperature(start, spf);

                        double dryBulbTemperature_ADP = (coolingCoilFluidReturnTemperature + coolingCoilFluidFlowTemperature) / 2;
                        double humidityRatio_ADP = Core.Mollier.Query.HumidityRatio(dryBulbTemperature_ADP, 100, pressure);

                        MollierPoint mollierPoint_ADP = new MollierPoint(dryBulbTemperature_ADP, humidityRatio_ADP, start.Pressure);
                        
                        double dewPointTemperature = start.DewPointTemperature();

                        double dryBulbTemperature = summerSupplyTempearture - pickupTemperature;
                        double humidityRatio = dewPointTemperature < dryBulbTemperature_ADP ? start.HumidityRatio : (humidityRatio_ADP * (dryBulbTemperature - start.DryBulbTemperature) - start.HumidityRatio * (dryBulbTemperature - dryBulbTemperature_ADP)) / (dryBulbTemperature_ADP - start.DryBulbTemperature);

                        MollierPoint mollierPoint_End = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);

                        double coolingCoilContactFactor = (start.Enthalpy - mollierPoint_End.Enthalpy) / (start.Enthalpy - mollierPoint_ADP.Enthalpy);//coolingProcess.ContactFactor();
                        double coolingCoilContactFactor_2 = System.Math.Sqrt((System.Math.Pow(start.DryBulbTemperature - dryBulbTemperature, 2) + System.Math.Pow(start.HumidityRatio - humidityRatio, 2)) / (System.Math.Pow(start.DryBulbTemperature - dryBulbTemperature_ADP, 2) + System.Math.Pow(start.HumidityRatio - humidityRatio_ADP, 2)));

                        CoolingProcess coolingProcess =  Core.Mollier.Create.CoolingProcess(start, dryBulbTemperature_ADP);
                        coolingProcess.Scale(coolingCoilContactFactor);
                        if (coolingProcess != null)
                        {
                            mollierProcesses.Add(coolingProcess);
                            start = coolingProcess.End;
                        }

                        double coolingCoilContactFactor_3 = coolingProcess.ContactFactor();

                        if (!double.IsNaN(coolingCoilContactFactor))
                        {
                            airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.CoolingCoilContactFactor, coolingCoilContactFactor);
                        }

                        if(!double.IsNaN(supplyAirFlow))
                        {
                            double coolingCoilSensibleLoad = Core.Mollier.Query.SensibleLoad(coolingProcess, supplyAirFlow);
                            if(!double.IsNaN(coolingCoilSensibleLoad))
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

                    if(airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.SummerHeatingCoil, out bool summerHeatingCoil) && summerHeatingCoil)
                    {
                        double temperatureDifference = start.DryBulbTemperature - summerSpaceTemperature;
                        HeatingProcess heatingProcess = Core.Mollier.Create.HeatingProcess_ByTemperatureDifference(start, temperatureDifference);
                        if (heatingProcess != null)
                        {
                            mollierProcesses.Add(heatingProcess);
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
                    double dryBulbTemperature_Fan = start.DryBulbTemperature + Query.PickupTemperature(start, spf);

                    HeatingProcess heatingProcess_Fan = Core.Mollier.Create.HeatingProcess(start, dryBulbTemperature_Fan);
                    if (heatingProcess_Fan != null)
                    {
                        mollierProcesses.Add(heatingProcess_Fan);
                        start = heatingProcess_Fan.End;
                    }

                    if (start != null)
                    {
                        airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerSupplyFanTemperature, start.DryBulbTemperature);
                        airHandlingUnitResult.SetValue(AirHandlingUnitResultParameter.SummerSupplyFanRelativeHumidty, start.RelativeHumidity);
                    }

                    //TO ROOM
                    if (room != null)
                    {
                        UndefinedProcess undefinedProcess = Core.Mollier.Create.UndefinedProcess(start, room);
                        if (undefinedProcess != null)
                        {
                            mollierProcesses.Add(undefinedProcess);
                            start = undefinedProcess.End;
                        }
                    }
                }
            }

            return true;
        }
    }
}

