
using G4AW2.Data.DropSystem;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/Specific/Armor")]
	public class GameEventArmor : GameEventGeneric<Armor, GameEventArmor, UnityEventArmor> {
	}
}
