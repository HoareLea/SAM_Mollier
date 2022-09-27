using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SAM.Core.Mollier
{
    public class MollierGroup : Collection<IMollierGroupable>, IMollierGroup
    {
        public string Name { get; set; }

        public MollierGroup(string name)
        {
            Name = name;
        }

        public MollierGroup(MollierGroup mollierGroup)
        {
            if(mollierGroup != null)
            {
                foreach(IMollierGroupable mollierGroupable in mollierGroup)
                {
                    Add(mollierGroupable);
                }
            }
        }

        public MollierGroup(JObject jObject)
        {
            FromJObject(jObject);
        }

        public List<T> GetObjects<T>(bool includeNestedObjects = true) where T: IMollierGroupable
        {
            List<T> result = new List<T>();
            foreach(IMollierGroupable mollierGroupable in this)
            {
                if(mollierGroupable is T)
                {
                    result.Add((T)mollierGroupable);
                }

                if(includeNestedObjects)
                {
                    if(mollierGroupable is MollierGroup)
                    {
                        List<T> objects = ((MollierGroup)mollierGroupable).GetObjects<T>();
                        if(objects != null)
                        {
                            result.AddRange(objects);
                        }
                    }
                }
            }

            return result;
        }

        public List<IMollierProcess> GetMollierProcesses()
        {
            return GetObjects<IMollierProcess>(true);
        }

        public List<IMollierPoint> GetMollierPoints()
        {
            return GetObjects<IMollierPoint>(true);
        }

        public bool FromJObject(JObject jObject)
        {
            if(jObject == null)
            {
                return false;
            }

            if(jObject.ContainsKey("Name"))
            {
                Name = jObject.Value<string>("Name");
            }

            if(jObject.ContainsKey("Objects"))
            {
                List<IMollierGroupable> mollierGroupables =  Core.Create.IJSAMObjects<IMollierGroupable>(jObject.Value<JArray>("Objects"));
                mollierGroupables?.ForEach(x => Add(x));
            }

            return true;
        }

        public IEnumerator<IMollierGroupable> GetEnumerator()
        {
            return base.GetEnumerator();
        }

        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if(Name != null)
            {
                jObject.Add("Name", Name);
            }

            JArray jArray = new JArray();
            foreach (IMollierGroupable mollierGroupable in this)
            {
                jArray.Add(mollierGroupable.ToJObject());
            }

            jObject.Add("Objects", jArray);

            return jObject;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
