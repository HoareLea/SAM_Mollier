using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static double SensibleLoad(this Space space, MollierPoint outside, MollierPoint inside)
        {
            if(space == null || outside == null || inside == null)
            {
                return double.NaN;
            }

            TryGetInfiltrationGains(space, outside, inside, out double infiltrationLatentGain, out double infiltrationSensibleGain);

            double equipmentSensibleGain = Analytical.Query.CalculatedEquipmentSensibleGain(space);
            double lightingGain = Analytical.Query.CalculatedLightingGain(space);
            double occupancySensibleGain = Analytical.Query.OccupancySensibleGain(space);

            return infiltrationSensibleGain + equipmentSensibleGain + lightingGain + occupancySensibleGain;
        }
    }
}
