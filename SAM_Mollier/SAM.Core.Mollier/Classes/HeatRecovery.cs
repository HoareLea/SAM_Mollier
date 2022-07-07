using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class HeatRecovery : MollierProcess
    {
        public HeatRecovery(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public HeatRecovery(JObject jObject)
            :base(jObject)
        {

        }
    }
}
