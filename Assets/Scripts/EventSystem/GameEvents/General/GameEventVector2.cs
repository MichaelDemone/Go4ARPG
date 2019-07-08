
using UnityEngine;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/General/Vector2")]
	public class GameEventVector2 : GameEventGeneric<Vector2, GameEventVector2, UnityEventVector2> {
	}
}
