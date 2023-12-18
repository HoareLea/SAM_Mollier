using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class RoomProcess : SpecificProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.RoomProcess;

        internal RoomProcess(MollierPoint start, MollierPoint end, double efficiency = 1)
            : base(start, end, efficiency)
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
