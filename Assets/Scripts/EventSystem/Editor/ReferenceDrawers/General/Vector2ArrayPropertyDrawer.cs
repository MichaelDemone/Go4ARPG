
using UnityEngine;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<Vector2[], Vector2ArrayVariable, UnityEventVector2Array>))]
		public class Vector2ArrayPropertyDrawer : AbstractReferenceDrawer { }
}
