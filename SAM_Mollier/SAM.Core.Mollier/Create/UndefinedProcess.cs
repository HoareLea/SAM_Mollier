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

        public static UndefinedProcess UndefinedProcess(this MollierPoint mollierPoint, double sensibleHeatRatio, double specificHeat, double dryBulbTemperature_Start, double dryBulbTemperature_End)
        {
            if(mollierPoint == null || double.IsNaN(sensibleHeatRatio) || double.IsNaN(dryBulbTemperature_Start) || double.IsNaN(dryBulbTemperature_End) || double.IsNaN(specificHeat))
            {
                return null;
            }

            double enthaply_Start = Query.Enthalpy_BySensibleHeatRatio(sensibleHeatRatio, specificHeat, mollierPoint, dryBulbTemperature_Start);
            if(double.IsNaN(enthaply_Start))
            {
                return null;
            }

            double humidityRatio_Start = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_Start, enthaply_Start);
            if (double.IsNaN(humidityRatio_Start))
            {
                return null;
            }

            double enthaply_End = Query.Enthalpy_BySensibleHeatRatio(sensibleHeatRatio, specificHeat, mollierPoint, dryBulbTemperature_End);
            if (double.IsNaN(enthaply_Start))
            {
                return null;
            }

            double humidityRatio_End = Query.HumidityRatio_ByEnthalpy(dryBulbTemperature_End, enthaply_End);
            if (double.IsNaN(humidityRatio_End))
            {
                return null;
            }

            MollierPoint start = new MollierPoint(dryBulbTemperature_Start, humidityRatio_Start, mollierPoint.Pressure);
            MollierPoint end = new MollierPoint(dryBulbTemperature_End, humidityRatio_End, mollierPoint.Pressure);

            return new UndefinedProcess(start, end);
        }
    }
}
