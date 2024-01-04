using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class MixingProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.MixingProcess;
        internal MixingProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public MixingProcess(JObject jObject)
            :base(jObject)
        {

        }

        public MixingProcess(MixingProcess mixingProcess)
            : base(mixingProcess)
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
