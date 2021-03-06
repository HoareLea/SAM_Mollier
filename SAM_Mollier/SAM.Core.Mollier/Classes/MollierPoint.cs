using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Core.Mollier
{
    public class MollierPoint : IJSAMObject
    {
        private double dryBulbTemperature;
        private double humidityRatio;
        private double pressure;

        public MollierPoint(double dryBulbTemperature, double humidityRatio, double pressure)
        {
            this.dryBulbTemperature = dryBulbTemperature;
            this.humidityRatio = humidityRatio;
            this.pressure = pressure;
        }

        public MollierPoint(JObject jObject)
        {
            FromJObject(jObject);
        }

        public MollierPoint(MollierPoint mollierPoint)
        {
            if(mollierPoint != null)
            {
                dryBulbTemperature = mollierPoint.dryBulbTemperature;
                humidityRatio = mollierPoint.humidityRatio;
                pressure = mollierPoint.pressure;
            }
        }
        /// <summary>
        /// Enthalpy [J/kg]
        /// </summary>
        public double Enthalpy
        {
            get
            {
                if(!IsValid())
                {
                    return double.NaN;
                }

                return Query.Enthalpy(dryBulbTemperature, humidityRatio);
            }
        }

        public double DryBulbTemperature
        {
            get
            {
                return dryBulbTemperature;
            }
        }

        public double HumidityRatio
        {
            get
            {
                return humidityRatio;
            }
        }

        public double Pressure
        {
            get
            {
                return pressure;
            }
        }

        public double RelativeHumidity
        {
            get
            {
                if(!IsValid())
                {
                    return double.NaN;
                }

                return Query.RelativeHumidity(dryBulbTemperature, humidityRatio, pressure);
            }
        }

        public virtual bool FromJObject(JObject jObject)
        {
            if(jObject == null)
            {
                return false;
            }

            dryBulbTemperature = jObject.ContainsKey("DryBulbTemperature") ? jObject.Value<double>("DryBulbTemperature") : double.NaN;
            humidityRatio = jObject.ContainsKey("HumidityRatio") ? jObject.Value<double>("HumidityRatio") : double.NaN;
            pressure = jObject.ContainsKey("Pressure") ? jObject.Value<double>("Pressure") : double.NaN;

            return true;
        }

        public virtual bool IsValid()
        {
            return !double.IsNaN(dryBulbTemperature) && !double.IsNaN(humidityRatio) && !double.IsNaN(pressure);
        }

        public virtual JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if(!double.IsNaN(dryBulbTemperature))
            {
                jObject.Add("DryBulbTemperature", dryBulbTemperature);
            }

            if (!double.IsNaN(humidityRatio))
            {
                jObject.Add("HumidityRatio", humidityRatio);
            }

            if (!double.IsNaN(pressure))
            {
                jObject.Add("Pressure", pressure);
            }

            return jObject;
        }
    }
}
