using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnimationCreator : EditorWindow {
	public Sprite[] Sprites = new Sprite[0];
	public bool SaveInSameFolderAsSprites;
	public string SaveDest = "";
	public string Name = "";
	public int FramesPerSecond = 8;
	public bool Loop = true;

	private string SaveLocation { get { return Path.Combine(SaveDest, Name + ".anim"); } }

	[MenuItem("Window/Animation/Animation Importer")]
	static void Init() {
		EditorWindow window = GetWindowWithRect(typeof(AnimationCreator), new Rect(0, 0, 400, 400));
		window.Show();
	}

	void OnGUI() {

		ScriptableObject target = this;
		SerializedObject so = new SerializedObject(target);
		SerializedProperty spritesProperty = so.FindProperty("Sprites");
		EditorGUILayout.PropertyField(spritesProperty, true); // True means show children
		so.ApplyModifiedProperties(); // Remember to apply modified properties

		SaveInSameFolderAsSprites = GUILayout.Toggle(SaveInSameFolderAsSprites, "Save In Same Folder As Sprites?");

		if (SaveInSameFolderAsSprites) {
			if (Sprites.Length > 0) {
				SaveDest = AssetDatabase.GetAssetPath(Sprites[0]);
				SaveDest = Path.GetDirectoryName(SaveDest);
			}
		}
		else {
			EditorGUILayout.BeginHorizontal();
			SaveDest = EditorGUILayout.TextField("Save Location", SaveDest);
			if (GUILayout.Button("Set Location")) {
				SaveDest = EditorUtility.OpenFolderPanel("Select Save Location", "", "");
			}
			EditorGUILayout.EndHorizontal();
		}

		Name = EditorGUILayout.TextField("Animation Name", Name);
		EditorGUILayout.TextField("Save Location", SaveLocation);
		FramesPerSecond = EditorGUILayout.IntField("Frames Per Second", FramesPerSecond);
		Loop = EditorGUILayout.Toggle("Should Loop?", Loop);

		if (GUILayout.Button("Create Animation")) {
			if (Sprites.Length == 0) {
				throw new Exception("There are no sprites to create an animation from.");
			}

			AnimationClip clip = new AnimationClip();
			clip.frameRate = FramesPerSecond;
			if (Loop) {
				Debug.LogWarning("Go to the LOOP button and click it TO MAKE IT FUCKING LOOP.");
				clip.wrapMode = WrapMode.Loop; // BECAUSE THIS DOESN'T WORK.
			}

			EditorCurveBinding curveBinding = new EditorCurveBinding();
			
			curveBinding.type = typeof(Image);
			curveBinding.path = "";
			curveBinding.propertyName = "m_Sprite";
			
			ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[Sprites.Length];
			for (int i = 0; i < Sprites.Length; i++) {
				keyframes[i] = new ObjectReferenceKeyframe();
				keyframes[i].time = i / (float)FramesPerSecond;
				keyframes[i].value = Sprites[i];
			}
			
			AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);
			clip.wrapMode = WrapMode.Loop;


			AssetDatabase.CreateAsset(clip, SaveLocation);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		Repaint();
	}

}
