using System.ComponentModel;

namespace SAM.Core.Mollier
{
    public enum ChartParameterType
    {
        [Description("Undefined")] Undefined,
        [Description("Unit")] Unit,
        [Description("Line")] Line,
        [Description("Bold Line")] BoldLine,
        [Description("Medium Line")] MediumLine,
        [Description("Label")] Label,
        [Description("Point")] Point
    }
}
