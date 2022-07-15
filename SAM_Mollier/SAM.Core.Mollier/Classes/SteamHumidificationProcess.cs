using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class SteamHumidificationProcess : IsotermicHumidificationProcess
    {
        internal SteamHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public SteamHumidificationProcess(JObject jObject)
            :base(jObject)
        {

        }
    }
}
