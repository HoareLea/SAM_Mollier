using Newtonsoft.Json.Linq;
using SAM.Core.Mollier;
using System;

namespace SAM.Weather.Mollier
{
    public class WeatherMollierPoint : MollierPoint
    {
        private DateTime dateTime;

        public WeatherMollierPoint(MollierPoint mollierPoint, DateTime dateTime)
            :base(mollierPoint)
        {
            this.dateTime = dateTime;
        }

        public WeatherMollierPoint(WeatherMollierPoint weatherMollierPoint)
            : base(weatherMollierPoint)
        {
            if(weatherMollierPoint != null)
            {
                dateTime = weatherMollierPoint.dateTime;
            }
        }

        public WeatherMollierPoint(JObject jObject)
            : base(jObject)
        {

        }

        public DateTime DateTime
        {
            get
            {
                return dateTime;
            }
        }

        public override JObject ToJObject()
        {
            JObject result = base.ToJObject();
            if(result == null)
            {
                return null;
            }

            result.Add("DateTime", dateTime);

            return result;
        }

        public override bool FromJObject(JObject jObject)
        {
            if(!base.FromJObject(jObject))
            {
                return false;
            }

            if(jObject.ContainsKey("DateTime"))
            {
                dateTime = jObject.Value<DateTime>("DateTime");
            }

            return true;
        }
    }
}
