using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public interface IMollierCurve : IMollierGroupable
    {
        List<MollierPoint> MollierPoints { get; }
        
        double Pressure { get; }

        MollierPoint Start { get; }

        MollierPoint End { get; }
    }
}
