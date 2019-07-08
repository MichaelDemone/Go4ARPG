
using G4AW2.Data.DropSystem;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<Armor, ArmorVariable, UnityEventArmor>))]
		public class ArmorPropertyDrawer : AbstractReferenceDrawer { }
}
