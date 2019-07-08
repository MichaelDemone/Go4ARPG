using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

	public class GameEventListener : MonoBehaviour {

		public GameEvent Event;
		public UnityEvent Response;
        public bool Debugging = false;
		private void OnEnable() {
            if(Debugging) Debug.Log("Registering: " + Event.name);
			Event.RegisterListener(this);
		}

		private void OnDisable() {
		    if(Debugging)
		        Debug.Log("Deregistering: " + Event.name);
            Event.UnregisterListener(this);
		}

		public void OnEventRaised() {
            if(Debugging)
                Debug.Log("Raised: " + Event.name);
            Response.Invoke();
		}
	}
}
