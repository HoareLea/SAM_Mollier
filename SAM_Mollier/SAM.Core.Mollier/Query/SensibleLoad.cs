namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Sensible load for the air
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <param name="temperatureDifference">temperature Difference [K]</param>
        /// <param name="airFlow">Flow rate [m3/s]</param>
        /// <returns>Sensible load [W]</returns>
        public static double SensibleLoad(this MollierPoint mollierPoint, double temperatureDifference, double airFlow)
        {
            if(mollierPoint == null || double.IsNaN(airFlow) || double.IsNaN(temperatureDifference))
            {
                return double.NaN;
            }

            return airFlow * mollierPoint.Density() * mollierPoint.HeatCapacity() * temperatureDifference * 1000;
        }

        /// <summary>
        /// Sensible load for given cooling process and airflow
        /// </summary>
        /// <param name="coolingProcess">Cooling process</param>
        /// <param name="airflow">Air flow [m3/s]</param>
        /// <returns>Sensible load [W]</returns>
        public static double SensibleLoad(this CoolingProcess coolingProcess, double airflow)
        {
            if(double.IsNaN(airflow) || coolingProcess == null)
            {
                return double.NaN;
            }

            return SensibleLoad(coolingProcess.End, System.Math.Abs(coolingProcess.Start.DryBulbTemperature - coolingProcess.End.DryBulbTemperature), airflow);
        }

        /// <summary>
        /// Sensible load for given heating process and airflow
        /// </summary>
        /// <param name="heatingProcess">Heating process</param>
        /// <param name="airflow">Air flow [m3/s]</param>
        /// <returns>Sensible load [W]</returns>
        public static double SensibleLoad(this HeatingProcess heatingProcess, double airflow)
        {
            if (double.IsNaN(airflow) || heatingProcess == null)
            {
                return double.NaN;
            }

            return SensibleLoad(heatingProcess.End, System.Math.Abs(heatingProcess.Start.DryBulbTemperature - heatingProcess.End.DryBulbTemperature), airflow);
        }

        /// <summary>
        /// Sensible load for given heating process and airflow
        /// </summary>
        /// <param name="heatRecoveryProcess">Heat recovery process</param>
        /// <param name="airflow">Air flow [m3/s]</param>
        /// <returns>Sensible load [W]</returns>
        public static double SensibleLoad(this HeatRecoveryProcess heatRecoveryProcess, double airflow)
        {
            if (double.IsNaN(airflow) || heatRecoveryProcess == null)
            {
                return double.NaN;
            }

            return SensibleLoad(heatRecoveryProcess.End, System.Math.Abs(heatRecoveryProcess.Start.DryBulbTemperature - heatRecoveryProcess.End.DryBulbTemperature), airflow);
        }
    }
}
