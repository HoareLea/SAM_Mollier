namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static UndefinedProcess UndefinedProcess(this MollierPoint start, MollierPoint stop)
        {
            if (start == null || stop == null)
            {
                return null;
            }

            return new UndefinedProcess(start, stop);
        }

        /// <summary>
        /// Returns new UndefinedProcess based on given input
        /// </summary>
        /// <param name="start">Start MollierPoint</param>
        /// <param name="latentLoad">Latent Load [W]</param>
        /// <param name="airFlow">Air Flow [m3/s]</param>
        /// <param name="sensibleLoad">Sensible Load [W]</param>
        /// <returns>UndefinedProcess</returns>
        public static UndefinedProcess UndefinedProcess(this MollierPoint start, double airFlow, double sensibleLoad, double latentLoad)
        {
            if(start == null || !start.IsValid())
            {
                return null;
            }

            if (double.IsNaN(airFlow) || double.IsNaN(sensibleLoad) || double.IsNaN(latentLoad))
            {
                return null;
            }

            double dryBulbTemperature = start.DryBulbTemperature + ((sensibleLoad / 1000) / (airFlow * Query.HeatCapacity(start) * Query.Density(start)));
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }


            double humidityRatio = Query.HumidityRatio(start, latentLoad, airFlow);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }


            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, start.Pressure);
            if(!end.IsValid())
            {
                return null;
            }

            return UndefinedProcess(start, end);

        }

        public static UndefinedProcess UndefinedProcess_BySensibleHeatRatio(this MollierPoint mollierPoint, double sensibleHeatRatio, double dryBulbTemperature_Start, double dryBulbTemperature_End, double humidityRatio_Start, double humidityRatio_End)
        {
            if(mollierPoint == null || double.IsNaN(sensibleHeatRatio) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(dryBulbTemperature_End))
            {
                return null;
            }

            //START

            double specificHeat_Start = Query.SpecificHeat_Air(dryBulbTemperature_Start);

            MollierPoint start_DryBulbTemperature = null;
            if (sensibleHeatRatio != 0)
            {
                double enthaply = Query.Enthalpy_BySensibleHeatRatioAndDryBulbTemperature(mollierPoint.Pressure, sensibleHeatRatio, specificHeat_Start, mollierPoint, dryBulbTemperature_Start);
                if (!double.IsNaN(enthaply))
                {
                    double humidityRatio = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_Start, enthaply);
                    if (!double.IsNaN(humidityRatio))
                    {
                        start_DryBulbTemperature = new MollierPoint(dryBulbTemperature_Start, humidityRatio, mollierPoint.Pressure);
                    }
                }
            }

            MollierPoint start_HumidityRatio = null;
            if (sensibleHeatRatio != 1)
            {
                double humidityRatio = Query.HumidityRatio_BySensibleHeatRatioAndHumidityRatio(sensibleHeatRatio, specificHeat_Start, dryBulbTemperature_Start, dryBulbTemperature_End, humidityRatio_Start);
                if (!double.IsNaN(humidityRatio))
                {
                    start_HumidityRatio = new MollierPoint(dryBulbTemperature_Start, humidityRatio, mollierPoint.Pressure);
                }
            }

            if(start_DryBulbTemperature == null && start_HumidityRatio == null)
            {
                return null;
            }

            MollierPoint start = null;
            if(start_DryBulbTemperature == null)
            {
                start = start_HumidityRatio;
            } 
            else if(start_HumidityRatio == null)
            {
                start = start_DryBulbTemperature;
            }
            else
            {
                double enthalpyDifference_HumidityRatio = System.Math.Abs(mollierPoint.Enthalpy - start_HumidityRatio.Enthalpy);
                double enthalpyDifference_DryBulbTemperature = System.Math.Abs(mollierPoint.Enthalpy - start_DryBulbTemperature.Enthalpy);
                
                start = enthalpyDifference_HumidityRatio < enthalpyDifference_DryBulbTemperature ? start_HumidityRatio : start_DryBulbTemperature;
            }

            if(start == null)
            {
                return null;
            }

            //END

            double specificHeat_End = Query.SpecificHeat_Air(dryBulbTemperature_End);

            MollierPoint end_DryBulbTemperature = null;
            if (sensibleHeatRatio != 0)
            {
                double enthaply = Query.Enthalpy_BySensibleHeatRatioAndDryBulbTemperature(mollierPoint.Pressure, sensibleHeatRatio, specificHeat_End, mollierPoint, dryBulbTemperature_End);
                if (!double.IsNaN(enthaply))
                {
                    double humidityRatio = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_End, enthaply);
                    if (!double.IsNaN(humidityRatio))
                    {
                        end_DryBulbTemperature = new MollierPoint(dryBulbTemperature_End, humidityRatio, mollierPoint.Pressure);
                    }
                }
            }

            MollierPoint end_HumidityRatio = null;
            if (sensibleHeatRatio != 1)
            {
                double humidityRatio = Query.HumidityRatio_BySensibleHeatRatioAndHumidityRatio(sensibleHeatRatio, specificHeat_End, dryBulbTemperature_End, dryBulbTemperature_End, humidityRatio_End);
                if (!double.IsNaN(humidityRatio))
                {
                    end_HumidityRatio = new MollierPoint(dryBulbTemperature_End, humidityRatio, mollierPoint.Pressure);
                }
            }

            if (end_DryBulbTemperature == null && end_HumidityRatio == null)
            {
                return null;
            }

            MollierPoint end = null;
            if (end_DryBulbTemperature == null)
            {
                end = end_HumidityRatio;
            }
            else if (end_HumidityRatio == null)
            {
                end = end_DryBulbTemperature;
            }
            else
            {
                double enthalpyDifference_HumidityRatio = System.Math.Abs(mollierPoint.Enthalpy - end_HumidityRatio.Enthalpy);
                double enthalpyDifference_DryBulbTemperature = System.Math.Abs(mollierPoint.Enthalpy - end_DryBulbTemperature.Enthalpy);

                end = enthalpyDifference_HumidityRatio < enthalpyDifference_DryBulbTemperature ? end_HumidityRatio : end_DryBulbTemperature;
            }

            if (end == null)
            {
                return null;
            }


            return new UndefinedProcess(start, end);
        }
    }
}
