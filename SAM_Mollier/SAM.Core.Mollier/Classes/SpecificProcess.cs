using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public abstract class SpecificProcess : MollierProcess
    {
        internal SpecificProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public SpecificProcess(JObject jObject)
            : base(jObject)
        {

        }

        public SpecificProcess(SpecificProcess specificMollierProcess)
            : base(specificMollierProcess)
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
