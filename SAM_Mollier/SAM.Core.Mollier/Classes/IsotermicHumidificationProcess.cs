using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class IsotermicHumidificationProcess : MollierProcess
    {
        internal IsotermicHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public IsotermicHumidificationProcess(JObject jObject)
            :base(jObject)
        {

        }
    }
}
