using Newtonsoft.Json.Linq;

namespace SAM.Core.Mollier
{
    public class MollierPoint : IMollierPoint
    {
        private double dryBulbTemperature;
        private double humidityRatio;
        private double pressure;

        //// Constants used in calculations
        //private double vapourizationLatentHeat = Zero.VapourizationLatentHeat;
        //private double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour;
        //private double specificHeat_Air = Zero.SpecificHeat_Air;
        //private double specificHeat_Water = Zero.SpecificHeat_Water;


        /// <summary>
        /// Create new MollierPoint
        /// </summary>
        /// <param name="dryBulbTemperature">Dry-bulb temperature [°C]</param>
        /// <param name="humidityRatio">Humidity Ratio [kg_waterVapor/kg_dryAir]</param>
        /// <param name="pressure">Atmospheric pressure [Pa]</param>
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

        ///// <summary>
        ///// Constructor with ability to change constants, default is 0 degrees constants
        ///// </summary>
        ///// <param name="mollierPoint"></param>
        ///// <param name="vapourizationLatentHeat"></param>
        ///// <param name="specificHeat_WaterVapour"></param>
        ///// <param name="specificHeat_Air"></param>
        ///// <param name="specificHeat_Water"></param>
        //public MollierPoint(MollierPoint mollierPoint, double vapourizationLatentHeat = Zero.VapourizationLatentHeat, double specificHeat_WaterVapour = Zero.SpecificHeat_WaterVapour, double specificHeat_Air = Zero.SpecificHeat_Air, double specificHeat_Water = Zero.SpecificHeat_Water)
        //{
        //    if (mollierPoint != null)
        //    {
        //        dryBulbTemperature = mollierPoint.dryBulbTemperature;
        //        humidityRatio = mollierPoint.humidityRatio;
        //        pressure = mollierPoint.pressure;
        //    }
        //    if (!double.IsNaN(vapourizationLatentHeat))
        //    {
        //        this.vapourizationLatentHeat = vapourizationLatentHeat;
        //    }
        //    if (!double.IsNaN(specificHeat_WaterVapour))
        //    {
        //        this.specificHeat_WaterVapour = specificHeat_WaterVapour;
        //    }
        //    if (!double.IsNaN(specificHeat_Air))
        //    {
        //        this.specificHeat_Air = specificHeat_Air;
        //    }
        //    if (!double.IsNaN(specificHeat_Water))
        //    {
        //        this.specificHeat_Water = specificHeat_Water;
        //    }
        //}

        public MollierPoint(MollierPoint mollierPoint)
        {
            if (mollierPoint != null)
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

                return Query.Enthalpy(dryBulbTemperature, humidityRatio, pressure);
            }
        }

        /// <summary>
        /// Dry Bulb Temperature [C]
        /// </summary>
        public double DryBulbTemperature
        {
            get
            {
                return dryBulbTemperature;
            }
        }

        /// <summary>
        /// Humidity Ratio [kg_waterVapor/kg_dryAir]
        /// </summary>
        public double HumidityRatio
        {
            get
            {
                return humidityRatio;
            }
        }

        /// <summary>
        /// Pressure [Pa]
        /// </summary>
        public double Pressure
        {
            get
            {
                return pressure;
            }
        }

        /// <summary>
        /// Relative Humidity [%]
        /// </summary>
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
            return !double.IsNaN(dryBulbTemperature) && !double.IsNaN(humidityRatio) && !double.IsNaN(pressure) && !double.IsInfinity(dryBulbTemperature) && !double.IsInfinity(humidityRatio) && !double.IsInfinity(pressure);
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

        public double this[MollierPointProperty mollierPointProperty]
        {
            get
            {
                switch(mollierPointProperty)
                {
                    case MollierPointProperty.Undefined: 
                        return double.NaN;

                    case MollierPointProperty.DryBulbTemperature:
                        return dryBulbTemperature;

                    case MollierPointProperty.WetBulbTemperature:
                        return Query.WetBulbTemperature(this);

                    case MollierPointProperty.DewPointTemperature:
                        return Query.DewPointTemperature(this);

                    case MollierPointProperty.DiagramTemperature:
                        return Query.DiagramTemperature(this);

                    case MollierPointProperty.RelativeHumidity:
                        return Query.RelativeHumidity(dryBulbTemperature, humidityRatio, pressure);

                    case MollierPointProperty.Density:
                        return Query.Density(this);

                    case MollierPointProperty.Enthalpy:
                        return Query.Enthalpy(dryBulbTemperature, humidityRatio, pressure);

                    case MollierPointProperty.HumidityRatio:
                        return humidityRatio;

                    case MollierPointProperty.SpecificVolume:
                        return Query.SpecificVolume(this);

                    default:
                        return double.NaN;

                }
            }
        }
    }
}
