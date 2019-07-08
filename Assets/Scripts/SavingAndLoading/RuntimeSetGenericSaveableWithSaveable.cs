using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    public abstract class RuntimeSetGenericSaveableWithSaveable<T, TEvent> : RuntimeSetGeneric<T, TEvent>
        where TEvent : UnityEvent<T>
        where T : ScriptableObject, ISaveable {

        [System.Serializable]
        private struct SaveObject {
            public List<string> List;
        }

        public override string GetSaveString() {
            SaveObject so = new SaveObject();
            so.List = new List<string>();

            foreach(T val in Value) {
                so.List.Add(val.GetSaveString());
            }

            return JsonUtility.ToJson(so);
        }

        public override void SetData(string saveString, params object[] otherData) {
            Clear();
            List<string> loadedStrings = JsonUtility.FromJson<SaveObject>(saveString).List;
            if (loadedStrings == null) return;

            foreach(string loadedObject in loadedStrings) {
                T newItem = CreateInstance<T>();
                newItem.SetData(loadedObject, otherData[0]);
                Add(newItem);
            }
        }
    }

}
