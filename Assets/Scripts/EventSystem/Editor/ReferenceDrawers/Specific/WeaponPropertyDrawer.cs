
using G4AW2.Data.DropSystem;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<Weapon, WeaponVariable, UnityEventWeapon>))]
		public class WeaponPropertyDrawer : AbstractReferenceDrawer { }
}
