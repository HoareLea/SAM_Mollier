namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static HeatingProcess HeatingProcess(this MollierPoint start, double dryBulbTemperature)
        {
            if (start == null || double.IsNaN(dryBulbTemperature))
            {
                return null;
            }

            if(dryBulbTemperature < start.DryBulbTemperature)
            {
                return null;
            }

            MollierPoint end = new MollierPoint(dryBulbTemperature, start.HumidityRatio, start.Pressure);
            if(end == null)
            {
                return null;
            }

            return new HeatingProcess(start, end);
        }

        public static HeatingProcess HeatingProcess_ByTemperatureDifference(this MollierPoint start, double temperatureDifference)
        {
            if (start == null || double.IsNaN(temperatureDifference))
            {
                return null;
            }


            MollierPoint end = new MollierPoint(start.DryBulbTemperature + temperatureDifference, start.HumidityRatio, start.Pressure);
            if(end == null)
            {
                return null;
            }

            return new HeatingProcess(start, end);
        }

        public static HeatingProcess HeatingProcess_ByEnthalpyDifference(this MollierPoint start, double enthalpyDifference)
        {
            if (start == null || double.IsNaN(enthalpyDifference))
            {
                return null;
            }

            MollierPoint end = MollierPoint_ByEnthalpy(start.Enthalpy + enthalpyDifference, start.HumidityRatio, start.Pressure);
            if(end == null)
            {
                return null;
            }

            return new HeatingProcess(start, end);
        }
    }
}
