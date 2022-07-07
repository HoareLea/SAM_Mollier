using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public abstract class HumidificationProcess : MollierProcess
    {
        internal HumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public HumidificationProcess(JObject jObject)
            :base(jObject)
        {

        }
    }
}
