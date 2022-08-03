using SAM.Core.Mollier;
using SAM.Geometry.Planar;

namespace SAM.Analytical.Mollier
{
    public static partial class Convert
    {
        public static Polygon2D ToSAM_Polygon2D(this MollierZone mollierZone)
        {
            if (mollierZone == null)
                return null;

            return new Polygon2D(mollierZone.MollierPoints?.ConvertAll(x => x.ToSAM_Point2D()));
        }
    }
}