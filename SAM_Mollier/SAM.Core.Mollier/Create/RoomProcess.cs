namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static RoomProcess RoomProcess(this MollierPoint start, MollierPoint stop, double efficiency = 1)
        {
            if (start == null || stop == null)
            {
                return null;
            }

            return new RoomProcess(start, stop, efficiency);
        }

        /// <summary>
        /// Returns new RoomProcess based on given input
        /// </summary>
        /// <param name="start">Start MollierPoint</param>
        /// <param name="latentLoad">Latent Load [W]</param>
        /// <param name="airMassFlow">Air Mass Flow [kg/s]</param>
        /// <param name="sensibleLoad">Sensible Load [W]</param>
        /// <returns>RoomProcess</returns>
        public static RoomProcess RoomProcess(this MollierPoint start, double airMassFlow, double sensibleLoad, double latentLoad, double efficiency = 1)
        {
            if(start == null || !start.IsValid())
            {
                return null;
            }

            if (double.IsNaN(airMassFlow) || double.IsNaN(sensibleLoad) || double.IsNaN(latentLoad))
            {
                return null;
            }

            double dryBulbTemperature = start.DryBulbTemperature + ((sensibleLoad / 1000) / (airMassFlow * Query.SpecificHeatCapacity_Air(start)));
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }


            double humidityRatio = Query.HumidityRatio(start, latentLoad, airMassFlow);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }


            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);
            if(!end.IsValid())
            {
                return null;
            }

            return RoomProcess(start, end, efficiency);

        }

        /// <summary>
        /// Returns new RoomProcess based on given input
        /// </summary>
        /// <param name="end">End MollierPoint</param>
        /// <param name="latentLoad">Latent Load [W]</param>
        /// <param name="airMassFlow">Air Mass Flow [kg/s]</param>
        /// <param name="sensibleLoad">Sensible Load [W]</param>
        /// <returns>RoomProcess</returns>
        public static RoomProcess RoomProcess_ByEnd(this MollierPoint end, double airMassFlow, double sensibleLoad, double latentLoad, double efficiency = 1)
        {
            if (end == null || !end.IsValid())
            {
                return null;
            }
            if (double.IsNaN(airMassFlow) || double.IsNaN(sensibleLoad) || double.IsNaN(latentLoad))
            {
                return null;
            }

            double dryBulbTemperature = end.DryBulbTemperature - ((sensibleLoad / 1000) / (airMassFlow * Query.SpecificHeatCapacity_Air(end)));
            if (double.IsNaN(dryBulbTemperature))
            {
                return null;
            }


            double humidityRatio = Query.HumidityRatio_ByEnd(end, latentLoad, airMassFlow);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }

            MollierPoint start = new MollierPoint(dryBulbTemperature, humidityRatio, end.Pressure);

            if(!start.IsValid())
            {
                return null;
            }

            return new RoomProcess(start, end, efficiency);
        }
        
        public static RoomProcess RoomProcess_BySensibleHeatRatio(this MollierPoint mollierPoint, double sensibleHeatRatio, double dryBulbTemperature_Start, double dryBulbTemperature_End, double humidityRatio_Start, double humidityRatio_End, double efficiency = 1)
        {
            if(mollierPoint == null || double.IsNaN(sensibleHeatRatio) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(dryBulbTemperature_End))
            {
                return null;
            }

            if(sensibleHeatRatio == 0)
            {
                return new RoomProcess(new MollierPoint(mollierPoint.DryBulbTemperature, humidityRatio_Start, mollierPoint.Pressure), new MollierPoint(mollierPoint.DryBulbTemperature, humidityRatio_End, mollierPoint.Pressure));
            }

            //START

            double specificHeat_Start = Zero.SpecificHeat_Air;//Query.SpecificHeat_Air(dryBulbTemperature_Start);

            MollierPoint mollierPoint_Start_DryBulbTemperature = null;
            if (sensibleHeatRatio != 0)
            {
                double enthalpy = Query.Enthalpy_BySensibleHeatRatioAndDryBulbTemperature(mollierPoint.Pressure, sensibleHeatRatio, specificHeat_Start, mollierPoint, dryBulbTemperature_Start);
                if (!double.IsNaN(enthalpy))
                {
                    double humidityRatio = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_Start, enthalpy);
                    if (!double.IsNaN(humidityRatio))
                    {
                        mollierPoint_Start_DryBulbTemperature = new MollierPoint(dryBulbTemperature_Start, humidityRatio, mollierPoint.Pressure);
                    }
                }
            }

            MollierPoint mollierPoint_Start_HumidityRatio = null;
            if (sensibleHeatRatio != 1)
            {
                double dryBulbTemperature = Query.DryBulbTemperature_BySensibleHeatRatioAndHumidityRatio(sensibleHeatRatio, specificHeat_Start, mollierPoint, humidityRatio_Start);
                if (!double.IsNaN(dryBulbTemperature))
                {
                    mollierPoint_Start_HumidityRatio = new MollierPoint(dryBulbTemperature, humidityRatio_Start, mollierPoint.Pressure);
                }
            }

            if(mollierPoint_Start_DryBulbTemperature == null && mollierPoint_Start_HumidityRatio == null)
            {
                return null;
            }

            MollierPoint mollierPoint_Start = null;
            if(mollierPoint_Start_DryBulbTemperature == null)
            {
                mollierPoint_Start = mollierPoint_Start_HumidityRatio;
            } 
            else if(mollierPoint_Start_HumidityRatio == null)
            {
                mollierPoint_Start = mollierPoint_Start_DryBulbTemperature;
            }
            else
            {
                double enthalpyDifference_HumidityRatio = System.Math.Abs(mollierPoint.Enthalpy - mollierPoint_Start_HumidityRatio.Enthalpy);
                double enthalpyDifference_DryBulbTemperature = System.Math.Abs(mollierPoint.Enthalpy - mollierPoint_Start_DryBulbTemperature.Enthalpy);
                
                mollierPoint_Start = enthalpyDifference_HumidityRatio < enthalpyDifference_DryBulbTemperature ? mollierPoint_Start_HumidityRatio : mollierPoint_Start_DryBulbTemperature;
            }

            if(mollierPoint_Start == null)
            {
                return null;
            }

            //END

            double specificHeat_End = Zero.SpecificHeat_Air;//Query.SpecificHeat_Air(dryBulbTemperature_End);

            MollierPoint mollierPoint_End_DryBulbTemperature = null;
            if (sensibleHeatRatio != 0)
            {
                double enthaply = Query.Enthalpy_BySensibleHeatRatioAndDryBulbTemperature(mollierPoint.Pressure, sensibleHeatRatio, specificHeat_End, mollierPoint, dryBulbTemperature_End);
                if (!double.IsNaN(enthaply))
                {
                    double humidityRatio = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_End, enthaply);
                    if (!double.IsNaN(humidityRatio))
                    {
                        mollierPoint_End_DryBulbTemperature = new MollierPoint(dryBulbTemperature_End, humidityRatio, mollierPoint.Pressure);
                    }
                }
            }

            MollierPoint mollierPoint_End_HumidityRatio = null;
            if (sensibleHeatRatio != 1)
            {
                double dryBulbTemperature = Query.DryBulbTemperature_BySensibleHeatRatioAndHumidityRatio(sensibleHeatRatio, specificHeat_End, mollierPoint, humidityRatio_End);
                if (!double.IsNaN(dryBulbTemperature))
                {
                    mollierPoint_End_HumidityRatio = new MollierPoint(dryBulbTemperature, humidityRatio_End, mollierPoint.Pressure);
                }
            }

            if (mollierPoint_End_DryBulbTemperature == null && mollierPoint_End_HumidityRatio == null)
            {
                return null;
            }

            MollierPoint mollierPoint_End = null;
            if (mollierPoint_End_DryBulbTemperature == null)
            {
                mollierPoint_End = mollierPoint_End_HumidityRatio;
            }
            else if (mollierPoint_End_HumidityRatio == null)
            {
                mollierPoint_End = mollierPoint_End_DryBulbTemperature;
            }
            else
            {
                double enthalpyDifference_HumidityRatio = System.Math.Abs(mollierPoint.Enthalpy - mollierPoint_End_HumidityRatio.Enthalpy);
                double enthalpyDifference_DryBulbTemperature = System.Math.Abs(mollierPoint.Enthalpy - mollierPoint_End_DryBulbTemperature.Enthalpy);

                mollierPoint_End = enthalpyDifference_HumidityRatio < enthalpyDifference_DryBulbTemperature ? mollierPoint_End_HumidityRatio : mollierPoint_End_DryBulbTemperature;
            }

            if (mollierPoint_End == null)
            {
                return null;
            }


            return new RoomProcess(mollierPoint_Start, mollierPoint_End, efficiency);
        }
    
        public static RoomProcess RoomProcess_ByEpsilonAndEnthalpyDifference(this MollierPoint mollierPoint, double epsilon, double enthalpyDifference, double efficiency = 1)
        {
            if(mollierPoint == null || double.IsNaN(epsilon) || double.IsNaN(enthalpyDifference) || epsilon == 0)
            {
                return null;
            }

            double enthalpy = mollierPoint.Enthalpy + enthalpyDifference;
            double humidityRatio = mollierPoint.HumidityRatio + (enthalpyDifference / 1000) / epsilon;

            MollierPoint mollierPoint_End = MollierPoint_ByEnthalpy(enthalpy, humidityRatio, mollierPoint.Pressure);
            if (mollierPoint_End == null)
            {
                return null;
            }

            return new RoomProcess(mollierPoint, mollierPoint_End, efficiency);
        }

        public static RoomProcess RoomProcess_ByEpsilonAndHumidityRatioDifference(this MollierPoint mollierPoint, double epsilon, double humidityRatioDifference, bool start = true, double efficiency = 1)
        {
            if (mollierPoint == null || double.IsNaN(epsilon) || double.IsNaN(humidityRatioDifference))
            {
                return null;
            }

            double factor = start ? 1 : -1;

            double enthalpy = mollierPoint.Enthalpy + humidityRatioDifference * epsilon * 1000 * factor;
            double humidityRatio = mollierPoint.HumidityRatio + humidityRatioDifference * factor;

            MollierPoint mollierPoint_End = MollierPoint_ByEnthalpy(enthalpy, humidityRatio, mollierPoint.Pressure);
            if (mollierPoint_End == null)
            {
                return null;
            }

            return new RoomProcess(mollierPoint, mollierPoint_End, efficiency);
        }
    }
}
