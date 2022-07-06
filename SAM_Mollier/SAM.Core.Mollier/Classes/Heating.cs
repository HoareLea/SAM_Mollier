using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class Heating : MollierProcess
    {
        public Heating(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public Heating(JObject jObject)
            :base(jObject)
        {

        }
    }
}
