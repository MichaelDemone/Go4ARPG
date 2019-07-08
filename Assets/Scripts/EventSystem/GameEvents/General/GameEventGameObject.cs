using G4AW2.Data.DropSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CustomEvents
{

    [Serializable]
    [CreateAssetMenu(menuName = "SO Architecture/Events/General/GameObject")]
    public class GameEventGameObject : ScriptableObject
    {
        private readonly List<GameEventListenerGameObject> listeners = new List<GameEventListenerGameObject>();

        public void Raise(GameObject obj)
        {
            foreach (GameEventListenerGameObject listener in listeners)
            {
                listener.OnEventRaised(obj);
            }
        }

        public void RegisterListener(GameEventListenerGameObject listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListenerGameObject listener)
        {
            listeners.Remove(listener);
        }
    }

}
