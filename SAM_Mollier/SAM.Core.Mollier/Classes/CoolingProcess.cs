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

        //public double EvaporatingTemperature(double tolerance = Tolerance.Distance)
        //{
        //    if (Core.Query.AlmostEqual(Start.HumidityRatio, End.HumidityRatio, tolerance))
        //    {
        //        return Query.DryBulbTemperature_ByHumidityRatio(Start.HumidityRatio, 100, Start.Pressure);
        //    }
        //}
    }
}
