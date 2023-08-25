
namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static MollierRange MollierRange(double dryBulbTemperature_Min = Default.DryBulbTemperature_Min, double dryBulbTemperature_Max = Default.DryBulbTemperature_Max, double humidityRatio_Min = Default.HumidityRatio_Min, double humidityRatio_Max = Default.HumidityRatio_Max)
        {
            if (double.IsNaN(dryBulbTemperature_Min) || double.IsNaN(dryBulbTemperature_Max) || double.IsNaN(humidityRatio_Min) || double.IsNaN(humidityRatio_Max))
            {
                return null;
            }

            return new MollierRange(dryBulbTemperature_Min, dryBulbTemperature_Max, humidityRatio_Min, humidityRatio_Max);
        }
    }
}
