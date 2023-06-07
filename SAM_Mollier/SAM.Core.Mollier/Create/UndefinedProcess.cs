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
    }
}
