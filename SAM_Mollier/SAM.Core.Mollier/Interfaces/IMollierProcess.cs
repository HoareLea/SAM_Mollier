namespace SAM.Core.Mollier
{
    public interface IMollierProcess : IMollierObject
    {
        MollierPoint Start { get; }
        MollierPoint End { get; }
        double Pressure { get; }
    }
}
