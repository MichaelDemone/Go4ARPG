using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConvertAnimationToSpriteRenderer : EditorWindow {

    [MenuItem("Tools/Image to Sprite Animation Convert")]
    static void Init() {
        EditorWindow window = GetWindowWithRect(typeof(ConvertAnimationToSpriteRenderer), new Rect(0, 0, 400, 400));
        window.Show();
    }

    void OnEnable() {

    }

    public AnimationClip[] clip;

    void OnGUI() {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);

        SerializedProperty textureProp = so.FindProperty(nameof(clip));
        EditorGUILayout.PropertyField(textureProp, true); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

        so.Update();
        so.ApplyModifiedProperties(); // Remember to apply modified properties

        if(GUILayout.Button("Convert")) {
            if (clip == null) return;
            foreach(var c in clip)
                CreateAnimation(c);
        }

    }

    private AnimationClip CreateAnimation(AnimationClip clip) {
        EditorCurveBinding curveBinding = new EditorCurveBinding();

        curveBinding.type = typeof(SpriteRenderer);
        curveBinding.path = "";
        curveBinding.propertyName = "m_Sprite";

        EditorCurveBinding[] bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);

        if (bindings[0].type == typeof(Image)) {
            var keyframes = AnimationUtility.GetObjectReferenceCurve(clip, bindings[0]);
            ObjectReferenceKeyframe[] new_keyframes = new ObjectReferenceKeyframe[keyframes.Length];
            for(int i = 0; i < keyframes.Length; ++i) {
                new_keyframes[i] = new ObjectReferenceKeyframe();
                new_keyframes[i].time = keyframes[i].time;
                new_keyframes[i].value = keyframes[i].value;
            }


            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, new_keyframes);
            AnimationUtility.SetObjectReferenceCurve(clip, bindings[0], null);
        }

        return clip;
    }
}
