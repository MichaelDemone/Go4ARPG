
using G4AW2.Questing;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/Specific/Quest")]
	public class GameEventQuest : GameEventGeneric<Quest, GameEventQuest, UnityEventQuest> {
	}
}
