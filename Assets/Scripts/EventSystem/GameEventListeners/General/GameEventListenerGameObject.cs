using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents
{

    [System.Serializable]
    public class myEvent : UnityEvent<GameObject>
    {
    }

    public class GameEventListenerGameObject : MonoBehaviour
    {

        public GameEventGameObject Event;
        public myEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(GameObject obj)
        {

            Response.Invoke(obj);
        }
    }
}