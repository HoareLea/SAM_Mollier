namespace SAM.Core.Mollier
{
    public interface IMollierProcess : IMollierGroupable
    {
        MollierPoint Start { get; }
        MollierPoint End { get; }
        double Pressure { get; }
    }
}
