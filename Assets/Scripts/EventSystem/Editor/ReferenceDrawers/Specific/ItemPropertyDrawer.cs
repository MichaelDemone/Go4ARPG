
using G4AW2.Data.DropSystem;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<Item, ItemVariable, UnityEventItem>))]
		public class ItemPropertyDrawer : AbstractReferenceDrawer { }
}
