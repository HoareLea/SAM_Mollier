namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static bool AlmostEqual(this MollierPoint mollierPoint_1, MollierPoint mollierPoint_2, double tolerance = Tolerance.MacroDistance)
        {
            if(mollierPoint_1 == mollierPoint_2)
            {
                return true;
            }

            if(mollierPoint_1 == null || mollierPoint_2 == null)
            {
                return false;
            }

            return mollierPoint_1.Distance(mollierPoint_2) <= tolerance;

        }
    }
}
