namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Duty
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <param name="humidityRatioDifference">Humidity ratio difference[kg_waterVapor/kg_dryAir]</param>
        /// <param name="airFlow">Air flow [m3/s]</param>
        /// <returns>Duty [g/hr]</returns>
        public static double Duty(this MollierPoint mollierPoint, double humidityRatioDifference, double airFlow)
        {
            if(mollierPoint == null || double.IsNaN(humidityRatioDifference) || double.IsNaN(airFlow))
            {
                return double.NaN;
            }

            return airFlow * mollierPoint.Density() * humidityRatioDifference * 3600 * 1000;
        }

        /// <summary>
        /// Calculates Duty for given humidification process
        /// </summary>
        /// <param name="humidificationProcess">Humidification process</param>
        /// <param name="airFlow">Air flow [m3/s]</param>
        /// <returns>Duty [g/hr]</returns>
        public static double Duty(this HumidificationProcess humidificationProcess, double airFlow)
        {
            if(humidificationProcess == null || double.IsNaN(airFlow))
            {
                return double.NaN;
            }

            return Duty(humidificationProcess.End, System.Math.Abs(humidificationProcess.Start.HumidityRatio - humidificationProcess.End.HumidityRatio), airFlow);
        }
    }
}
