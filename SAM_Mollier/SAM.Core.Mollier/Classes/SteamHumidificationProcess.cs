using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{

    /// <summary>
    /// Pseudo isotermic humidification proces by Steam where the small rise in dry-bulb temperature is due to the sensible heating effect of the steam.
    /// </summary>
    public class SteamHumidificationProcess : IsotermicHumidificationProcess
    {
        internal SteamHumidificationProcess(MollierPoint start, MollierPoint end, double efficiency = 1)
            : base(start, end, efficiency)
        {

        }

        public SteamHumidificationProcess(JObject jObject)
            :base(jObject)
        {

        }

        public SteamHumidificationProcess(SteamHumidificationProcess steamHumidificationProcess)
            : base(steamHumidificationProcess)
        {

        }
    }
}
