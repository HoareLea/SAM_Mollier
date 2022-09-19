using System.ComponentModel;

namespace SAM.Core.Mollier
{
    public enum ChartType
    {
        [Description("Undefined")] Undefined,
        [Description("Mollier")] Mollier,
        [Description("Psychrometric")] Psychrometric
    }
}
