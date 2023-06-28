using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static bool TryGetInfiltrationGains(this Space space, MollierPoint outside, MollierPoint inside, out double infiltrationLatentGain, out double infiltrationSensibleGain)
        {
            infiltrationLatentGain = double.NaN;
            infiltrationSensibleGain = double.NaN;

            if (space == null || outside == null || inside == null)
            {
                return false;
            }

            double density = outside.Density();
            if(double.IsNaN(density))
            {
                return false;
            }

            infiltrationLatentGain = Analytical.Query.CalculatedInfiltrationLatentGain(space, outside.HumidityRatio, inside.HumidityRatio, inside.DryBulbTemperature, density);
            infiltrationSensibleGain = Analytical.Query.CalculatedInfiltrationSensibleGain(space, outside.DryBulbTemperature, inside.DryBulbTemperature, density);


            return !double.IsNaN(infiltrationSensibleGain) && !double.IsNaN(infiltrationLatentGain);

        }
    }
}
