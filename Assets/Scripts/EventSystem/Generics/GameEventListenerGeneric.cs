using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    public abstract class GameEventListenerGeneric<T, TGameEvent, TUnityEvent> : MonoBehaviour 
        where TGameEvent : GameEventGeneric<T, TGameEvent, TUnityEvent>
        where TUnityEvent : UnityEvent<T> {

        public TGameEvent Event;
        public TUnityEvent Response;

        private void OnEnable() {
            Event.RegisterListener(this);
        }

        private void OnDisable() {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(T value) {
            Response.Invoke(value);
        }
    }
}

