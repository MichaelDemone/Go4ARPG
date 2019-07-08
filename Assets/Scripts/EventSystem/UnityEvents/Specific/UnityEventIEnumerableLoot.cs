using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Events {
	[System.Serializable]
	public class UnityEventIEnumerableLoot : UnityEvent<IEnumerable<Item>> { }
}
