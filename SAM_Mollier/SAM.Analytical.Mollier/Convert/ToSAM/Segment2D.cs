using SAM.Core.Mollier;
using SAM.Geometry.Planar;

namespace SAM.Analytical.Mollier
{
    public static partial class Convert
    {
        public static Segment2D ToSAM_Segment2D(this IMollierProcess mollierProcess)
        {
            if (mollierProcess == null)
                return null;

            return new Segment2D(mollierProcess.Start.ToSAM_Point2D(), mollierProcess.End.ToSAM_Point2D());
        }
    }
}