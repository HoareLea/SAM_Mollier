using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class IsothermicHumidificationProcess : HumidificationProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.IsothermicHumidificationProcess;

        internal IsothermicHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public IsothermicHumidificationProcess(JObject jObject)
            :base(jObject)
        {

        }

        public IsothermicHumidificationProcess(IsothermicHumidificationProcess isothermicHumidificationProcess)
            : base(isothermicHumidificationProcess)
        {

        }
    }
}
