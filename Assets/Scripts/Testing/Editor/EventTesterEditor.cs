using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace G4AW2.Testing {

    [CustomEditor(typeof(EventTester))]
    public class EventTesterEditor : Editor {

        private SerializedProperty eventProp;
        private SerializedProperty gameEventProp;

        private GUIContent gc = new GUIContent("Event");
        private GUIContent gc2 = new GUIContent("Game Event");
        void OnEnable() {
            // Fetch the objects from the GameObject script to display in the inspector
            eventProp = serializedObject.FindProperty("TestEvent");
            gameEventProp = serializedObject.FindProperty("TestGameEvent");
        }

        public override void OnInspectorGUI() {
            EventTester et = (EventTester) target;

            //The variables and GameObject from the MyGameObject script are displayed in the Inspector with appropriate labels
            EditorGUILayout.PropertyField(eventProp, gc);
            EditorGUILayout.PropertyField(gameEventProp, gc2);

            if (GUILayout.Button(new GUIContent("Raise Event"))) {
                et.InvokeEvent();
            }

            // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }

}

