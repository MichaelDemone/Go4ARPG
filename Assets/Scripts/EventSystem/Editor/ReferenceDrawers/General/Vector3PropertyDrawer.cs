
using UnityEngine;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<Vector3, Vector3Variable, UnityEventVector3>))]
		public class Vector3PropertyDrawer : AbstractReferenceDrawer { }
}
