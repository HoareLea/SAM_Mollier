using System.ComponentModel;
using SAM.Core.Attributes;

namespace SAM.Analytical.Mollier
{
    [AssociatedTypes(typeof(Space)), Description("Space Parameter")]
    public enum SpaceParameter
    {
        [ParameterProperties("Air Handling Unit Calculation Method", "Air Handling Unit Calculation Method"), ParameterValue(Core.ParameterType.String)] AirHandlingUnitCalculationMethod,
    }
}