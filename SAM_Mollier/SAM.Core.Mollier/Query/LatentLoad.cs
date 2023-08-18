namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates latent load [kg/s]
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <param name="humidityRatioDifference">Humidity ratio difference [kg/kg]</param>
        /// <param name="airFlow">Air flow [m3/s]</param>
        /// <returns>Latent load [kg/s]</returns>
        public static double LatentLoad(this MollierPoint mollierPoint, double humidityRatioDifference, double airFlow)
        {
            if(mollierPoint == null || double.IsNaN(airFlow) || double.IsNaN(humidityRatioDifference))
            {
                return double.NaN;
            }

            return airFlow * mollierPoint.Density() * humidityRatioDifference;
        }

        /// <summary>
        /// Calculates latent load [kg/s]
        /// </summary>
        /// <param name="humidityRatioDifference">Humidity ratio difference [kg/kg]</param>
        /// <param name="massFlow">Mass flow [kg/s]</param>
        /// <returns>Latent load [kg/s]</returns>
        public static double LatentLoad_ByMassFlow(double humidityRatioDifference, double massFlow)
        {
            if(double.IsNaN(massFlow) || double.IsNaN(humidityRatioDifference))
            {
                return double.NaN;
            }

            return massFlow * humidityRatioDifference;
        }
    }
}
