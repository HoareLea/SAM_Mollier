using Newtonsoft.Json.Linq;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class MollierChartObject : IJSAMObject
    {
        private IUIMollierObject uIMollierObject;

        private ChartType chartType;

        private double z;

        public MollierChartObject(IUIMollierObject uIMollierObject, ChartType chartType, double z)
        {
            this.uIMollierObject = uIMollierObject;
            this.chartType = chartType;
            this.z = z;
        }

        public MollierChartObject(JObject jObject)
        {
            FromJObject(jObject);
        }

        public MollierChartObject(MollierChartObject mollierChartObject)
        { 
            if(mollierChartObject != null)
            {
                uIMollierObject = mollierChartObject.uIMollierObject;
                chartType = mollierChartObject.chartType;
                z = mollierChartObject.z;
            }
        }

        public IUIMollierObject UIMollierObject
        {
            get
            {
                return uIMollierObject?.Clone();
            }
        }

        public ChartType ChartType
        {
            get
            {
                return chartType;
            }
        }

        public double Z
        {
            get
            {
                return z;
            }
        }

        public UIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierObject?.UIMollierAppearance?.Clone();
            }
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("UIMollierObject"))
            {
                uIMollierObject = Core.Query.IJSAMObject< IUIMollierObject >(jObject.Value<JObject>("UIMollierObject"));
            }

            if (jObject.ContainsKey("ChartType"))
            {
                chartType = Core.Query.Enum<ChartType>(jObject.Value<string>("ChartType"));
            }

            if (jObject.ContainsKey("Z"))
            {
                z = jObject.Value<double>("Z");
            }

            return true;
        }
        
        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if(uIMollierObject != null)
            {
                jObject.Add("UIMollierObject", uIMollierObject.ToJObject());
            }

            jObject.Add("ChartType", chartType.ToString());

            if(!double.IsNaN(z))
            {
                jObject.Add("Z", z);
            }

            return jObject;
        }
    }
}
