using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using UnityEngine;
using UnityEngine.Events;

public class RuntimeSetGenericSaveableWithSaveableAndIID<T, TEvent> : RuntimeSetGeneric<T, TEvent>
    where TEvent : UnityEvent<T>
    where T : ScriptableObject, ISaveable, IID {

    [System.Serializable]
    private class SaveObject {
        public List<int> IDs = new List<int>();
        public List<string> Data = new List<string>();
    }

    public override string GetSaveString() {
        SaveObject so = new SaveObject();

        for(int i = 0; i < Value.Count; i++) {
            T val = Value[i];
            so.IDs.Add(val.GetID());
            so.Data.Add(val.GetSaveString());
        }

        return JsonUtility.ToJson(so);
    }

    public override void SetData(string saveString, params object[] otherData) {
        Clear();
        SaveObject loadedInfo = JsonUtility.FromJson<SaveObject>(saveString);

        if (!(otherData[0] is PersistentSetGeneric<T, TEvent>)) {
            return;
        }

        PersistentSetGeneric<T, TEvent> allThings = otherData[0] as PersistentSetGeneric<T, TEvent>;

        for(int i = 0; i < loadedInfo.IDs.Count; i++) {

            int id = loadedInfo.IDs[i];
            string data = loadedInfo.Data[i];

            T item = allThings.FirstOrDefault(thing => thing.GetID() == id);
            if (item == null) {
                Debug.LogWarning("Tried to load a thing that doesn't exist with ID: " + id);
                continue;
            }

            T newItem = Instantiate(item);
            newItem.SetData(data, item);
            Add(newItem);
        }
    }
}
