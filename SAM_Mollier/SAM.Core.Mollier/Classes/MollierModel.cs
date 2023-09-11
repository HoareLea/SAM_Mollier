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

        public bool Remove(IMollierObject mollierObject)
        {
            if(mollierObject == null)
            {
                return false;
            }
            bool deleted = false;

            if (mollierObject is IMollierPoint)
            {
                // Remove point from points
                List<IMollierPoint> mollierPoints = GetMollierObjects<IMollierPoint>();
                foreach(IMollierPoint mollierPoint in mollierPoints)
                {
                    if(mollierPoint.AlmostEqual((IMollierPoint)mollierObject))
                    {
                        dictionary[mollierPoint.GetType()].Remove(mollierPoint);
                        deleted = true;
                    }
                }

                // Remove point from groups
                List<MollierGroup> mollierGroups = GetMollierObjects<MollierGroup>();
                foreach (MollierGroup mollierGroup in mollierGroups)
                {
                    List<IMollierPoint> mollierPoints_2 = mollierGroup.GetMollierPoints();

                    foreach (IMollierPoint mollierPoint in mollierPoints_2)
                    {
                        if (mollierPoint.AlmostEqual((IMollierPoint)mollierObject))
                        {
                            mollierGroup.Remove(mollierPoint);
                            deleted = true;
                        }
                    }
                }
            }
            else if (mollierObject is IMollierProcess)
            {
                // Remove process from processes
                List<IMollierProcess> mollierProcesses = GetMollierObjects<IMollierProcess>();
                foreach (IMollierProcess mollierProcess in mollierProcesses)
                { 
                    if (mollierProcess.AlmostEqual((IMollierProcess)mollierObject))
                    {
                        dictionary[mollierProcess.GetType()].Remove(mollierProcess);
                        deleted = true;
                    }
                }

                // Remove process from groups
                List<MollierGroup> mollierGroups = GetMollierObjects<MollierGroup>();
                List<IMollierProcess> newMollierProcesses = new List<IMollierProcess>();

                foreach (MollierGroup mollierGroup in mollierGroups)
                {
                    List<IMollierProcess> mollierProcesses_2 = mollierGroup.GetMollierProcesses();

                    foreach (IMollierProcess mollierProcess in mollierProcesses_2)
                    {
                        if (mollierProcess.AlmostEqual((IMollierProcess)mollierObject))
                        {
                            mollierGroup.Remove(mollierProcess);
                            deleted = true;
                        }
                        else
                        {
                            newMollierProcesses.Add(mollierProcess);
                        }
                    }
                }
                
                List<IMollierGroup> newMollierGroups = Query.Group(newMollierProcesses);
                ClearGroups();
                AddRange(newMollierGroups);

                
            }
            else if (mollierObject is IMollierZone)
            {
                // Remove process from processes
                List<IMollierZone> mollierZones = GetMollierObjects<IMollierZone>();
                foreach (IMollierZone mollierZone in mollierZones)
                {
                    if (mollierZone.AlmostEqual((IMollierZone)mollierObject))
                    {
                        dictionary[mollierZone.GetType()].Remove(mollierZone);
                        deleted = true;
                    }
                }
            }
            else if(mollierObject is IMollierGroup)
            {

            }

            return deleted;
        }
        
        public void ClearGroups()
        {

            if (dictionary != null)
            {
                foreach(KeyValuePair<Type, List<IMollierObject>> keyValuePair in dictionary)
                {
                    if(keyValuePair.Key.Name == "MollierGroup")
                    {
                        dictionary.Remove(keyValuePair.Key);
                        return;
                    }
                }
            }
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
