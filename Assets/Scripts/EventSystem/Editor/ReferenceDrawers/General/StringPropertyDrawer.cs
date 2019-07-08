

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<string, StringVariable, UnityEventString>))]
		public class StringPropertyDrawer : AbstractReferenceDrawer { }
}
