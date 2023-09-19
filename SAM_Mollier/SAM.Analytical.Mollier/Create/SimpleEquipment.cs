using SAM.Core.Mollier;

namespace SAM.Analytical.Mollier
{
    public static partial class Create
    {
        public static SimpleEquipment SimpleEquipment(this IMollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return null;
            }

            if (mollierProcess is UndefinedProcess)
            {
                return null;
            }

            if(mollierProcess is SpecificProcess)
            {
                return null;
            }

            if(mollierProcess is HeatingProcess)
            {
                if(mollierProcess is FanProcess)
                {
                    return new Fan();
                }

                return new HeatingCoil(double.NaN, double.NaN, double.NaN, double.NaN);
            }

            if(mollierProcess is CoolingProcess)
            {
                return new CoolingCoil(double.NaN, double.NaN, double.NaN, double.NaN);
            }

            if(mollierProcess is HeatRecoveryProcess)
            {
                return new HeatRecoveryUnit(double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);
            }

            if (mollierProcess is HumidificationProcess)
            {
                return new Humidifier();
            }

            if(mollierProcess is MixingProcess)
            {
                return new MixingSection();
            }

            return null;

        }
    }
}