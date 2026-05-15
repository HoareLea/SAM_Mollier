using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public abstract class HumidificationProcess : MollierProcess
    {
        internal HumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public HumidificationProcess(JsonObject jObject)
            :base(jObject)
        {

        }

        public HumidificationProcess(HumidificationProcess humidificationProcess)
            : base(humidificationProcess)
        {

        }
        public override bool FromJsonObject(JsonObject jObject)
        {
            if (!base.FromJsonObject(jObject))
            {
                return false;
            }

            return true;
        }

        public override JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if (result == null)
            {
                return result;
            }

            return result;
        }
    }
}
