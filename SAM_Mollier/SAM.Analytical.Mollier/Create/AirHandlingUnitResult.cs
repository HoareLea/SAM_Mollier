using SAM.Core.Mollier;
using SAM.Weather;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAM.Analytical.Mollier
{
    public static partial class Create
    {
        public static AirHandlingUnitResult AirHandlingUnitResult(this AnalyticalModel analyticalModel, string airHandlingUnitName)
        {
            return AirHandlingUnitResult(analyticalModel, airHandlingUnitName, null);
        }

        public static AirHandlingUnitResult AirHandlingUnitResult(this AnalyticalModel analyticalModel, string airHandlingUnitName, Space space)
        {
            if (analyticalModel == null || string.IsNullOrEmpty(airHandlingUnitName))
            {
                return null;
            }

            if (space == null)
            {
                return AirHandlingUnitResult(analyticalModel, airHandlingUnitName, null, null);
            }

            AirHandlingUnitCalculationMethod airHandlingUnitCalculationMethod = AirHandlingUnitCalculationMethod.FixedSupplyTemperature;
            if (space.TryGetValue(SpaceParameter.AirHandlingUnitCalculationMethod, out string airHandlingUnitCalculationMethodString) && !string.IsNullOrEmpty(airHandlingUnitCalculationMethodString))
            {
                AirHandlingUnitCalculationMethod airHandlingUnitCalculationMethod_Temp = Core.Query.Enum<AirHandlingUnitCalculationMethod>(airHandlingUnitCalculationMethodString);
                if (airHandlingUnitCalculationMethod_Temp != AirHandlingUnitCalculationMethod.Undefined)
                {
                    airHandlingUnitCalculationMethod = airHandlingUnitCalculationMethod_Temp;
                }
            }

            if (airHandlingUnitCalculationMethod == AirHandlingUnitCalculationMethod.Undefined)
            {
                airHandlingUnitCalculationMethod = AirHandlingUnitCalculationMethod.FixedSupplyTemperature;
            }

            AirHandlingUnitResult result = AirHandlingUnitResult(analyticalModel, airHandlingUnitName, space, null);

            Func<double, double> func = new Func<double, double>((double summerSupplyTemperature_Temp) => 
            {
                AirHandlingUnitResult airHandlingUnitResult = AirHandlingUnitResult(analyticalModel, airHandlingUnitName, space, summerSupplyTemperature_Temp);
                if(airHandlingUnitResult == null)
                {
                    return double.NaN;
                }

                if (airHandlingUnitCalculationMethod == AirHandlingUnitCalculationMethod.HumidityRatioAndChilledWaterTemperature)
                {
                    double summerSupplyFanTemperature = airHandlingUnitResult.SummerSupplyFanTemperature();
                    if(!double.IsNaN(summerSupplyFanTemperature))
                    {
                        MollierPoint mollierPoint_ApparatusDewPoint = airHandlingUnitResult.ApparatusDewPoint();
                        if (mollierPoint_ApparatusDewPoint != null)
                        {
                            if(mollierPoint_ApparatusDewPoint.DryBulbTemperature > summerSupplyFanTemperature)
                            {
                                return double.NaN;
                            }
                        }
                    }
                }

                double supplyFanRelativeHumidity = airHandlingUnitResult.SummerSupplyFanRelativeHumidity();

                return supplyFanRelativeHumidity;
            });

            double summerSupplyTemperature= double.NaN;

            switch (airHandlingUnitCalculationMethod)
            {
                case AirHandlingUnitCalculationMethod.HumidityRatio:
                    summerSupplyTemperature = Core.Query.Calculate_ByDivision(func, result.SummerSpaceRelativeHumidity(), 0, 50);
                    if (!double.IsNaN(summerSupplyTemperature))
                    {
                        return AirHandlingUnitResult(analyticalModel, airHandlingUnitName, space, summerSupplyTemperature);
                    }
                    break;

                case AirHandlingUnitCalculationMethod.HumidityRatioAndChilledWaterTemperature:
                    summerSupplyTemperature = Core.Query.Calculate_ByDivision(func, result.SummerSpaceRelativeHumidity(), 0, 50);
                    if (!double.IsNaN(summerSupplyTemperature))
                    {
                        return AirHandlingUnitResult(analyticalModel, airHandlingUnitName, space, summerSupplyTemperature);
                    }
                    break;
            }

            return result;
        }

        public static AirHandlingUnitResult AirHandlingUnitResult(this AnalyticalModel analyticalModel, string airHandlingUnitName, Space space, double? summerSupplyTemperature)
        {
            AdjacencyCluster adjacencyCluster = analyticalModel?.AdjacencyCluster;
            if (adjacencyCluster == null || string.IsNullOrWhiteSpace(airHandlingUnitName))
            {
                return null;
            }

            AirHandlingUnit airHandlingUnit = adjacencyCluster.GetObject((AirHandlingUnit x) => x.Name == airHandlingUnitName);
            if(airHandlingUnit == null)
            {
                return null;
            }

            AirHandlingUnitResult result = new AirHandlingUnitResult(airHandlingUnitName, Query.Source(), airHandlingUnit.Guid.ToString());

            Analytical.Query.Spaces(adjacencyCluster, airHandlingUnitName, out List<Space> spaces_Supply, out List<Space> spaces_Exhaust);
            if(space != null)
            {
                spaces_Supply?.RemoveAll(x => x.Guid != space.Guid);
                spaces_Exhaust?.RemoveAll(x => x.Guid != space.Guid);
            }

            double sensibleHeatLoss = 0;
            double sensibleHeatGain = 0;
            double outsideSupplyAirFlow = 0;
            double supplyAirFlow = 0;
            List<double> coolingDesignTemperatures = new List<double>();
            List<double> heatingDesignTemperatures = new List<double>();
            List<double> coolingDesignRelativeHumidities = new List<double>();
            List<double> heatingDesignRelativeHumidities = new List<double>();
            if (spaces_Supply != null && spaces_Supply.Count != 0)
            {
                foreach (Space space_Supply in spaces_Supply)
                {
                    List<SpaceSimulationResult> spaceSimulationResults = adjacencyCluster.GetResults<SpaceSimulationResult>(space_Supply);
                    if (spaceSimulationResults != null && spaceSimulationResults.Count != 0)
                    {
                        foreach (SpaceSimulationResult spaceSimulationResult in spaceSimulationResults)
                        {
                            double designLoad = double.NaN;

                            LoadType loadType = spaceSimulationResult.LoadType();
                            switch (loadType)
                            {
                                case LoadType.Heating:
                                    if (spaceSimulationResult.TryGetValue(SpaceSimulationResultParameter.DesignLoad, out designLoad) && !double.IsNaN(designLoad))
                                    {
                                        sensibleHeatLoss += designLoad;
                                    }
                                    break;

                                case LoadType.Cooling:
                                    if (spaceSimulationResult.TryGetValue(SpaceSimulationResultParameter.DesignLoad, out designLoad) && !double.IsNaN(designLoad))
                                    {
                                        sensibleHeatGain += designLoad;
                                    }
                                    break;

                            }
                        }
                    }
                    else
                    {
                        if(space_Supply.TryGetValue(Analytical.SpaceParameter.DesignCoolingLoad, out double designCoolingLoad) && !double.IsNaN(designCoolingLoad))
                        {
                            sensibleHeatGain += designCoolingLoad;
                        }

                        if (space_Supply.TryGetValue(Analytical.SpaceParameter.DesignHeatingLoad, out double designHeatingLoad) && !double.IsNaN(designHeatingLoad))
                        {
                            sensibleHeatLoss += designHeatingLoad;
                        }
                    }

                    double supplyAirFlow_Space = space_Supply.CalculatedSupplyAirFlow();
                    if (!double.IsNaN(supplyAirFlow_Space))
                    {
                        outsideSupplyAirFlow += supplyAirFlow_Space;

                        supplyAirFlow_Space = adjacencyCluster.CalculatedSupplyAirFlow(space_Supply);
                        if(!double.IsNaN(supplyAirFlow_Space))
                        {
                            supplyAirFlow += supplyAirFlow_Space;
                        }
                    }

                    coolingDesignTemperatures.Add(Analytical.Query.CoolingDesignTemperature(space_Supply, analyticalModel?.ProfileLibrary));
                    heatingDesignTemperatures.Add(Analytical.Query.HeatingDesignTemperature(space_Supply, analyticalModel?.ProfileLibrary));

                    coolingDesignRelativeHumidities.Add(Analytical.Query.CoolingDesignRelativeHumidity(space_Supply, analyticalModel?.ProfileLibrary));
                    heatingDesignRelativeHumidities.Add(Analytical.Query.HeatingDesignRelativeHumidity(space_Supply, analyticalModel?.ProfileLibrary));
                }
            }

            double exhaustAirFlow = 0;
            if(spaces_Exhaust != null && spaces_Exhaust.Count != 0)
            {
                foreach (Space space_Exhaust in spaces_Exhaust)
                {
                    double exhaustAirFlow_Space = space_Exhaust.CalculatedExhaustAirFlow();
                    exhaustAirFlow_Space = double.IsNaN(exhaustAirFlow_Space) ? double.MinValue: exhaustAirFlow_Space;

                    double supplyAirFlow_Space = adjacencyCluster.CalculatedSupplyAirFlow(space_Exhaust);
                    supplyAirFlow_Space = double.IsNaN(supplyAirFlow_Space) ? double.MinValue : supplyAirFlow_Space;

                    exhaustAirFlow_Space = System.Math.Max(exhaustAirFlow_Space, supplyAirFlow_Space);
                    if (exhaustAirFlow_Space != double.MinValue)
                    {
                        exhaustAirFlow += exhaustAirFlow_Space;
                    }
                }
            }

            double pressure = Standard.Pressure;

            double summerDesignTemperature = double.NaN;
            double summerDesignRelativeHumidity = double.NaN;
            string summerDesignDayName = null;
            int summerDesignDayIndex = -1;

            double winterDesignTemperature = double.NaN;
            double winterDesignRelativeHumidity = double.NaN;
            string winterDesignDayName = null;
            int winterDesignDayIndex = -1;

            double enthalpy_Max = double.NaN;
            double enthalpy_Min = double.NaN;

            List<DesignDay> designDays = adjacencyCluster.GetObjects<DesignDay>();
            if(designDays != null && designDays.Count != 0)
            {
                foreach (DesignDay designDay in designDays)
                {
                    if(!designDay.Contains(WeatherDataType.DryBulbTemperature)  || !designDay.Contains(WeatherDataType.RelativeHumidity))
                    {
                        continue;
                    }

                    for(int i = 0; i < 24; i++)
                    {
                        double dryBulbTemperature = designDay[WeatherDataType.DryBulbTemperature, i];
                        double relativeHumidity = designDay[WeatherDataType.RelativeHumidity, i];

                        double enthalpy = Core.Mollier.Query.Enthalpy_ByRelativeHumidity(dryBulbTemperature, relativeHumidity, pressure);
                        if(double.IsNaN(enthalpy))
                        {
                            continue;
                        }

                        if(double.IsNaN(enthalpy_Max) || enthalpy > enthalpy_Max)
                        {
                            summerDesignTemperature = dryBulbTemperature;
                            summerDesignRelativeHumidity = relativeHumidity;
                            summerDesignDayName = designDay.Name;
                            summerDesignDayIndex = i;
                            enthalpy_Max = enthalpy;
                        }

                        if (double.IsNaN(enthalpy_Min) || enthalpy < enthalpy_Min)
                        {
                            winterDesignTemperature = dryBulbTemperature;
                            winterDesignRelativeHumidity = relativeHumidity;
                            winterDesignDayName = designDay.Name;
                            winterDesignDayIndex = i;
                            enthalpy_Min = enthalpy;
                        }
                    }
                }
            }
            
            if(double.IsNaN(summerDesignTemperature))
            {
                summerDesignTemperature = 32.1;
            }

            if (double.IsNaN(summerDesignRelativeHumidity))
            {
                summerDesignRelativeHumidity = 35.9;
            }

            if (double.IsNaN(winterDesignTemperature))
            {
                winterDesignTemperature = -3;
            }

            if (double.IsNaN(winterDesignRelativeHumidity))
            {
                winterDesignRelativeHumidity = 86.9;
            }

            result.SetValue(AirHandlingUnitResultParameter.SensibleHeatGain, sensibleHeatGain);
            result.SetValue(AirHandlingUnitResultParameter.SensibleHeatLoss, sensibleHeatLoss);
            result.SetValue(AirHandlingUnitResultParameter.SummerDesignTemperature, summerDesignTemperature);
            result.SetValue(AirHandlingUnitResultParameter.SummerDesignRelativeHumidity, summerDesignRelativeHumidity);
            result.SetValue(AirHandlingUnitResultParameter.WinterDesignTemperature, winterDesignTemperature);
            result.SetValue(AirHandlingUnitResultParameter.WinterDesignRelativeHumidity, winterDesignRelativeHumidity);
            result.SetValue(AirHandlingUnitResultParameter.SupplyAirFlow, supplyAirFlow);
            result.SetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, outsideSupplyAirFlow);
            result.SetValue(AirHandlingUnitResultParameter.ExhaustAirFlow, exhaustAirFlow);

            AirSupplyMethod airSupplyMethod = Analytical.Query.AirSupplyMethod(adjacencyCluster, airHandlingUnitName);

            double winterSupplyTemperature = airHandlingUnit.WinterSupplyTemperature;
            if (double.IsNaN(winterSupplyTemperature))
            {
                if (heatingDesignTemperatures != null && heatingDesignTemperatures.Count != 0)
                {
                    winterSupplyTemperature = heatingDesignTemperatures.Min();
                    if(airSupplyMethod == AirSupplyMethod.Total)
                    {
                        winterSupplyTemperature += sensibleHeatLoss / (supplyAirFlow * 1.2 * 1.005) / 1000;
                    }
                }
            }

            if (!double.IsNaN(winterSupplyTemperature))
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterSupplyTemperature, winterSupplyTemperature);
            }

            bool summerHeatingCoil = false;
            double winterHeatingCoilSupplyTemperature = double.NaN;

            HeatingCoil heatingCoil = airHandlingUnit.GetSimpleEquipments<HeatingCoil>(FlowClassification.Supply)?.FirstOrDefault();
            if (heatingCoil != null)
            {
                summerHeatingCoil = heatingCoil.Summer;
                winterHeatingCoilSupplyTemperature = heatingCoil.WinterOffTemperature;
            }

            double summerSupplyTemperature_Temp = double.NaN;
            if(summerSupplyTemperature != null && summerSupplyTemperature.HasValue && !double.IsNaN(summerSupplyTemperature.Value))
            {
                summerSupplyTemperature_Temp = summerSupplyTemperature.Value;
            }
            else
            {
                summerSupplyTemperature_Temp = airHandlingUnit.SummerSupplyTemperature;
            }

            if(!double.IsNaN(summerSupplyTemperature_Temp))
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerSupplyTemperature, summerSupplyTemperature_Temp);
            }

            HeatingCoil frostCoil = airHandlingUnit.GetSimpleEquipments<HeatingCoil>(FlowClassification.Intake)?.FirstOrDefault();
            if(frostCoil != null && !double.IsNaN(frostCoil.WinterOffTemperature))
            {
                result.SetValue(AirHandlingUnitResultParameter.FrostCoilOffTemperature, frostCoil.WinterOffTemperature);
            }

            double winterHeatRecoveryDryBulbTemperature = double.NaN;
            double winterHeatRecoveryRelativeHumidity = double.NaN;
            double summerHeatRecoveryDryBulbTemperature = double.NaN;
            double summerHeatRecoveryRelativeHumidity = double.NaN;

            HeatRecoveryUnit heatRecoveryUnit = airHandlingUnit.GetSimpleEquipments<HeatRecoveryUnit>(FlowClassification.Supply)?.FirstOrDefault();
            if(heatRecoveryUnit != null)
            {
                if(!double.IsNaN(heatRecoveryUnit.WinterSensibleEfficiency))
                {
                    result.SetValue(AirHandlingUnitResultParameter.WinterHeatRecoverySensibleEfficiency, heatRecoveryUnit.WinterSensibleEfficiency);
                }

                if (!double.IsNaN(heatRecoveryUnit.WinterLatentEfficiency))
                {
                    result.SetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryLatentEfficiency, heatRecoveryUnit.WinterLatentEfficiency);
                }

                if (!double.IsNaN(heatRecoveryUnit.SummerSensibleEfficiency))
                {
                    result.SetValue(AirHandlingUnitResultParameter.SummerHeatRecoverySensibleEfficiency, heatRecoveryUnit.SummerSensibleEfficiency);
                }

                if (!double.IsNaN(heatRecoveryUnit.SummerLatentEfficiency))
                {
                    result.SetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryLatentEfficiency, heatRecoveryUnit.SummerLatentEfficiency);
                }

                if (!double.IsNaN(heatRecoveryUnit.WinterDryBulbTemperature))
                {
                    winterHeatRecoveryDryBulbTemperature = heatRecoveryUnit.WinterDryBulbTemperature;
                }

                if (!double.IsNaN(heatRecoveryUnit.WinterRelativeHumidity))
                {
                    winterHeatRecoveryRelativeHumidity = heatRecoveryUnit.WinterRelativeHumidity;
                }

                if (!double.IsNaN(heatRecoveryUnit.SummerDryBulbTemperature))
                {
                    summerHeatRecoveryDryBulbTemperature = heatRecoveryUnit.SummerDryBulbTemperature;
                }

                if (!double.IsNaN(heatRecoveryUnit.SummerRelativeHumidity))
                {
                    summerHeatRecoveryRelativeHumidity = heatRecoveryUnit.SummerRelativeHumidity;
                }
            }

            double coolingCoilFluidReturnTemperature = double.NaN;

            CoolingCoil coolingCoil = airHandlingUnit.GetSimpleEquipments<CoolingCoil>(FlowClassification.Supply)?.FirstOrDefault();
            if(coolingCoil != null)
            {
                if (!double.IsNaN(coolingCoil.ContactFactor))
                {
                    result.SetValue(AirHandlingUnitResultParameter.CoolingCoilContactFactor, coolingCoil.ContactFactor);
                }

                if (!double.IsNaN(coolingCoil.FluidSupplyTemperature))
                {
                    result.SetValue(AirHandlingUnitResultParameter.CoolingCoilFluidFlowTemperature, coolingCoil.FluidSupplyTemperature);
                }

                if (!double.IsNaN(coolingCoil.FluidReturnTemperature))
                {
                    result.SetValue(AirHandlingUnitResultParameter.CoolingCoilFluidReturnTemperature, coolingCoil.FluidReturnTemperature);
                    coolingCoilFluidReturnTemperature = coolingCoil.FluidReturnTemperature;
                }
            }

            if (!double.IsNaN(winterHeatingCoilSupplyTemperature) && !double.IsNaN(coolingCoilFluidReturnTemperature))
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterHeatingCoilSupplyTemperature, winterHeatingCoilSupplyTemperature);
            }
            else
            {
                double heatingDesignTemperature = double.NaN;
                if (heatingDesignTemperatures != null && heatingDesignTemperatures.Count != 0)
                {
                    heatingDesignTemperature = heatingDesignTemperatures.Min();
                    result.SetValue(AirHandlingUnitResultParameter.WinterSpaceTemperature, heatingDesignTemperature);
                }

                winterHeatingCoilSupplyTemperature = heatingDesignTemperature;
                if (airSupplyMethod == AirSupplyMethod.Total)
                {
                    winterHeatingCoilSupplyTemperature += sensibleHeatLoss / (supplyAirFlow * 1.2 * 1.005) / 1000;
                }

                if (!double.IsNaN(winterHeatingCoilSupplyTemperature))
                {
                    result.SetValue(AirHandlingUnitResultParameter.WinterHeatingCoilSupplyTemperature, winterHeatingCoilSupplyTemperature);
                }
            }

            if (coolingDesignTemperatures != null && coolingDesignTemperatures.Count != 0)
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerSpaceTemperature, coolingDesignTemperatures.Max());
            }

            if (heatingDesignRelativeHumidities != null && heatingDesignRelativeHumidities.Count != 0)
            {
                double value = heatingDesignRelativeHumidities.Min();
                if(Core.Query.AlmostEqual(value, 100))
                {
                    value = 20;
                }

                result.SetValue(AirHandlingUnitResultParameter.WinterSpaceRelativeHumidty, value);
            }

            if (coolingDesignRelativeHumidities != null && coolingDesignRelativeHumidities.Count != 0)
            {
                double value = coolingDesignRelativeHumidities.Max();
                if (Core.Query.AlmostEqual(value, 0))
                {
                    value = 70;
                }

                result.SetValue(AirHandlingUnitResultParameter.SummerSpaceRelativeHumidty, value);
            }

            if (!string.IsNullOrWhiteSpace(summerDesignDayName))
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerDesignDayName, summerDesignDayName);
            }

            if (!string.IsNullOrWhiteSpace(winterDesignDayName))
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterDesignDayName, winterDesignDayName);
            }

            if (summerDesignDayIndex != -1)
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerDesignDayIndex, summerDesignDayIndex);
            }

            if (winterDesignDayIndex != -1)
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterDesignDayIndex, winterDesignDayIndex);
            }

            if (!double.IsNaN(winterHeatRecoveryDryBulbTemperature))
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryDryBulbTemperature, winterHeatRecoveryDryBulbTemperature);
            }
            else if (result.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceTemperature, out double winterSpaceTemperature))
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryDryBulbTemperature, winterSpaceTemperature - 1);
            }

            if (double.IsNaN(winterHeatRecoveryRelativeHumidity))
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryRelativeHumidity, winterHeatRecoveryRelativeHumidity);
            }
            else if (result.TryGetValue(AirHandlingUnitResultParameter.WinterSpaceRelativeHumidty, out double winterSpaceRelativeHumidity))
            {
                result.SetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryRelativeHumidity, winterSpaceRelativeHumidity);
            }

            if (!double.IsNaN(summerHeatRecoveryDryBulbTemperature))
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryDryBulbTemperature, summerHeatRecoveryDryBulbTemperature);
            }
            else if (result.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceTemperature, out double summerSpaceTemperature))
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryDryBulbTemperature, summerSpaceTemperature + 1);
            }

            if (!double.IsNaN(summerHeatRecoveryRelativeHumidity))
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryRelativeHumidity, summerHeatRecoveryRelativeHumidity);
            }
            else if(result.TryGetValue(AirHandlingUnitResultParameter.SummerSpaceRelativeHumidty, out double summerSpaceRelativeHumidity))
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerHeatRecoveryRelativeHumidity, summerSpaceRelativeHumidity);
            }

            result.UpdateProcesses();

            return result;
        }
    }
}
