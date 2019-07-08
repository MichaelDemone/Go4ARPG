using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Testing {
	[CreateAssetMenu(menuName = "Misc/Test/EventDebugger")]
	public class EventDebugger : ScriptableObject {
		public void Print( string s ) {
			Debug.Log(s);
		}
	}
}

