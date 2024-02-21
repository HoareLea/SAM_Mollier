using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SAM.Core.Mollier
{
    public class VisibilitySettings : IJSAMObject
    {
        Dictionary<string, List<IVisibilitySetting>> dictionary;

        public VisibilitySettings()
        {

        }

        public VisibilitySettings(VisibilitySettings visibilitySettings)
        {
            if (visibilitySettings?.dictionary != null)
            {
                dictionary = new Dictionary<string, List<IVisibilitySetting>>();
                foreach (KeyValuePair<string, List<IVisibilitySetting>> keyValuePair in visibilitySettings.dictionary)
                {
                    dictionary[keyValuePair.Key] = keyValuePair.Value;
                }
            }
        }

        public VisibilitySettings(JObject jObject)
        {
            FromJObject(jObject);
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }


            if (jObject.ContainsKey("Templates"))
            {
                dictionary = new Dictionary<string, List<IVisibilitySetting>>();
                JArray jArray = jObject.Value<JArray>("Templates");
                foreach (JObject jObject_Temp in jArray)
                {
                    if (!jObject_Temp.ContainsKey("TemplateName") || !jObject_Temp.ContainsKey("VisibilitySettings"))
                    {
                        continue;
                    }

                    string templateName = jObject_Temp.Value<string>("TemplateName");
                    JArray jArray_VisibilitySettings = jObject_Temp.Value<JArray>("VisibilitySettings");

                    foreach (JObject jObject_VisibilitySetting in jArray_VisibilitySettings)
                    {
                        if (!dictionary.TryGetValue(templateName, out List<IVisibilitySetting> visibilitySettings))
                        {
                            visibilitySettings = new List<IVisibilitySetting>();
                            dictionary[templateName] = visibilitySettings;
                        }

                        visibilitySettings.Add(new JSAMObjectWrapper(jObject_VisibilitySetting).ToIJSAMObject() as IVisibilitySetting);
                    }
                }
            }

            return true;
        }

        public JObject ToJObject()
        {
            JObject result = new JObject();

            if (dictionary != null)
            {
                JArray jArray = new JArray();
                foreach (KeyValuePair<string, List<IVisibilitySetting>> keyValuePair in dictionary)
                {
                    JObject jObject_Template = new JObject();


                    string templateName = keyValuePair.Key;
                    jObject_Template.Add("TemplateName", templateName);

                    JArray jArray_VisibilitySettings = new JArray();
                    foreach (IVisibilitySetting visibilitySetting in keyValuePair.Value)
                    {
                        jArray_VisibilitySettings.Add(visibilitySetting.ToJObject());
                    }

                    jObject_Template.Add("VisibilitySettings", jArray_VisibilitySettings);

                    jArray.Add(jObject_Template);

                }
                result.Add("Templates", jArray);
            }

            return result;
        }

        public bool Add(IVisibilitySetting visibilitySetting)
        {
            return Add(String.Empty, visibilitySetting);
        }

        public bool Add(string templateName, IVisibilitySetting visibilitySetting)
        {
            if (visibilitySetting == null)
            {
                return false;
            }

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, List<IVisibilitySetting>>();
            }

            if (!dictionary.TryGetValue(templateName, out List<IVisibilitySetting> visibilitySettingList))
            {
                visibilitySettingList = new List<IVisibilitySetting>();
                dictionary[templateName] = visibilitySettingList;

            }

            visibilitySettingList.Add(visibilitySetting);
            return true;
        }

        public IVisibilitySetting GetVisibilitySetting(ChartParameterType chartParameterType, ChartDataType chartDataType = ChartDataType.Undefined)
        {
            return GetVisibilitySetting(String.Empty, chartParameterType, chartDataType);
        }

        public IVisibilitySetting GetVisibilitySetting(string templateName, ChartParameterType chartParameterType, ChartDataType chartDataType = ChartDataType.Undefined)//GradientPoint == true when this mode is on
        {
            if (dictionary == null)
            {
                return null;
            }

            if (!dictionary.TryGetValue(templateName, out List<IVisibilitySetting> visibilitySettings) || visibilitySettings == null)
            {
                return null;
            }

            IVisibilitySetting result = visibilitySettings.Find(x => x is IUserVisibilitySetting && ((IUserVisibilitySetting)x).ChartParameterType == chartParameterType && ((IUserVisibilitySetting)x).ChartDataType == chartDataType);

            if (result == null)
            {
                result = visibilitySettings.Find(x => x is IChartVisibilitySetting && ((IChartVisibilitySetting)x).ChartParameterType == chartParameterType && ((IChartVisibilitySetting)x).ChartDataType == chartDataType);
            }
            return result;

        }

        public System.Drawing.Color GetColor(string templateName, ChartParameterType chartParameterType, ChartDataType chartDataType = ChartDataType.Undefined)
        {
            IVisibilitySetting visibilitySetting = GetVisibilitySetting(templateName, chartParameterType, chartDataType);
            if (visibilitySetting == null)
            {
                return System.Drawing.Color.Empty;
            }

            return visibilitySetting.Color;
        }

        public System.Drawing.Color GetColor(string templateName, ChartParameterType chartParameterType, IMollierProcess mollierProcess)
        {
            ChartDataType chartDataType = mollierProcess.ChartDataType();

            IVisibilitySetting visibilitySetting = GetVisibilitySetting(templateName, chartParameterType, chartDataType);
            if (visibilitySetting == null)
            {
                return System.Drawing.Color.Empty;
            }
            return visibilitySetting.Color;
        }

        public bool GetVisible(string templateName, ChartDataType chartDataType, double value)
        {
            ChartParameterType chartParameterType = Query.ChartParameterType(chartDataType, value);
            if (chartParameterType == ChartParameterType.Undefined)
            {
                return true;
            }

            IVisibilitySetting visibilitySetting = GetVisibilitySetting(templateName, chartParameterType, chartDataType);
            if (visibilitySetting == null)
            {
                return true;
            }

            return visibilitySetting.Visible;
        }

        public List<T> GetVisibilitySettings<T>(string templateName) where T : IVisibilitySetting
        {
            if (dictionary == null)
            {
                return null;
            }

            if (!dictionary.TryGetValue(templateName, out List<IVisibilitySetting> visibilitySettings) || visibilitySettings == null)
            {
                return null;
            }

            return visibilitySettings.FindAll(x => x is T).ConvertAll(x => (T)x);
        }

        public void SetVisibilitySettings<T>(string templateName, List<T> visibilitySettings) where T : IVisibilitySetting
        {
            if (visibilitySettings == null || string.IsNullOrWhiteSpace(templateName))
            {
                return;
            }

            if (dictionary == null)
            {
                dictionary = new Dictionary<string, List<IVisibilitySetting>>();
            }

            dictionary[templateName] = new List<IVisibilitySetting>(visibilitySettings.Cast<IVisibilitySetting>());
        }
    }
}
