namespace SAM.Core.Mollier
{
    public interface IUIMollierObject : IMollierObject
    {
        System.Guid Guid { get; }

        UIMollierAppearance UIMollierAppearance { get; }
    }
}
