using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class HeatingProcess : MollierProcess
    {
        internal HeatingProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public HeatingProcess(JObject jObject)
            :base(jObject)
        {

        }
    }
}
