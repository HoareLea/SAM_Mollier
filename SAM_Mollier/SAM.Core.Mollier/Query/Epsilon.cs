namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static double Epsilon(this MollierPoint mollierPoint_1, MollierPoint mollierPoint_2)
        {
            if (mollierPoint_1 == null || mollierPoint_2 == null)
            {
                return double.NaN;
            }

            double humidityRatioDifference = mollierPoint_2.HumidityRatio - mollierPoint_1.HumidityRatio;
            if (humidityRatioDifference == 0)
            {
                return double.NegativeInfinity;
            }

            double enthalpyDifference = (mollierPoint_2.Enthalpy - mollierPoint_1.Enthalpy) / 1000;

            return enthalpyDifference / humidityRatioDifference;
        }

        public static double Epsilon(this MollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return double.NaN;
            }

            return Epsilon(mollierProcess.Start, mollierProcess.End);

        }

        /// <summary>
        /// Calculates slope coefficient Epsilon ε [kJ/kg] by Steam Temperature.
        /// </summary>
        /// <param name="steamTemperature">Steam Temperature [°C]</param>
        /// <returns>Epsilon ε [kJ/kg]</returns>
        public static double Epsilon(double steamTemperature)
        {
            if (double.IsNaN(steamTemperature))
            {
                return double.NaN;
            }

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000;
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;

            return vapourizationLatentHeat + specificHeat_WaterVapour * steamTemperature;
        }

        /// <summary>
        /// Calculates slope coefficient Epsilon ε [kJ/kg] by Total Heat Gains Qtotal=Qsens+Qlat [W] and Moisture Gains Mass Flow Ẇ [kg/s]
        /// </summary>
        /// <param name="totaLoad">Total Heat Gains Qtotal [W]</param>
        /// <param name="moistureGainsMassFlow">Moisture Gains Mass Flow  Ẇ [kg/s]</param>
        /// <returns>Epsilon ε [kJ/kg]</returns>
        public static double Epsilon(double totaLoad, double moistureGainsMassFlow)
        {
            if (double.IsNaN(totaLoad) || double.IsNaN(moistureGainsMassFlow))
            {
                return double.NaN;
            }

            double vapourizationLatentHeat = Zero.VapourizationLatentHeat / 1000;
            double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour / 1000;

            return totaLoad / moistureGainsMassFlow;
        }

    }
}
