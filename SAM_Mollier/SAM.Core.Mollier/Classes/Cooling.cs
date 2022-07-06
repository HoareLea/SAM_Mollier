using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class Cooling : MollierProcess
    {
        public Cooling(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public Cooling(JObject jObject)
            :base(jObject)
        {

        }
    }
}
