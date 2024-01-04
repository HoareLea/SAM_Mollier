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

        public HumidificationProcess(HumidificationProcess humidificationProcess)
            : base(humidificationProcess)
        {

        }
        public override bool FromJObject(JObject jObject)
        {
            if (!base.FromJObject(jObject))
            {
                return false;
            }

            return true;
        }

        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if (result == null)
            {
                return result;
            }

            return result;
        }
    }
}
