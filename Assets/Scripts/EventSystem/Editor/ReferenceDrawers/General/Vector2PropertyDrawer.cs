
using UnityEngine;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<Vector2, Vector2Variable, UnityEventVector2>))]
		public class Vector2PropertyDrawer : AbstractReferenceDrawer { }
}
