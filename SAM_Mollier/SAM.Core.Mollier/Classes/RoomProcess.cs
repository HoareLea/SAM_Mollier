using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class RoomProcess : SpecificProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.RoomProcess;

        internal RoomProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public RoomProcess(JObject jObject)
            : base(jObject)
        {

        }

        public RoomProcess(RoomProcess roomProcess)
            : base(roomProcess)
        {

        }
    }
}
