using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace CustomEvents {
    public class SaveableVariableWithIID<T, TEvent> : Variable<T, TEvent>
        where TEvent : UnityEvent<T>, ISerializationCallbackReceiver, new()
        where T : IID {

        public override string GetSaveString() {
            SaveObject2 so2 = new SaveObject2();
            if(Value != null && !Value.Equals(default(T))) {
                so2.id = Value.GetID();
            } else {
                so2.id = -1;
            }
            return JsonUtility.ToJson(so2);
        }

        public override void SetData(string saveString, params object[] otherData) {
            SaveObject2 so2 = JsonUtility.FromJson<SaveObject2>(saveString);

            if(so2.id == -1)
                return;

            IEnumerable<IID> allitems = (IEnumerable<IID>) otherData[0];
            Value = (T) allitems.First(item => item.GetID() == so2.id);
        }

        private class SaveObject2 { public int id; }
    }
}
