using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class FanProcess : HeatingProcess
    {
        internal FanProcess(MollierPoint start, MollierPoint end, double efficiency = 1)
            : base(start, end)
        {

        }

        public FanProcess(JObject jObject)
            :base(jObject)
        {

        }

        public FanProcess(FanProcess fanProcess)
            : base(fanProcess)
        {

        }   
    }
}
