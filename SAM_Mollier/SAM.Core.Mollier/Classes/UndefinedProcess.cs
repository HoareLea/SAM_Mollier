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
    }
}
