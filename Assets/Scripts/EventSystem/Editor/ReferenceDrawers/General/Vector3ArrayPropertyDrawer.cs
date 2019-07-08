
using UnityEngine;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<Vector3[], Vector3ArrayVariable, UnityEventVector3Array>))]
		public class Vector3ArrayPropertyDrawer : AbstractReferenceDrawer { }
}
