using System.Collections.Generic;
using System.Linq;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static HashSet<double> Pressures(this IEnumerable<IMollierObject> mollierObjects)
        {
            if(mollierObjects == null || mollierObjects.Count() == 0)
            {
                return null;
            }

            HashSet<double> result = new HashSet<double>();
            foreach(IMollierObject mollierObject in mollierObjects)
            {
                if(mollierObject is IMollierCurve)
                {
                    List<MollierPoint> mollierPoints = ((IMollierCurve)mollierObject).MollierPoints;
                    mollierPoints?.ForEach(x => result.Add(x.Pressure));
                }
                else if(mollierObject is IMollierPoint)
                {
                    result.Add(((IMollierPoint)mollierObject).Pressure);
                }
                else if(mollierObject is IMollierGroup)
                {
                    HashSet<double> pressures = Pressures(((IMollierGroup)mollierObject));
                    pressures?.ToList().ForEach(x => result.Add(x));
                }
            }

            return result;
        }
    }
}
