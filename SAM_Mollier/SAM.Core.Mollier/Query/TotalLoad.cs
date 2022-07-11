namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Total load of the air
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <param name="enthalpyDifference">Enthalpy difference [kJ/kg]</param>
        /// <param name="airFlow">Flow rate of air[m3/s]</param>
        /// <returns></returns>
        public static double TotalLoad(this MollierPoint mollierPoint, double enthalpyDifference, double airFlow)
        {
            if(mollierPoint == null || double.IsNaN(airFlow) || double.IsNaN(enthalpyDifference))
            {
                return double.NaN;
            }

            return airFlow * mollierPoint.Density() * enthalpyDifference * 1000;
        }

        /// <summary>
        /// Total load for given coolingProcess and airflow
        /// </summary>
        /// <param name="coolingProcess">Cooling process</param>
        /// <param name="airflow">Air flow [m3/s]</param>
        /// <returns>Total load [W]</returns>
        public static double TotalLoad(this CoolingProcess coolingProcess, double airflow)
        {
            if (double.IsNaN(airflow) || coolingProcess == null)
            {
                return double.NaN;
            }

            return SensibleLoad(coolingProcess.End, System.Math.Abs(coolingProcess.Start.Enthalpy - coolingProcess.End.Enthalpy), airflow);
        }
    }
}
