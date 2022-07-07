using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class HeatRecoveryProcess : MollierProcess
    {
        public HeatRecoveryProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public HeatRecoveryProcess(JObject jObject)
            :base(jObject)
        {

        }
    }
}
