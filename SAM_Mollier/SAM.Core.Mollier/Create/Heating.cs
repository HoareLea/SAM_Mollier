namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static Heating Heating(this MollierPoint start, double dryBulbTemperature)
        {
            if (start == null || double.IsNaN(dryBulbTemperature))
            {
                return null;
            }


            MollierPoint end = null;

            return new Heating(start, end);
        }

        public static Heating Heating_ByPower(this MollierPoint start, double power)
        {
            if(start == null || double.IsNaN(power))
            {
                return null;
            }


            MollierPoint end = null;

            return new Heating(start, end);
        }

        public static Heating Heating_ByTemperatureDifference(this MollierPoint start, double temperatureDifference)
        {
            if (start == null || double.IsNaN(temperatureDifference))
            {
                return null;
            }


            MollierPoint end = null;

            return new Heating(start, end);
        }

        public static Heating Heating_ByEnthalpyDifference(this MollierPoint start, double enthalpy)
        {
            if (start == null || double.IsNaN(enthalpy))
            {
                return null;
            }


            MollierPoint end = null;

            return new Heating(start, end);
        }
    }
}
