using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class MixingProcess : MollierProcess
    {
        internal MixingProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public MixingProcess(JObject jObject)
            :base(jObject)
        {

        }
    }
}
