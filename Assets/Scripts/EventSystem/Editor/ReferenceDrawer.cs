using G4AW2.Events;
using UnityEditor;
using UnityEngine;

namespace CustomEvents.Editor {
	
	public abstract class AbstractReferenceDrawer : PropertyDrawer {

		SerializedProperty activeProperty;
		void SetConstant() {
			activeProperty.FindPropertyRelative("UseConstant").boolValue = true;
			activeProperty.serializedObject.ApplyModifiedProperties();
		}

		void SetVariable() {
			activeProperty.FindPropertyRelative("UseConstant").boolValue = false;
			activeProperty.serializedObject.ApplyModifiedProperties();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			EditorGUI.BeginProperty(position, label, property);

			Event e = Event.current;

			if (e.type == EventType.MouseDown && e.button == 1 && position.Contains(e.mousePosition)) {

				property.serializedObject.Update();

				activeProperty = property;
				GenericMenu context = new GenericMenu();

				context.AddItem(new GUIContent("Constant"), property.FindPropertyRelative("UseConstant").boolValue, SetConstant);
				context.AddItem(new GUIContent("Variable"), !property.FindPropertyRelative("UseConstant").boolValue, SetVariable);

				context.ShowAsContext();
			}

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var floatConst = new Rect(position.x, position.y, position.width, position.height);

			if (property.FindPropertyRelative("UseConstant").boolValue) {
				EditorGUI.PropertyField(floatConst, property.FindPropertyRelative("ConstantValue"), GUIContent.none, true);
			}
			else {
				EditorGUI.PropertyField(floatConst, property.FindPropertyRelative("Variable"), GUIContent.none, true);
			}

			EditorGUI.indentLevel = indent;


			EditorGUI.EndProperty();
		}
	}


}

