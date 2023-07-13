using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static List<MollierPoint> Intersections(MollierPoint start, MollierPoint end, List<MollierPoint> mollierPoints, double tolerance = Tolerance.Distance)
        {
            if(start == null || end == null || mollierPoints == null || mollierPoints.Count < 2)
            {
                return null;
            }

            List<MollierPoint> result = new List<MollierPoint>();
            for(int i=0; i < mollierPoints.Count - 1; i++)
            {
                if(!Intersection(start, end, mollierPoints[i], mollierPoints[i + 1], true, out MollierPoint intersection, tolerance))
                {
                    continue;
                }

                if(intersection == null || !intersection.IsValid())
                {
                    continue;
                }

                result.Add(intersection);
            }

            return result;
        }
    }
}
