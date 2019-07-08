using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    public abstract class RuntimeSetGenericSaveableWithIID<T, TEvent> : RuntimeSetGeneric<T, TEvent>
        where TEvent : UnityEvent<T>
        where T : ScriptableObject, IID {

        [System.Serializable]
        private struct SaveObject {
            public List<int> List;
        }

        public override string GetSaveString() {

            SaveObject so = new SaveObject();
            so.List = Value.Select(iid => iid.GetID()).ToList();

            return JsonUtility.ToJson(so);
        }

        public override void SetData(string saveString, params object[] otherData) {
            Clear();

            List<T> listOfAllPossibleObjects = ((PersistentSetGeneric<T, TEvent>) otherData[0]).ToList();
            List<int> ids = JsonUtility.FromJson<SaveObject>(saveString).List;

            foreach(int id in ids) {

                T matchingItem = listOfAllPossibleObjects.FirstOrDefault(o => o.GetID() == id);

                if(matchingItem != null && !matchingItem.Equals(default(T))) {
                    Add(matchingItem);
                } else {
                    Debug.LogWarning("Tried to load an ID that does not exist - " + id);
                }
            }
        }
    }

}
