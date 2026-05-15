using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class FanProcess : HeatingProcess
    {
        internal FanProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public FanProcess(JsonObject jObject)
            :base(jObject)
        {

        }

        public FanProcess(FanProcess fanProcess)
            : base(fanProcess)
        {

        }   
    }
}
