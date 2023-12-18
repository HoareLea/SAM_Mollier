namespace SAM.Core.Mollier
{
    public static partial class Create
    {

        /// <summary>
        /// Creates HeatRecoveryProcess based on supply and return air parameters and heat recovery efficiences
        /// </summary>
        /// <param name="intake">Supply air parameters</param>
        /// <param name="extract">Return air parameters</param>
        /// <param name="sensibleHeatRecoveryEfficiency">Sensible Heat Recovery Efficiency [%]</param>
        /// <param name="latentHeatRecoveryEfficiency">Latent Heat Recovery Efficiency [%]</param>
        /// <returns>HeatRecoveryProcess</returns>
        public static HeatRecoveryProcess HeatRecoveryProcess_Supply(this MollierPoint intake, MollierPoint extract, double sensibleHeatRecoveryEfficiency, double latentHeatRecoveryEfficiency, double efficiency = 1)
        {
            if (intake == null || extract == null || double.IsNaN(sensibleHeatRecoveryEfficiency) || double.IsNaN(latentHeatRecoveryEfficiency))
            {
                return null;
            }

            double dryBulbTemperature = intake.DryBulbTemperature + ((extract.DryBulbTemperature - intake.DryBulbTemperature) * sensibleHeatRecoveryEfficiency /100);
            if(double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            double humidityRatio = intake.HumidityRatio + ((extract.HumidityRatio - intake.HumidityRatio) * latentHeatRecoveryEfficiency / 100);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }
            //if (exhaust)
            //{
            //    dryBulbTemperature = @return.DryBulbTemperature - ((@return.DryBulbTemperature - supply.DryBulbTemperature) * sensibleHeatRecoveryEfficiency / 100);
            //    humidityRatio = @return.HumidityRatio - ((@return.HumidityRatio - supply.HumidityRatio) * latentHeatRecoveryEfficiency / 100);
            //}
            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, intake.Pressure);
            if (end == null)
            {
                return null;
            }

            //if(exhaust)
            //{
            //    return new HeatRecoveryProcess(@return, end);
            //}

            return new HeatRecoveryProcess(intake, end, efficiency);
        }


        public static HeatRecoveryProcess HeatRecoveryProcess_Extract(this MollierPoint extract, MollierPoint intake, double sensibleHeatRecoveryEfficiency, double latentHeatRecoveryEfficiency, double efficiency = 1)
        {
            if (intake == null || extract == null || double.IsNaN(sensibleHeatRecoveryEfficiency) || double.IsNaN(latentHeatRecoveryEfficiency))
            {
                return null;
            }

            //calculate exhaust dryBulbTemperature
            double dryBulbTemperature = extract.DryBulbTemperature + ((intake.DryBulbTemperature - extract.DryBulbTemperature) * sensibleHeatRecoveryEfficiency / 100);
            if (double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            //calculate exhaust humidityRatio
            double humidityRatio = extract.HumidityRatio + ((intake.HumidityRatio - extract.HumidityRatio) * latentHeatRecoveryEfficiency / 100);
            if (double.IsNaN(humidityRatio))
            {
                return null;
            }

            //if heat recovery is sensible ONLY eff lat = 0, then condensate water if RH>100%
            double dryBulbTemperature_Start_Saturation = Query.DryBulbTemperature_ByHumidityRatio(humidityRatio, 100, extract.Pressure);

            if (dryBulbTemperature_Start_Saturation > dryBulbTemperature)//&& latentHeatRecoveryEfficiency == 0
            {
                humidityRatio = Query.HumidityRatio(dryBulbTemperature, 100, extract.Pressure);
            }

            //create exhaust Mollier point
            MollierPoint end = new MollierPoint(dryBulbTemperature, humidityRatio, extract.Pressure);
            if (end == null)
            {
                return null;
            }

            //Create extract heat recovery process between Extract Point and Exhaust
            return new HeatRecoveryProcess(extract, end, efficiency);
        }
    }
}
