using SAM.Core.Mollier;

namespace SAM.Core.Mollier
{
    public interface IUIMollierObject : IMollierObject
    {
        System.Guid Guid { get; }

        IUIMollierAppearance UIMollierAppearance { get; }
    }
}
