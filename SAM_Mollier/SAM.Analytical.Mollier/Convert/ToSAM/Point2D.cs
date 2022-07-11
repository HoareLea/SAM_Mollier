using SAM.Core.Mollier;
using SAM.Geometry.Planar;

namespace SAM.Analytical.Mollier
{
    public static partial class Convert
    {
        public static Point2D ToSAM_Point2D(this MollierPoint mollerPoint)
        {
            if (mollerPoint == null)
                return null;

            return new Point2D(mollerPoint.HumidityRatio, mollerPoint.DryBulbTemperature);
        }
    }
}