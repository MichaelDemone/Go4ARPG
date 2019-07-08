
using UnityEngine;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/General/Vector3Array")]
	public class GameEventVector3Array : GameEventGeneric<Vector3[], GameEventVector3Array, UnityEventVector3Array> {
	}
}
