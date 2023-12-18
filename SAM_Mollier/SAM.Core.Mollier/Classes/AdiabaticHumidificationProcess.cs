using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class AdiabaticHumidificationProcess : HumidificationProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.AdiabaticHumidificationProcess;

        internal AdiabaticHumidificationProcess(MollierPoint start, MollierPoint end, double efficiency = 1)
            : base(start, end, efficiency)
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
