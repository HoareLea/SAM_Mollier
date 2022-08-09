using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class CoolingProcess : MollierProcess
    {
        internal CoolingProcess(MollierPoint start, MollierPoint end)
            : base(start, end)
        {

        }

        public CoolingProcess(JObject jObject)
            :base(jObject)
        {

        }

        public double CondensationPrecipation()
        {
            MollierPoint start = Start;
            MollierPoint end = End;

            if(start == null || end == null || !start.IsValid() || !end.IsValid())
            {
                return double.NaN;
            }

            return System.Math.Abs(start.HumidityRatio - end.HumidityRatio);
        }

        /// <summary>
        /// ApparatusDewPoint ADP ()
        /// </summary>
        /// <returns>ADP MollierPoint</returns>
        public MollierPoint ApparatusDewPoint()
        {
            MollierPoint start = Start;
            MollierPoint end = End;

            //curve parameters
            double a = (start.DryBulbTemperature - end.DryBulbTemperature) / (start.HumidityRatio - end.HumidityRatio);
            double b = start.DryBulbTemperature - a * start.HumidityRatio;
            double value = end.RelativeHumidity;
            double hum_Temp = end.HumidityRatio;
            while(100 - value < 0.01)
            {
                hum_Temp -= 0.01;
                double dryBulBTemperature = linearFunction(a, b, hum_Temp);
                value = Query.RelativeHumidity(dryBulBTemperature , hum_Temp, start.Pressure);
                if(value == double.NaN)
                {
                    break;
                }
            }
            MollierPoint mollierPoint = new MollierPoint(linearFunction(a, b, hum_Temp), hum_Temp, start.Pressure);
            return mollierPoint;
        }

        public MollierPoint XXXpoint()
        {
            MollierPoint start = Start;
            MollierPoint end = End;

            throw new System.NotImplementedException();

            return null;
        }
        public double linearFunction(double a, double b, double x)
        {
            return a * x + b;
        }
    }
}
