using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class AdiabaticHumidificationProcess : HumidificationProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.AdiabaticHumidificationProcess;

        internal AdiabaticHumidificationProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public AdiabaticHumidificationProcess(JObject jObject)
            :base(jObject)
        {

        }

        public AdiabaticHumidificationProcess(AdiabaticHumidificationProcess adiabaticHumidificationProcess)
            : base(adiabaticHumidificationProcess)
        {

        }
    }
}
