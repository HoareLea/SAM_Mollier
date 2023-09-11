using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool AlmostEqual(this IMollierPoint mollierPoint_1, IMollierPoint mollierPoint_2, double tolerance = Tolerance.MacroDistance)
        {
            if (mollierPoint_1 == mollierPoint_2)
            {
                return true;
            }

            if (mollierPoint_1 == null || mollierPoint_2 == null)
            {
                return false;
            }

            return mollierPoint_1.Distance(mollierPoint_2) <= tolerance;
        }

        public static bool AlmostEqual(this IMollierZone mollierZone_1, IMollierZone mollierZone_2, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierZone_1 == mollierZone_2)
            {
                return true;
            }
            if(mollierZone_1 == null || mollierZone_2 == null)
            {
                return false;
            }

            List<MollierPoint> zonePoints_1 = null;
            List<MollierPoint> zonePoints_2 = null;
            if (mollierZone_1 is UIMollierZone)
            {
                zonePoints_1 = ((UIMollierZone)mollierZone_1).MollierZone.MollierPoints;
            }
            else if(mollierZone_1 is MollierZone)
            {
                zonePoints_1 = ((MollierZone)mollierZone_1).MollierPoints;
            }

            if (mollierZone_2 is UIMollierZone)
            {
                zonePoints_2 = ((UIMollierZone)mollierZone_2).MollierZone.MollierPoints;
            }
            else if (mollierZone_2 is MollierZone)
            {
                zonePoints_2 = ((MollierZone)mollierZone_2).MollierPoints;
            }

            if(zonePoints_1 == null && zonePoints_2 == null)
            {
                return true;
            }
            if(zonePoints_1 == null || zonePoints_2 == null)
            {
                return false;
            }

            foreach (MollierPoint mollierPoint in zonePoints_1)
            {
                bool equal = false;
                foreach(MollierPoint mollierPoint2 in zonePoints_2)
                {
                    if(mollierPoint.AlmostEqual(mollierPoint2))
                    {
                        equal = true; 
                        break;
                    }
                }
                if(equal == false)
                {
                    return false;
                }
            }

            return true;
        }
    
        public static bool AlmostEqual(this IMollierProcess mollierProcess_1, IMollierProcess mollierProcess_2, double tolerance = Tolerance.MacroDistance)
        {
            return AlmostEqual(mollierProcess_1.Start, mollierProcess_2.Start, tolerance) && AlmostEqual(mollierProcess_1.End, mollierProcess_2.End, tolerance);
        }
    }
}
