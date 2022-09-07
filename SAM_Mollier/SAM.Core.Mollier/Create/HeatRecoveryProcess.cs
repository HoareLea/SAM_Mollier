namespace SAM.Core.Mollier
{
    public static partial class Create
    {

        /// <summary>
        /// Creates HeatRecoveryProcess based on supply and return air parameters and heat recovery efficiences
        /// </summary>
        /// <param name="supply">Supply air parameters</param>
        /// <param name="return">Return air parameters</param>
        /// <param name="sensibleHeatRecoveryEfficiency">Sensible Heat Recovery Efficiency [%]</param>
        /// <param name="latentHeatRecoveryEfficiency">Latent Heat Recovery Efficiency [%]</param>
        /// <returns>HeatRecoveryProcess</returns>
        public static HeatRecoveryProcess HeatRecoveryProcess(this MollierPoint supply, MollierPoint @return, double sensibleHeatRecoveryEfficiency, double latentHeatRecoveryEfficiency, bool exhaust = false)
        {
            if (supply == null || @return == null || double.IsNaN(sensibleHeatRecoveryEfficiency) || double.IsNaN(latentHeatRecoveryEfficiency))
            {
                return null;
            }

            double dryBulbTemperature = supply.DryBulbTemperature + ((@return.DryBulbTemperature - supply.DryBulbTemperature) * sensibleHeatRecoveryEfficiency /100);
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            double humidityRatio = supply.HumidityRatio + ((@return.HumidityRatio - supply.HumidityRatio) * latentHeatRecoveryEfficiency / 100);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }
            if (exhaust)
            {

            }
            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, supply.Pressure);
            if (end == null)
            {
                return null;
            }

            return new HeatRecoveryProcess(supply, end);
        }
    }
}
