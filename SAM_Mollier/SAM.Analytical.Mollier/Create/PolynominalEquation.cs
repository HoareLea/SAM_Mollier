
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Create
    {
        public static Math.PolynomialEquation PolynomialEquation(this Core.Mollier.IMollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return null;
            }

            List<Core.Mollier.MollierPoint> mollierPoints = new List<Core.Mollier.MollierPoint>();
            mollierPoints.Add(mollierProcess.Start);
            mollierPoints.Add(mollierProcess.End);

            return Math.Create.PolynomialEquation(mollierPoints.ConvertAll(x => x.HumidityRatio), mollierPoints.ConvertAll(x => x.DryBulbTemperature), 2);
        }
    }
}
