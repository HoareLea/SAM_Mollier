using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class CoolingProcess : MollierProcess
    {
        private double efficiency = 1;
        internal CoolingProcess(MollierPoint start, MollierPoint end, double efficiency)
            : base(start, end)
        {
            this.efficiency = efficiency;
        }

        public CoolingProcess(JObject jObject)
            :base(jObject)
        {

        }

        public double Efficiency
        {
            get
            {
                return efficiency;
            }
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

            double dewPointY = start.DryBulbTemperature - (start.DryBulbTemperature - end.DryBulbTemperature) / efficiency;
            double dewPointX = start.HumidityRatio - (start.HumidityRatio - end.HumidityRatio) / efficiency;
            MollierPoint DewPoint = new MollierPoint(dewPointY, dewPointX, Pressure);
            return DewPoint;
        }

        public MollierPoint DownPoint(MollierPoint dewPoint)
        {
            MollierPoint start = Start;
            MollierPoint end = End;

            double dryBulbTemperatureDew = start.DryBulbTemperature - efficiency * (start.DryBulbTemperature - Mollier.Query.DryBulbTemperature_ByHumidityRatio(start.HumidityRatio, 100, start.Pressure));

            MollierPoint downPoint = new MollierPoint(dryBulbTemperatureDew, start.HumidityRatio, start.Pressure);
            return downPoint;
        }
        public double linearFunction(double a, double b, double x)
        {
            return a * x + b;
        }
        public double CountDistance(MollierPoint start, MollierPoint end)
        {
            return System.Math.Sqrt((System.Math.Pow(start.DryBulbTemperature - end.DryBulbTemperature, 2)) + System.Math.Pow(start.HumidityRatio - end.HumidityRatio, 2));
        }

        public override bool FromJObject(JObject jObject)
        {
            if(!base.FromJObject(jObject))
            {
                return false;
            }

            if(jObject.ContainsKey("Efficiency"))
            {
                efficiency = jObject.Value<double>("Efficiency");
            }

            return true;
        }

        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return result;
            }

            if(!double.IsNaN(efficiency))
            {
                result.Add("Efficiency", efficiency);
            }

            return result;
        }
    }
}
