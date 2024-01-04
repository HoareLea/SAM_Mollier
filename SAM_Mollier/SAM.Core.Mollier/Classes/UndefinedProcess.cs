using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class UndefinedProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.UndefinedProcess;
        internal UndefinedProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public UndefinedProcess(JObject jObject)
            :base(jObject)
        {

        }

        public UndefinedProcess(UndefinedProcess undefinedProcess)
            : base(undefinedProcess)
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
