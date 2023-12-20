using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace SAM.Core.Mollier
{
    public abstract class MollierProcess : MollierCurve, IMollierProcess
    {
        internal MollierProcess(MollierPoint start, MollierPoint end)
            :base(start, end)
        {

        }

        public MollierProcess(MollierProcess mollierProcess)
            :base(mollierProcess)
        {

        }

        public MollierProcess(JObject jObject)
            :base(jObject)
        {

        }

        public virtual void Scale(double factor)
        {
            if(Start == null || End == null)
            {
                return;
            }

            double dryBulbTemperatureDifference = Start.DryBulbTemperature - End.DryBulbTemperature;
            double humidityRatioDifference = Start.HumidityRatio - End.HumidityRatio;

            mollierPoints[1] = new MollierPoint(Start.DryBulbTemperature - (dryBulbTemperatureDifference * factor), Start.HumidityRatio - (humidityRatioDifference * factor), Start.Pressure);
        }

        public virtual bool FromJObject(JObject jObject)
        {
            bool result = base.FromJObject(jObject);
            if (!result)
            {
                return false;
            }

            return result;
        }

        public virtual JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return result;
            }

            return result;
        }
    }
}
