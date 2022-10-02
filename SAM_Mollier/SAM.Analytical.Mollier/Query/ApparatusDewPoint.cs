using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Apparatus Dew Point (ADP)
        /// </summary>
        /// <returns>Apparatus Dew Point (ADP)</returns>
        public static MollierPoint ApparatusDewPoint(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if (airHandlingUnitResult == null)
            {
                return null;
            }

            if(!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.CoolingCoilApparatusDewPoint, out MollierPoint result))
            {
                return null;
            }

            return result;
        }
    }
}
