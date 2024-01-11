using SAM.Core.Mollier;
using SAM.Geometry.Planar;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static MollierPoint EvaporatingPoint(this CoolingProcess coolingProcess, double tolerance = Core.Tolerance.Distance)
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

            if(Core.Query.AlmostEqual(coolingProcess.Start.HumidityRatio, coolingProcess.End.HumidityRatio, tolerance))
            {
                double dryBulbTemperature = Core.Mollier.Query.DryBulbTemperature_ByHumidityRatio(coolingProcess.End.HumidityRatio, 100, coolingProcess.Start.Pressure);
                if(double.IsNaN(dryBulbTemperature))
                {
                    return null;
                }

                return new MollierPoint(dryBulbTemperature, coolingProcess.End.HumidityRatio, coolingProcess.Start.Pressure);
            }

            Math.LinearEquation linearEquation = coolingProcess.LinearEquation();
            if (linearEquation == null)
            {
                return null;
            }

            Math.PolynomialEquation polynomialEquation_SaturatedAir = SaturatedAirPolynominalEquation(coolingProcess.Pressure);
            if (polynomialEquation_SaturatedAir == null)
            {
                return null;
            }


            List<Point2D> point2Ds = Geometry.Planar.Query.Intersections(linearEquation, polynomialEquation_SaturatedAir);
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