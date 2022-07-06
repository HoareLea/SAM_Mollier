namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static Cooling Cooling(this MollierPoint start, double dryBulbTemperature)
        {
            if (start == null || double.IsNaN(dryBulbTemperature))
            {
                return null;
            }


            MollierPoint end = null;

            return new Cooling(start, end);
        }

        public static Cooling Cooling_ByPower(this MollierPoint start, double power)
        {
            if(start == null || double.IsNaN(power))
            {
                return null;
            }


            MollierPoint end = null;

            return new Cooling(start, end);
        }

        public static Cooling Cooling_ByTemperatureDifference(this MollierPoint start, double temperatureDifference)
        {
            if (start == null || double.IsNaN(temperatureDifference))
            {
                return null;
            }


            MollierPoint end = null;

            return new Cooling(start, end);
        }

        public static Cooling Cooling_ByEnthalpyDifference(this MollierPoint start, double enthalpy)
        {
            if (start == null || double.IsNaN(enthalpy))
            {
                return null;
            }


            MollierPoint end = null;

            return new Cooling(start, end);
        }
    }
}
