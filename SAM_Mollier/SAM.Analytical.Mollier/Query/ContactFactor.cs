using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        /// <summary>
        /// Calculates Contact Factor
        /// </summary>
        /// <param name="coolingProcess">Cooling Process</param>
        /// <returns>Contact Factor </returns>
        public static double ContactFactor(this CoolingProcess coolingProcess)
        {
            if (coolingProcess == null)
            {
                return double.NaN;
            }

            MollierPoint evaporatingPoint = coolingProcess.EvaporatingPoint();
            if(evaporatingPoint == null)
            {
                return double.NaN;
            }

            return (coolingProcess.Start.Enthalpy - coolingProcess.End.Enthalpy) / (coolingProcess.Start.Enthalpy - evaporatingPoint.Enthalpy);

        }
    }
}