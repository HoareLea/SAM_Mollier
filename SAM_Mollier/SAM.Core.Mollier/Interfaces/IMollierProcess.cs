namespace SAM.Core.Mollier
{
    public interface IMollierProcess : IJSAMObject
    {
        MollierPoint Start { get; }
        MollierPoint End { get; }
    }
}
