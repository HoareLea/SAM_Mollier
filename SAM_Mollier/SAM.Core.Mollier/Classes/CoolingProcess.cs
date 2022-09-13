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

        public CoolingProcess(CoolingProcess coolingProcess)
            : base(coolingProcess)
        {
            if(coolingProcess != null)
            {
                efficiency = coolingProcess.efficiency;
            }
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
            if(100 - end.RelativeHumidity < 0.05)//end point hit 100% and DewPoint might not follow straight line
            {
                double distance = start.Distance(end) / efficiency - start.Distance(end);//distance vetween end point and dew point
                double dewPointDryBulbTemperature_Temp = end.DryBulbTemperature;
                double dewPointHumidityRatio_Temp = end.HumidityRatio;
                MollierPoint DewPointTemp = new MollierPoint(dewPointDryBulbTemperature_Temp, dewPointHumidityRatio_Temp, Pressure);
                while (distance - end.Distance(DewPointTemp) > 0.01)
                {
                    dewPointHumidityRatio_Temp -= 0.00001;
                    dewPointDryBulbTemperature_Temp = Query.DryBulbTemperature_ByHumidityRatio(dewPointHumidityRatio_Temp, 100, start.Pressure);
                    DewPointTemp = new MollierPoint(dewPointDryBulbTemperature_Temp, dewPointHumidityRatio_Temp, Pressure);
                }
                return DewPointTemp;
            }
            double dewPointDryBulbTemperature = start.DryBulbTemperature - (start.DryBulbTemperature - end.DryBulbTemperature) / efficiency;
            double dewPointHumidityRatio = start.HumidityRatio - (start.HumidityRatio - end.HumidityRatio) / efficiency;
            MollierPoint DewPoint = new MollierPoint(dewPointDryBulbTemperature, dewPointHumidityRatio, Pressure);
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
