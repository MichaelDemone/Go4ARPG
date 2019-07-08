
using G4AW2.Data.DropSystem;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/Specific/Weapon")]
	public class GameEventWeapon : GameEventGeneric<Weapon, GameEventWeapon, UnityEventWeapon> {
	}
}
