using SAM.Core.Mollier;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static List<IMollierProcess> MollierProcesses(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if (airHandlingUnitResult == null)
            {
                return null;
            }

            List<IMollierProcess> result = new List<IMollierProcess>();

            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignTemperature, out double winterDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignRelativeHumidity, out double winterDesignRelativeHumidity);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.FrostCoilOffTemperature, out double frostOffCoilTemperature);

            if(frostOffCoilTemperature > winterDesignTemperature)
            {
                MollierPoint start = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterDesignTemperature, winterDesignRelativeHumidity, 101325);

                Heating heating = Core.Mollier.Create.Heating(start, frostOffCoilTemperature);
                result.Add(heating);
            }


            return result;
        }
    }
}

