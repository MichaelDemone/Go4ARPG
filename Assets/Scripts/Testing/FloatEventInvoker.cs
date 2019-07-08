using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;

public class FloatEventInvoker : MonoBehaviour {

	public UnityEventFloat Event;
	public float valueToInvokeWith;

	[ContextMenu("Invoke")]
	public void TestInvoke() {
		Event.Invoke(valueToInvokeWith);
	}
}
