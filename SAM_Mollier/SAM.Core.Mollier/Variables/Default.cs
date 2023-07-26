namespace SAM.Core.Mollier
{
    public static class Default
    {
        public const double Density_Min = 0.45;
        public const double Density_Max = 1.41;
        public const double Density_Interval = 0.05;
        public static System.Drawing.Color Density_Color { get; } = System.Drawing.Color.LightGreen;

        public const double Enthalpy_Min = -20; //TODO: [MACIEK] Implement Enthalpy, use SI units [J/kg]
        public const double Enthalpy_Max = 140; //TODO: [MACIEK] Implement Enthalpy, use SI units [J/kg]
        public const double Enthalpy_Interval = 1; //TODO: [MACIEK] Implement Enthalpy, use SI units [J/kg]
        public static System.Drawing.Color Enthalpy_Color { get; } = System.Drawing.Color.LightGray;

        public const double SpecificVolume_Min = 0.65;
        public const double SpecificVolume_Max = 1.92;
        public const double SpecificVolume_Interval = 0.05;
        public static System.Drawing.Color SpecificVolume_Color { get; } = System.Drawing.Color.LightPink;

        public const double WetBulbTemperature_Min = -10;
        public const double WetBulbTemperature_Max = 30;
        public const double WetBulbTemperature_Interval = 5;
        public static System.Drawing.Color WetBulbTemperature_Color { get; } = System.Drawing.Color.LightSalmon;

        public const double DryBulbTemperature_Min = -20;
        public const double DryBulbTemperature_Max = 50;
        public const double DryBulbTemperature_Interval = 5;
        public static System.Drawing.Color DryBulbTemperature_Color { get; } = System.Drawing.Color.LightGray;

        public const double HumidityRatio_Min = 0;//TODO: [MACIEK] Update Units Humidity Ratio [kg_waterVapor/kg_dryAir]
        public const double HumidityRatio_Max = 35; //TODO: [MACIEK] Update Units Humidity Ratio [kg_waterVapor/kg_dryAir]
        public const double HumidityRatio_Interval = 5;//TODO: [MACIEK] Update Units Humidity Ratio [kg_waterVapor/kg_dryAir]

        public const double PartialVapourPressure_Interval = 0.5; //TODO: [MACIEK] rename to CodeName PartialVapourPressure, use SI units [Pa]

        public static System.Drawing.Color RelativeHumidity_Color { get; } = System.Drawing.Color.LightBlue;

    }
}