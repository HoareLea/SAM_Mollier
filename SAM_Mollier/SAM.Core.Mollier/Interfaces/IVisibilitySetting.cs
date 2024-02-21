using System.Drawing;

namespace SAM.Core.Mollier
{
    public interface IVisibilitySetting : IJSAMObject
    {
        Color Color { get; set; }

        bool Visible { get; set; }

    }
}
