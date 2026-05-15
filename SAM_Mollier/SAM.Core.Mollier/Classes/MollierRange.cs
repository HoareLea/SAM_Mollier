using System.Text.Json.Nodes;
namespace SAM.Core.Mollier
{
    public class MollierRange : IJSAMObject
    {
        private Range<double> dryBulbTemperatureRange;
        private Range<double> humidityRatioRange;

        public MollierRange(double dryBulbTemperature_Start, double dryBulbTemperature_End, double humidityRatio_Start, double humidityRatio_End)
        {
            dryBulbTemperatureRange = new Range<double>(dryBulbTemperature_Start, dryBulbTemperature_End);
            humidityRatioRange = new Range<double>(humidityRatio_Start, humidityRatio_End);
        }

        public MollierRange(MollierRange mollierRange)
        {
            dryBulbTemperatureRange = mollierRange?.dryBulbTemperatureRange == null ? null : new Range<double>(mollierRange.dryBulbTemperatureRange);
            humidityRatioRange = mollierRange?.humidityRatioRange == null ? null : new Range<double>(mollierRange.humidityRatioRange);
        }
        
        public MollierRange(JsonObject jObject)
        {
            FromJsonObject(jObject);
        }

        public double DryBulbTemperature_Max
        {
            get
            {
                return dryBulbTemperatureRange == null? double.NaN: dryBulbTemperatureRange.Max;
            }
        }

        public double DryBulbTemperature_Min
        {
            get
            {
                return dryBulbTemperatureRange == null ? double.NaN : dryBulbTemperatureRange.Min;
            }
        }

        public double HumidityRatio_Max
        {
            get
            {
                return humidityRatioRange == null ? double.NaN : humidityRatioRange.Max;
            }
        }

        public double HumidityRatio_Min
        {
            get
            {
                return humidityRatioRange == null ? double.NaN : humidityRatioRange.Min;
            }
        }

        public Range<double> DryBulbTemperatureRange
        {
            get
            {
                return dryBulbTemperatureRange == null ? null : new Range<double>(dryBulbTemperatureRange);
            }
        }

        public Range<double> HumidityRatioRange
        {
            get
            {
                return humidityRatioRange == null ? null : new Range<double>(humidityRatioRange);
            }
        }

        public bool IsValid()
        {
            return dryBulbTemperatureRange != null && humidityRatioRange != null && !double.IsNaN(dryBulbTemperatureRange.Max) && !double.IsNaN(dryBulbTemperatureRange.Min) && !double.IsNaN(humidityRatioRange.Max) && !double.IsNaN(humidityRatioRange.Min);
        }

        public virtual bool FromJsonObject(JsonObject jObject)
        {
            if(jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("DryBulbTemperatureRange"))
            {
                dryBulbTemperatureRange = new Range<double>(jObject["DryBulbTemperatureRange"] as JsonObject);
            }

            if (jObject.ContainsKey("HumidityRatioRange"))
            {
                humidityRatioRange = new Range<double>(jObject["HumidityRatioRange"] as JsonObject);
            }

            return true;
        }

        public virtual JsonObject ToJsonObject()
        {
            JsonObject jObject = new JsonObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));
            
            if(dryBulbTemperatureRange != null)
            {
                jObject.Add("DryBulbTemperatureRange", dryBulbTemperatureRange.ToJsonObject());
            }

            if (humidityRatioRange != null)
            {
                jObject.Add("HumidityRatioRange", humidityRatioRange.ToJsonObject());
            }

            return jObject;
        }
    }
}

