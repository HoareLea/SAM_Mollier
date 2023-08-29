using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static double LatentLoad(this Space space, MollierPoint outside, MollierPoint inside)
        {
            if(space == null || outside == null || inside == null)
            {
                return double.NaN;
            }

            TryGetInfiltrationGains(space, outside, inside, out double infiltrationLatentGain, out double infiltrationSensibleGain);

            double equipmentLatentGain = Analytical.Query.CalculatedEquipmentLatentGain(space);
            double occupancyLatentGain = Analytical.Query.OccupancyLatentGain(space);

            return infiltrationLatentGain + equipmentLatentGain + occupancyLatentGain;
        }
    }
}
