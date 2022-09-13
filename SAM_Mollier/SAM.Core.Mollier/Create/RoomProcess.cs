namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static RoomProcess RoomProcess(MollierPoint start, MollierPoint end)
        {
            return new RoomProcess(start, end);
        }
    }
}
