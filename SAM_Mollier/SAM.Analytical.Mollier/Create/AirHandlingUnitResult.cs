﻿

using SAM.Weather;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Create
    {
        public static AirHandlingUnitResult AirHandlingUnitResult(this AdjacencyCluster adjacencyCluster, string airHandlingUnitName, out AirHandlingUnit airHandlingUnit)
        {
            airHandlingUnit = null;

            if (adjacencyCluster == null || string.IsNullOrWhiteSpace(airHandlingUnitName))
            {
                return null;
            }

            airHandlingUnit = adjacencyCluster.GetObject((AirHandlingUnit x) => x.Name == airHandlingUnitName);
            if(airHandlingUnit == null)
            {
                airHandlingUnit = Analytical.Create.AirHandlingUnit(airHandlingUnitName);
                adjacencyCluster.AddObject(airHandlingUnit);
            }

            AirHandlingUnitResult result = new AirHandlingUnitResult(airHandlingUnitName, Query.Source(), airHandlingUnit.Guid.ToString());

            List<Space> spaces = Analytical.Query.Spaces(adjacencyCluster, airHandlingUnitName, out List<Space> spaces_Supply, out List<Space> spaces_Exhaust);

            double sensibleHeatLoss = 0;
            double sensibleHeatGain = 0;
            double outsideSupplyAirFlow = 0;
            double supplyAirFlow = 0;
            if (spaces_Supply != null && spaces_Supply.Count != 0)
            {
                foreach (Space space in spaces_Supply)
                {
                    List<SpaceSimulationResult> spaceSimulationResults = adjacencyCluster.GetResults<SpaceSimulationResult>(space);
                    if (spaceSimulationResults != null)
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

                    double supplyAirFlow_Space = space.CalculatedSupplyAirFlow();
                    if (!double.IsNaN(supplyAirFlow_Space))
                    {
                        outsideSupplyAirFlow += supplyAirFlow_Space;
                    }

                    List<VentilationSystem> ventilationSystems = adjacencyCluster.GetRelatedObjects<VentilationSystem>(space);
                    if (ventilationSystems != null && ventilationSystems.Count != 0)
                    {
                        VentilationSystem ventilationSystem = ventilationSystems.Find(x => x.Type != null);
                        VentilationSystemType ventilationSystemType = ventilationSystem.Type as VentilationSystemType;
                        if (ventilationSystemType.TryGetValue(VentilationSystemTypeParameter.AirSupplyMethod, out string airSupplyMethodString) && !string.IsNullOrWhiteSpace(airSupplyMethodString))
                        {
                            AirSupplyMethod airSupplyMethod = Core.Query.Enum<AirSupplyMethod>(airSupplyMethodString);
                            switch (airSupplyMethod)
                            {
                                case AirSupplyMethod.Outside:
                                    supplyAirFlow += supplyAirFlow_Space;
                                    break;

                                case AirSupplyMethod.Total:
                                    if (space.TryGetValue(SpaceParameter.DesignHeatingLoad, out double designHeatingLoad) && space.TryGetValue(SpaceParameter.DesignCoolingLoad, out double designCoolingLoad))
                                    {
                                        if (ventilationSystemType.TryGetValue(VentilationSystemTypeParameter.TemperatureDifference, out double temperatureDifference))
                                        {
                                            double supplyAirFlow_Load = System.Math.Max(designCoolingLoad, designHeatingLoad) / 1.2 / 1.005 / temperatureDifference;

                                            supplyAirFlow += System.Math.Max(supplyAirFlow_Space, supplyAirFlow_Load);
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }
            }

            double exhaustAirFlow = 0;
            if(spaces_Exhaust != null && spaces_Exhaust.Count != 0)
            {
                foreach (Space space in spaces_Exhaust)
                {
                    double exhaustAirFlow_Space = space.CalculatedExhaustAirFlow();
                    exhaustAirFlow_Space = double.IsNaN(exhaustAirFlow_Space) ? double.MinValue: exhaustAirFlow_Space;

                    double supplyAirFlow_Space = space.CalculatedSupplyAirFlow();
                    supplyAirFlow_Space = double.IsNaN(supplyAirFlow_Space) ? double.MinValue : supplyAirFlow_Space;

                    exhaustAirFlow_Space = System.Math.Max(exhaustAirFlow_Space, supplyAirFlow_Space);
                    if (exhaustAirFlow_Space != double.MinValue)
                    {
                        exhaustAirFlow += exhaustAirFlow_Space;
                    }
                }
            }

            double pressure = 101325;

            double summerDesignTemperature = double.NaN;
            double summerDesignRelativeHumidity = double.NaN;
            string summerDesignDayName = null;
            int summerDesignDayIndex = -1;

            double enthalpy_Max = double.NaN;

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


            result.SetValue(AirHandlingUnitResultParameter.SensibleHeatGain, sensibleHeatGain);
            result.SetValue(AirHandlingUnitResultParameter.SensibleHeatLoss, sensibleHeatLoss);
            result.SetValue(AirHandlingUnitResultParameter.SummerDesignTemperature, summerDesignTemperature);
            result.SetValue(AirHandlingUnitResultParameter.SummerDesignRelativeHumidity, summerDesignRelativeHumidity);
            result.SetValue(AirHandlingUnitResultParameter.SupplyAirFlow, supplyAirFlow);
            result.SetValue(AirHandlingUnitResultParameter.OutsideSupplyAirFlow, outsideSupplyAirFlow);
            result.SetValue(AirHandlingUnitResultParameter.ExhaustAirFlow, exhaustAirFlow);
            if (!string.IsNullOrWhiteSpace(summerDesignDayName))
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerDesignDayName, summerDesignDayName);
            }

            if(summerDesignDayIndex != -1)
            {
                result.SetValue(AirHandlingUnitResultParameter.SummerDesignDayIndex, summerDesignDayIndex);
            }

            adjacencyCluster.AddObject(result);
            adjacencyCluster.AddRelation(airHandlingUnit, result);

            return result;
        }
    }
}
