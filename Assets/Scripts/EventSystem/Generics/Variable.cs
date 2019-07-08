using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

    [Serializable]
	public abstract class Variable<T, TEvent> : ScriptableObject, ISaveable where TEvent : UnityEvent<T>, ISerializationCallbackReceiver, new() {
		[Multiline]
		public string DeveloperDescription = "";
	    public T InitialValue;
        public Action BeforeChange;
	    public TEvent OnChange = new TEvent();

		[ReadOnly] [SerializeField] private T _value;

		public T Value {
            get { return _value; }
            set {
                BeforeChange?.Invoke();
                _value = value;
                OnChange.Invoke(Value);
            }
        }

        public static implicit operator T( Variable<T, TEvent> val) {
            return val.Value;
        }

	    public void OnEnable() {
		    _value = InitialValue;
		}

		public void OnAfterDeserialization() {
			Debug.Log("After Deserialize");
			_value = InitialValue;
		}

	    public void OnBeforeSerialization() { }

        public virtual string GetSaveString() {
            var ots = new SaveObject();
            ots.ObjectToSave = Value;
            return JsonUtility.ToJson(ots);
        }

        public virtual void SetData( string saveString, params object[] otherData ) {
            var ots = JsonUtility.FromJson<SaveObject>(saveString);
            Value = ots.ObjectToSave;
        }

        private class SaveObject {
            public T ObjectToSave;
        }
    }
}
