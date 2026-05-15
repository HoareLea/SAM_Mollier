using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class RoomProcess : SpecificProcess
    {
        public override ChartDataType ChartDataType => ChartDataType.RoomProcess;

        internal RoomProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public RoomProcess(JsonObject jObject)
            : base(jObject)
        {

        }

        public RoomProcess(RoomProcess roomProcess)
            : base(roomProcess)
        {

        }
    }
}
