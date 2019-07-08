

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<int, IntVariable, UnityEventInt>))]
		public class IntPropertyDrawer : AbstractReferenceDrawer { }
}
