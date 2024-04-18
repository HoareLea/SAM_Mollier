using SAM.Core.Grasshopper.Mollier;
using SAM.Core.Mollier;
using SAM.Geometry.Mollier;
using System.Drawing;

namespace SAM.Geometry.Grasshopper.Mollier
{
    public static partial class Create
    {
        public static MollierChartObject MollierChartObject(this IMollierProcess mollierProcess, Color color, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            if(color == Color.Empty)
            {
                return MollierChartObject(mollierProcess as UIMollierProcess, chartType, z);
            }

            return MollierChartObject(mollierProcess, color, chartType, z);
        }

        public static MollierChartObject MollierChartObject(this UIMollierProcess uIMollierProcess, ChartType chartType, double z = 0)
        {
            if (uIMollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            Color color = Color.Empty;
            IUIMollierAppearance uIMollierAppearance = uIMollierProcess.UIMollierAppearance;
            if(uIMollierAppearance != null)
            {
                color = uIMollierAppearance.Color;
            }

            return MollierChartObject(uIMollierProcess, color, chartType, z);
        }
    }
}
