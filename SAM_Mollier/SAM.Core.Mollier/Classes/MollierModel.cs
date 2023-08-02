using System;
using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public class MollierModel : SAMModel
    {
        private MollierSettings mollierSettings;
        private Dictionary<Type, List<IMollierObject>> dictionary;

        public MollierModel(string name, MollierSettings mollierSettings)
            :base(name)
        {
            this.mollierSettings = mollierSettings == null ? null : new MollierSettings(mollierSettings); 
        }

        public bool Add(IMollierObject mollierObject)
        {
            throw new NotImplementedException();
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

    }
}
