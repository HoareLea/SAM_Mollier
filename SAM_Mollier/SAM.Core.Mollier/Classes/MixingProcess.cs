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
    }
}
