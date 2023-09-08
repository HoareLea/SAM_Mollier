using System;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class MollierModel : SAMModel
    {
        private MollierSettings mollierSettings;
        private Dictionary<Type, List<IMollierObject>> dictionary;

        public MollierModel()
        {
            dictionary = new Dictionary<Type, List<IMollierObject>>();
        }
        public MollierModel(string name, MollierSettings mollierSettings)
            :base(name)
        {
            this.mollierSettings = mollierSettings == null ? null : new MollierSettings(mollierSettings);
            dictionary = new Dictionary<Type, List<IMollierObject>>();
        }
        public MollierModel(string name, MollierSettings mollierSettings, Dictionary<Type, List<IMollierObject>> dictionary)
            :base(name)
        {
            this.mollierSettings = mollierSettings == null ? null : new MollierSettings(mollierSettings);
            this.dictionary = dictionary;
        }

        public bool Add(IMollierObject mollierObject)
        {
            if(mollierObject == null)
            {
                return false;
            }

            if(!dictionary.ContainsKey(mollierObject.GetType()))
            {
                dictionary[mollierObject.GetType()] = new List<IMollierObject>();
            }
            dictionary[mollierObject.GetType()].Add(mollierObject);

            return true;
        }

        public bool AddRange(IEnumerable<IMollierObject> mollierObjects)
        {
            if(mollierObjects == null)
            {
                return false;
            }

            foreach(IMollierObject mollierObject in mollierObjects)
            {
                Add(mollierObject);
            }
            return true;
        }

        public void Regenerate()
        {
            if(dictionary == null || dictionary.Count == 0)
            {
                return;
            }
        }


        public List<T> GetMollierObjects<T>() where T : IMollierObject
        {
            if(dictionary == null || dictionary.Count == 0)
            {
                return null;
            }

            List<T> result = new List<T>();
            foreach(KeyValuePair<Type, List<IMollierObject>> keyValuePair in dictionary)
            {
                if(keyValuePair.Value == null || keyValuePair.Value.Count == 0)
                {
                    continue;
                }

                if(!typeof(T).IsAssignableFrom(keyValuePair.Key))
                {
                    continue;
                }

                foreach(IMollierObject mollierObject in keyValuePair.Value)
                {
                    if(mollierObject == null)
                    {
                        continue;
                    }

                    result.Add((T)(object)mollierObject);
                }
            }

            return result;
        }

        public void Clear()
        {
            if (dictionary == null)
            {
                return;
            }
            dictionary.Clear();
        }

    }
}
