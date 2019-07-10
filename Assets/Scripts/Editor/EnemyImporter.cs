using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using G4AW2.Data.Combat;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.Tools {
	public class EnemyImporter : EditorWindow {

		[Serializable]
		public class Anim {
			public string name;
			public int frameCount;
			public int milliPerFrame;

			public Anim(string name, int frameCount, int milliPerFrame ) {
				this.name = name;
				this.frameCount = frameCount;
				this.milliPerFrame = milliPerFrame;
			}
		}

		public Texture2D ImagePng;
		public int SpriteSizes;

		public string EnemyName;

		public bool skipImportStep = false;


		public Anim[] animations = {
            new Anim("Idle", 0, 125),
            new Anim("Flinch", 0, 125),
			new Anim("BeforeAttack", 0, 125),
			new Anim("AttackExecute", 0, 125),
			new Anim("AfterAttack", 0, 125),
            new Anim("Death", 0, 125),		
			new Anim("Walking", 0, 125),
        };

		public ReorderableList AnimationOrder;

		private string SelectedObject;
		[MenuItem("Tools/Enemy Importer")]
		static void Init() {
			EditorWindow window = GetWindowWithRect(typeof(EnemyImporter), new Rect(0, 0, 400, 400));
			window.Show();
		}

		void OnEnable() {
			var ob = new SerializedObject(this);
			AnimationOrder = new ReorderableList(ob, ob.FindProperty("animations"), true, false, false, false);
			AnimationOrder.drawElementCallback = (rect, index, active, focused) => {
				var elem = AnimationOrder.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;
				EditorGUI.LabelField(new Rect(rect.x, rect.y, 120, EditorGUIUtility.singleLineHeight), elem.FindPropertyRelative("name").stringValue);
				elem.FindPropertyRelative("frameCount").intValue = EditorGUI.IntField(new Rect(rect.x + 124, rect.y, 30, EditorGUIUtility.singleLineHeight), elem.FindPropertyRelative("frameCount").intValue);
				elem.FindPropertyRelative("milliPerFrame").intValue = EditorGUI.IntField(new Rect(rect.x + 158, rect.y, 30, EditorGUIUtility.singleLineHeight), elem.FindPropertyRelative("milliPerFrame").intValue);
			};
			AnimationOrder.drawHeaderCallback = rect => {
				EditorGUI.LabelField(rect, "Animation -> Frame Counts -> ms per frame");
			};
		}

		void OnGUI() {

			EnemyName = EditorGUILayout.TextField("Enemy Name", EnemyName);
			SpriteSizes = EditorGUILayout.IntField("Sprite Size (32/64)", SpriteSizes);

			ScriptableObject target = this;
			SerializedObject so = new SerializedObject(target);

			SerializedProperty textureProp = so.FindProperty("ImagePng");
			EditorGUILayout.PropertyField(textureProp, true); // True means show children
			so.ApplyModifiedProperties(); // Remember to apply modified properties

			so.Update();
			AnimationOrder.DoLayoutList();
			so.ApplyModifiedProperties(); // Remember to apply modified properties

			skipImportStep = EditorGUILayout.Toggle("Skip Import Step?", skipImportStep);


			if (GUILayout.Button("Create Enemy")) {
				string path = AssetDatabase.GetAssetPath(ImagePng);

				if (!skipImportStep) {
					TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(path);
					importer.wrapMode = TextureWrapMode.Clamp;
					importer.textureType = TextureImporterType.Sprite;
					importer.spriteImportMode = SpriteImportMode.Multiple;
					importer.filterMode = FilterMode.Point;
					importer.textureCompression = TextureImporterCompression.Uncompressed;

					EditorUtility.SetDirty(importer);
					importer.SaveAndReimport();

					Rect[] rects = InternalSpriteUtility.GenerateGridSpriteRectangles(ImagePng, Vector2.zero, new Vector2(SpriteSizes, SpriteSizes),
						Vector2.zero);

					string filenameNoExtension = Path.GetFileNameWithoutExtension(path);
					List<SpriteMetaData> metas = new List<SpriteMetaData>();
					int rectNum = 0;

					foreach (Rect rect in rects) {
						SpriteMetaData meta = new SpriteMetaData();
						meta.rect = rect;
						meta.name = filenameNoExtension + "_" + rectNum++;
						metas.Add(meta);
					}

					importer.spritesheet = metas.ToArray();

					EditorUtility.SetDirty(importer);
					importer.SaveAndReimport();
				}

				Sprite[] spriteSheet = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();
                spriteSheet = spriteSheet.OrderBy(l => l.name, new NaturalComparer()).ToArray();

				List<Anim> anims = new List<Anim>();
				int totalFrameCount = 0; 

				for (int i = 0; i < AnimationOrder.count; i++) {
					string name = AnimationOrder.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue;
					int frameCount = AnimationOrder.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("frameCount").intValue;
					int millisPerFrame = AnimationOrder.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("milliPerFrame").intValue;

					totalFrameCount += frameCount;
					anims.Add(new Anim(name, frameCount, millisPerFrame));
				}

				if (totalFrameCount != spriteSheet.Length) {
					throw new Exception("Total Frames do not equal number of sprites!");
				}

				bool loadedData = true;
				string EnemyDataPath = Path.Combine(Path.GetDirectoryName(path), EnemyName + ".asset");

				EnemyData enemyScriptableObject = AssetDatabase.LoadAssetAtPath<EnemyData>(EnemyDataPath);

				if (enemyScriptableObject == null) {
					enemyScriptableObject = CreateInstance<EnemyData>();
					loadedData = false;
				}

			    enemyScriptableObject.DeadSprite = spriteSheet[spriteSheet.Length - 1];
			    enemyScriptableObject.Portrait = spriteSheet[0];

				totalFrameCount = 0;
				for (int i = 0; i < anims.Count; i++) {
					AnimationClip ac = CreateAnimation(anims[i], spriteSheet.Skip(totalFrameCount).Take(anims[i].frameCount).ToArray(), EnemyName, path);

					switch (anims[i].name) {
                        case "Idle":
							enemyScriptableObject.Idle = ac;
							break;
                        case "Flinch":
							enemyScriptableObject.Flinch = ac;
							break;
						case "BeforeAttack":
							enemyScriptableObject.BeforeAttack = ac;
							break;
                        case "AttackExecute":
                            enemyScriptableObject.AttackExecute = ac;
                            break;
                        case "AfterAttack":
                            if(anims[i].frameCount != 0)
                                enemyScriptableObject.AfterAttack = ac;
                            else
                                enemyScriptableObject.AfterAttack = CreateAnimation(new Anim("AfterAttack", 1, 1000), new[] { spriteSheet[totalFrameCount + anims[i].frameCount - 1] }, EnemyName, path);
                            break;
						case "Death":
							enemyScriptableObject.Death = ac;
                            // If this is the death animation then create a dead animation that is just the last frame of the death animation
						    Sprite deadSprite = spriteSheet[totalFrameCount + anims[i].frameCount - 1];
                            enemyScriptableObject.Dead = CreateAnimation(new Anim("Dead", 1, 1000), new[] { deadSprite }, EnemyName, path);
						    enemyScriptableObject.DeadSprite = deadSprite;
                            break;
                        case "Walking":
                            enemyScriptableObject.Walking = ac;
                            break;
					}

					totalFrameCount += anims[i].frameCount;
				}

				if(!loadedData) AssetDatabase.CreateAsset(enemyScriptableObject, EnemyDataPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		private AnimationClip CreateAnimation(Anim a, Sprite[] Sprites, string enemyName, string pngPath) {
			AnimationClip clip = new AnimationClip();
			float FramesPerSecond = 1000f / a.milliPerFrame;
			clip.frameRate = FramesPerSecond;

			EditorCurveBinding curveBinding = new EditorCurveBinding();

			curveBinding.type = typeof(Image);
			curveBinding.path = "";
			curveBinding.propertyName = "m_Sprite";

			ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[Sprites.Length];
			for (int i = 0; i < Sprites.Length; i++) {
				keyframes[i] = new ObjectReferenceKeyframe();
				keyframes[i].time = i / FramesPerSecond;
				keyframes[i].value = Sprites[i];
			}

			AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);
			clip.wrapMode = WrapMode.Loop;

			string saveLocation = Path.Combine(Path.GetDirectoryName(pngPath), enemyName + a.name + ".anim");

			AssetDatabase.CreateAsset(clip, saveLocation);

			return clip;
		}

        private class NaturalComparer : IComparer<string> {
            private readonly CultureInfo _CultureInfo = CultureInfo.CurrentCulture;

            public int Compare( string x, string y ) {
                // simple cases
                if (x == y) // also handles null
                    return 0;
                if (x == null)
                    return -1;
                if (y == null)
                    return +1;

                int ix = 0;
                int iy = 0;
                while (ix < x.Length && iy < y.Length) {
                    if (Char.IsDigit(x[ix]) && Char.IsDigit(y[iy])) {
                        // We found numbers, so grab both numbers
                        int ix1 = ix++;
                        int iy1 = iy++;
                        while (ix < x.Length && Char.IsDigit(x[ix]))
                            ix++;
                        while (iy < y.Length && Char.IsDigit(y[iy]))
                            iy++;
                        string numberFromX = x.Substring(ix1, ix - ix1);
                        string numberFromY = y.Substring(iy1, iy - iy1);

                        // Pad them with 0's to have the same length
                        int maxLength = Math.Max(
                            numberFromX.Length,
                            numberFromY.Length);
                        numberFromX = numberFromX.PadLeft(maxLength, '0');
                        numberFromY = numberFromY.PadLeft(maxLength, '0');

                        int comparison = _CultureInfo
                            .CompareInfo.Compare(numberFromX, numberFromY);
                        if (comparison != 0)
                            return comparison;
                    } else {
                        int comparison = _CultureInfo
                            .CompareInfo.Compare(x, ix, 1, y, iy, 1);
                        if (comparison != 0)
                            return comparison;
                        ix++;
                        iy++;
                    }
                }

                // we should not be here with no parts left, they're equal
                Debug.Assert(ix < x.Length || iy < y.Length);

                // we still got parts of x left, y comes first
                if (ix < x.Length)
                    return +1;

                // we still got parts of y left, x comes first
                return -1;
            }
        }
    }
}

