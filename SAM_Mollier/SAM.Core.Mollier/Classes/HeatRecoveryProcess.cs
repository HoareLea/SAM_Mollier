using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class HeatRecoveryProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.HeatRecoveryProcess;
        internal HeatRecoveryProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public HeatRecoveryProcess(JObject jObject)
            :base(jObject)
        {

        }

        public HeatRecoveryProcess(HeatRecoveryProcess heatRecoveryProcess)
            : base(heatRecoveryProcess)
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
