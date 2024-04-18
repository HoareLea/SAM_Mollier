using SAM.Core.Mollier;

namespace SAM.Core.Mollier
{
    public interface IChartVisibilitySetting : IVisibilitySetting
    {
        ChartDataType ChartDataType { get; set; }
        ChartParameterType ChartParameterType { get; set; }
    }
}
