using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

	public abstract class GameEventGeneric<T, TThis, TUnityEvent> : ScriptableObject 
        where TThis : GameEventGeneric<T, TThis, TUnityEvent>
        where TUnityEvent : UnityEvent<T>{

        private readonly List<GameEventListenerGeneric<T, TThis, TUnityEvent>> listeners = new List<GameEventListenerGeneric<T, TThis, TUnityEvent>>();

        public void Raise(T value) {
            foreach (GameEventListenerGeneric<T, TThis, TUnityEvent> listener in listeners) {
                listener.OnEventRaised(value);
            }
        }

        public void RegisterListener( GameEventListenerGeneric<T, TThis, TUnityEvent> listener ) {
            listeners.Add(listener);
        }

        public void UnregisterListener( GameEventListenerGeneric<T, TThis, TUnityEvent> listener ) {
            listeners.Remove(listener);
        }
    }
}

