namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static HeatRecovery HeatRecovery(this MollierPoint supply, MollierPoint @return, double sensibleHeatRecoveryEfficiency, double latentHeatRecoveryEfficiency)
        {
            if (supply == null || @return == null || double.IsNaN(sensibleHeatRecoveryEfficiency) || double.IsNaN(latentHeatRecoveryEfficiency))
            {
                return null;
            }

            double dryBulbTemperature = supply.DryBulbTemperature + ((@return.DryBulbTemperature - supply.DryBulbTemperature) * sensibleHeatRecoveryEfficiency);
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            double humidityRatio = supply.HumidityRatio + ((@return.HumidityRatio - supply.HumidityRatio) * latentHeatRecoveryEfficiency);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }

            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, supply.Pressure);
            if (end == null)
            {
                return null;
            }

            return new HeatRecovery(supply, end);
        }
    }
}
