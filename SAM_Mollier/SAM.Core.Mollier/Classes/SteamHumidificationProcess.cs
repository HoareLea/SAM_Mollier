using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{

    /// <summary>
    /// Pseudo isothermic humidification proces by Steam where the small rise in dry-bulb temperature is due to the sensible heating effect of the steam.
    /// </summary>
    public class SteamHumidificationProcess : IsothermicHumidificationProcess
    {
        internal SteamHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
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
