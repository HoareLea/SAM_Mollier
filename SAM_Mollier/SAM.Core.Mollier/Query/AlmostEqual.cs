using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool AlmostEqual(this IMollierObject mollierObject_1, IMollierObject mollierObject_2, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierObject_1 is IMollierPoint && mollierObject_2 is IMollierPoint)
            {
                return ((IMollierPoint)mollierObject_1).AlmostEqual((IMollierPoint)mollierObject_2, tolerance);
            }
            else if(mollierObject_1 is IMollierProcess && mollierObject_2 is IMollierProcess)
            {
                return ((IMollierProcess)mollierObject_1).AlmostEqual((IMollierProcess)mollierObject_2, tolerance);
            }
            else if(mollierObject_1 is IMollierZone && mollierObject_2 is IMollierZone)
            {
                return ((IMollierZone)mollierObject_1).AlmostEqual((IMollierZone)mollierObject_2, tolerance);
            }
            else if(mollierObject_1 is IMollierGroup && mollierObject_2 is IMollierGroup)
            {
                return ((IMollierGroup)mollierObject_1).AlmostEqual((IMollierGroup)mollierObject_2, tolerance);
            }
            return false;
        }
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

            List<MollierPoint> zonePoints_1 = (mollierZone_1 as MollierZone).MollierPoints;
            List<MollierPoint> zonePoints_2 = (mollierZone_2 as MollierZone).MollierPoints;

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
