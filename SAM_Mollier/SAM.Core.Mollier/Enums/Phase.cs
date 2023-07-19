using System.ComponentModel;

namespace SAM.Core.Mollier
{
    public enum Phase
    {
        [Description("Undefined")] Undefined,
        [Description("Gas")] Gas,
        [Description("Liquid")] Liquid,
        [Description("Solid")] Solid
    }
}
