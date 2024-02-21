namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static ChartParameterType ChartParameterType(this ChartDataType chartDataType, double value)
        {
            if (double.IsNaN(value) || chartDataType == Mollier.ChartDataType.Undefined)
            {
                return Mollier.ChartParameterType.Undefined;
            }

            switch (chartDataType)
            {
                case Mollier.ChartDataType.RelativeHumidity:
                    if (value == 50)
                    {
                        return Mollier.ChartParameterType.MediumLine;
                    }
                    else if (value != 100 && value != 50)
                    {
                        return Mollier.ChartParameterType.Line;
                    }

                    return Mollier.ChartParameterType.BoldLine;

                case Mollier.ChartDataType.DiagramTemperature:
                    return value % 10 != 0 ? Mollier.ChartParameterType.Line : Mollier.ChartParameterType.BoldLine;

                case Mollier.ChartDataType.DryBulbTemperature:
                    return value % 10 != 0 ? Mollier.ChartParameterType.Line : Mollier.ChartParameterType.BoldLine;

                case Mollier.ChartDataType.Enthalpy:
                    return value % 10000 != 0 ? Mollier.ChartParameterType.Line : Mollier.ChartParameterType.BoldLine;

                case Mollier.ChartDataType.Density:
                    return Mollier.ChartParameterType.Line;

                case Mollier.ChartDataType.WetBulbTemperature:
                    return Mollier.ChartParameterType.Line;

                case Mollier.ChartDataType.SpecificVolume:
                    return Mollier.ChartParameterType.Line;

            }

            return Mollier.ChartParameterType.Undefined;
        }
    }
}
