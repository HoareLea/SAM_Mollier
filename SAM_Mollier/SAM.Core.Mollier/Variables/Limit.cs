namespace SAM.Core.Mollier
{
    //Limits
    public static class Limit
    {
        public const double Density_Min = 0.45;
        public const double Density_Max = 1.41;

        public const double Enthalpy_Min = -20; //TODO: [MACIEK] Implement Enthalpy, use SI units [J/kg]
        public const double Enthalpy_Max = 160; //TODO: [MACIEK] Implement Enthalpy, use SI units [J/kg]

        public const double SpecificVolume_Min = 0.65;
        public const double SpecificVolume_Max = 1.92;

        public const double WetBulbTemperature_Min = -10;
        public const double WetBulbTemperature_Max = 30;

        public const double DryBulbTemperature_Min = -30;
        public const double DryBulbTemperature_Max = 55;

        public const double HumidityRatio_Min = 0;//TODO: [MACIEK] Update Units Humidity Ratio [kg_waterVapor/kg_dryAir]
        public const double HumidityRatio_Max = 55; //TODO: [MACIEK] Update Units Humidity Ratio [kg_waterVapor/kg_dryAir]
    }
}