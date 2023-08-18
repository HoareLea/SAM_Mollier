using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static System.Type SimpleEquipmentType(this IMollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return null;
            }

            if(mollierProcess is CoolingProcess)
            {
                return typeof(CoolingCoil);
            }

            if (mollierProcess is HeatingCoil)
            {
                return typeof(HeatingCoil);
            }

            if (mollierProcess is HeatRecoveryProcess)
            {
                return typeof(HeatRecoveryUnit);
            }

            if (mollierProcess is HumidificationProcess)
            {
                return typeof(Humidifier);
            }

            if (mollierProcess is MixingProcess)
            {
                return typeof(MixingSection);
            }

            return null;
        }
    }
}