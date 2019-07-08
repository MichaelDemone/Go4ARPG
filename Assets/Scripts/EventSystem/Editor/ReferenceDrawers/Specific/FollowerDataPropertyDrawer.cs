
using G4AW2.Data;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<FollowerData, FollowerDataVariable, UnityEventFollowerData>))]
		public class FollowerDataPropertyDrawer : AbstractReferenceDrawer { }
}
