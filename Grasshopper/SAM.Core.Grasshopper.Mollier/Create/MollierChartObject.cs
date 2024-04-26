using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Create
    {
        public static MollierChartObject MollierChartObject(this MollierProcess mollierProcess, Color color, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined || color == Color.Empty)
            {
                return null;
            }

            return MollierChartObject(mollierProcess, color, chartType, z);
        }
    }
}
