using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Query
    {
        public static string Name(this IMollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return null;
            }

            MollierProcess mollierProcess_Temp = mollierProcess is UIMollierProcess ? ((UIMollierProcess)mollierProcess).MollierProcess : mollierProcess as MollierProcess;
            if(mollierProcess_Temp is HeatingProcess)
            {
                return "Heating";
            }

            if(mollierProcess_Temp is CoolingProcess)
            {
                return "Cooling";
            }

            if(mollierProcess_Temp is HeatRecoveryProcess)
            {
                return "HeatRecovery";
            }

            if (mollierProcess_Temp is MixingProcess)
            {
                return "Mixing";
            }

            if (mollierProcess_Temp is AdiabaticHumidificationProcess)
            {
                return "Humidification Adiabatic";
            }

            if (mollierProcess_Temp is IsothermicHumidificationProcess)
            {
                return "Humidification Isothermal Steam";
            }

            if (mollierProcess_Temp is RoomProcess)
            {
                return "Room";
            }

            return null;
        }
    }
}
