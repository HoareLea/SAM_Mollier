using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public abstract class MollierLine : MollierCurve
    {
        internal MollierLine(MollierPoint mollierPoint)
            : base(mollierPoint)
        {

        }

        public MollierLine(MollierLine mollierLine)
            :base(mollierLine)
        {

        }

        public virtual bool FromJObject(JObject jObject)
        {
            bool result = base.FromJObject(jObject);
            if (!result)
            {
                return false;
            }

            return result;
        }

        public virtual JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return result;
            }

            return result;
        }
    }
}
