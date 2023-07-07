namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates dry air mass flow [kg/s] 
        /// </summary>
        /// <param name="massFlow">Moist Air Mass Flow [kg/s]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg/kg]</param>
        /// <returns>Dry Air Mass Flow [kg/s]</returns>
        public static double MassFlow_DryAir(double massFlow, double humidityRatio)
        {
            if(double.IsNaN(massFlow) || double.IsNaN(humidityRatio))
            {
                return double.NaN;
            }

            return massFlow / (1 + humidityRatio);

        }

        /// <summary>
        /// Calculates dry air mass flow [kg/s] 
        /// </summary>
        /// <param name="mollierPoint">MollierPoint</param>
        /// <param name="massFlow">Moist Air Mass Flow [kg/s]</param>
        /// <returns>Dry Air Mass Flow [kg/s]</returns>
        public static double MassFlow_DryAir(this MollierPoint mollierPoint, double massFlow)
        {
            if (double.IsNaN(massFlow) || mollierPoint == null)
            {
                return double.NaN;
            }

            return MassFlow_DryAir(massFlow, mollierPoint.HumidityRatio);

        }

    }
}
