using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public abstract class SpecificProcess : MollierProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.SpecificProcess;

        internal SpecificProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public SpecificProcess(JObject jObject)
            : base(jObject)
        {

        }

        public SpecificProcess(SpecificProcess specificMollierProcess)
            : base(specificMollierProcess)
        {

        }
    }
}
