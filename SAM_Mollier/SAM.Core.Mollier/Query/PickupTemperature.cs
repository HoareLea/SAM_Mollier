namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Pickup Temperature for given specific fan power
        /// </summary>
        /// <param name="mollierPoint"></param>
        /// <param name="sfp">Specific Fan Power [W/l/s]</param>
        /// <returns></returns>
        public static double PickupTemperature(this MollierPoint mollierPoint,  double sfp)
        {
            if(mollierPoint == null || double.IsNaN(sfp))
            {
                return double.NaN;
            }

            return sfp / (mollierPoint.Density() * SpecificHeatCapacity_Air(mollierPoint));
        }

        public static double PickupTemperature(double sfp)
        {
            if (double.IsNaN(sfp))
            {
                return double.NaN;
            }

            return PickupTemperature(sfp, 1.2, 1.005);
        }

        /// <summary>
        /// Calculates Pickup Temperature [C] for given specific fan power (sfp)
        /// </summary>
        /// <param name="sfp">Specific Fan Power [W/l/s]</param>
        /// <param name="density">Moist Air Density ρ [kg_MoistAir/m3]</param>
        /// <param name="specificHeatCapacity">Specific Heat Capacity of Air [kJ/kgK]</param>
        /// <returns>Pickup Dry Bulb Temperature [C]</returns>
        public static double PickupTemperature(double sfp, double density, double specificHeatCapacity)
        {
            if (double.IsNaN(sfp) || double.IsNaN(density) || double.IsNaN(specificHeatCapacity))
            {
                return double.NaN;
            }

            return sfp / density * specificHeatCapacity;
        }
    }
}