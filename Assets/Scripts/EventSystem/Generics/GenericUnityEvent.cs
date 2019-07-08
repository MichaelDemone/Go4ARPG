using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    public class GenericUnityEvent<T> : UnityEvent<T> {

#if UNITY_EDITOR
        public static bool Debugging = false;
#else
        public static bool Debugging = false;
#endif

        public new void Invoke( T var ) {
            if (Debugging) {
                string output = "";
                output += "Event call\n";
                output += "Calling event with variable: " + var.ToString() + " on following methods:\n";
                for (int i = 0; i < base.GetPersistentEventCount(); i++) {
                    output += GetPersistentMethodName(i);
                }
                Debug.Log(output);
            }
            base.Invoke(var);
        }
    }
}

