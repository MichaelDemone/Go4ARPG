
using G4AW2.Questing;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/Specific/PassiveQuest")]
	public class GameEventPassiveQuest : GameEventGeneric<PassiveQuest, GameEventPassiveQuest, UnityEventPassiveQuest> {
	}
}
