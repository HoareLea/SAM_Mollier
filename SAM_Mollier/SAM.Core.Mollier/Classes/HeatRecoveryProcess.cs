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
    }
}
