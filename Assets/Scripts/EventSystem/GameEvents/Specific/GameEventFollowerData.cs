
using G4AW2.Data;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/Specific/FollowerData")]
	public class GameEventFollowerData : GameEventGeneric<FollowerData, GameEventFollowerData, UnityEventFollowerData> {
	}
}
