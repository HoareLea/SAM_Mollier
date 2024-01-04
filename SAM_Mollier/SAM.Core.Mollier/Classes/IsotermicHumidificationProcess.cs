using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class IsotermicHumidificationProcess : HumidificationProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.IsotermicHumidificationProcess;

        internal IsotermicHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public IsotermicHumidificationProcess(JObject jObject)
            :base(jObject)
        {

        }

        public IsotermicHumidificationProcess(IsotermicHumidificationProcess isotermicHumidificationProcess)
            : base(isotermicHumidificationProcess)
        {

        }
    }
}
