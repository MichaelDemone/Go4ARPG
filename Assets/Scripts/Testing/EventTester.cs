using CustomEvents;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
namespace G4AW2.Testing {
	public class EventTester : MonoBehaviour {
        public UnityEvent TestEvent;
        public GameEvent TestGameEvent;

        public void InvokeEvent() {
            print("Invoking test event on object: " + gameObject.name);
            TestEvent.Invoke();
            if(TestGameEvent != null) TestGameEvent.Raise();
        }
    }
}
#endif
