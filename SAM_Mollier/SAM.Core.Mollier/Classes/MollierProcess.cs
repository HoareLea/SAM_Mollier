using System.Text.Json.Nodes;
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

        public MollierProcess(JsonObject jObject)
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

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            bool result = base.FromJsonObject(jObject);
            if (!result)
            {
                return false;
            }

            return result;
        }

        public virtual JsonObject ToJsonObject()
        {
            JsonObject result = base.ToJsonObject();
            if(result == null)
            {
                return result;
            }

            return result;
        }
    }
}
