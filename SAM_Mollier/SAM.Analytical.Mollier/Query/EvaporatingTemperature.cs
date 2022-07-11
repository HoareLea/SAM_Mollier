using SAM.Core.Mollier;
using SAM.Geometry.Planar;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static MollierPoint EvaporatingPoint(this CoolingProcess coolingProcess)
        {
            if (coolingProcess == null)
            {
                return null;
            }

            Point2D point2D = coolingProcess.End?.ToSAM_Point2D();
            if (point2D == null)
            {
                return null;
            }

            Math.PolynomialEquation polynomialEquation = coolingProcess.PolynomialEquation();
            if (polynomialEquation == null)
            {
                return null;
            }

            Math.PolynomialEquation polynomialEquation_SaturatedAir = SaturatedAirPolynominalEquation(coolingProcess.Pressure);
            if (polynomialEquation_SaturatedAir == null)
            {
                return null;
            }


            List<Point2D> point2Ds = Geometry.Query.Intersections(polynomialEquation, polynomialEquation_SaturatedAir);
            if (point2Ds == null || point2Ds.Count == 0)
            {
                return null;
            }

            if (point2Ds.Count > 1)
            {
                point2Ds.SortByDistance(point2D);
            }

            return new MollierPoint(point2Ds[0].Y, point2Ds[0].X, coolingProcess.Pressure);
        }
    }
}