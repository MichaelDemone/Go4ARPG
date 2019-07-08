using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace G4AW2.Events.Editor {
	public class NewType : EditorWindow {

		#region Scripts
		private static readonly string UnityEventScript = @"
{0}
namespace {1} {{
	[System.Serializable] public class UnityEvent{3} : GenericUnityEvent<{2}> {{ }}
}}
";
		private static readonly string GameEventScript = @"
{0}
namespace {1} {{
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = ""SO Architecture/Events/{4}/{3}"")]
	public class GameEvent{3} : GameEventGeneric<{2}, GameEvent{3}, UnityEvent{3}> {{
	}}
}}
";
		private static readonly string GameEventListenerScript = @"
{0}
namespace {1} {{
    public class GameEventListener{3} : GameEventListenerGeneric<{2}, GameEvent{3}, UnityEvent{3}> {{
    }}
}}
";
		private static readonly string VariableScript = @"
{0}
namespace {1} {{
    [UnityEngine.CreateAssetMenu(menuName = ""SO Architecture/Variable/{4}/{3}"")]
	public class {3}Variable : Variable<{2}, UnityEvent{3}> {{
	}}
}}
";
		private static readonly string ReferenceScript = @"
{0}
namespace {1} {{
	[System.Serializable]
    public class {3}Reference : Reference<{2}, {3}Variable, UnityEvent{3}> {{ }}
}}
";

		private static readonly string ReferenceDrawerScript = @"
{0}
namespace {1}.Editor {{
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<{2}, {3}Variable, UnityEvent{3}>))]
		public class {3}PropertyDrawer : AbstractReferenceDrawer {{ }}
}}
";

		private static readonly string RuntimeSetScript = @"
{0}
namespace {1} {{
	[UnityEngine.CreateAssetMenu(menuName = ""SO Architecture/Runtime Set/{4}/{3}"")]
		public class RuntimeSet{3} : RuntimeSetGeneric<{2}, UnityEvent{3}> {{
		}}
	}}
";

		private static readonly string PersistentSetScript = @"
{0}
namespace {1} {{
	[UnityEngine.CreateAssetMenu(menuName = ""SO Architecture/Persistent Set/{4}/{3}"")]
		public class PersistentSet{3} : PersistentSetGeneric<{2}, UnityEvent{3}> {{
		}}
	}}
";
		#endregion


		private static readonly string namespaceString = "CustomEvents";
		private bool generalFoldout;
		private bool overrideOriginalSaveLocation;
		private string mainPath;
		private bool doTypeCheck;

		public string[] Usings;
		private string typeNameString;
		private bool gameSpecific;

		private bool includeGameEvents = true;
		private bool includeVariablesAndReference = true;
		private bool forceOverwriteIfOthersExist = false;
		private bool includePersistentSet;
		private bool includeRuntimeSet;

		private string UsingsString {
			get { string usingsString = "";
			    Usings?.ForEach(s => usingsString = usingsString + "using " + s + ";\n");
			    return usingsString;
			}
		}

		private string FormattedType {
			get { return FormatType(typeNameString); }
		}

		private string FolderType {
			get { return gameSpecific ? "Specific" : "General"; }
		}

		[MenuItem("Variables/New Type Window")]
		static void Init() {
			EditorWindow window = GetWindowWithRect(typeof(NewType), new Rect(0, 0, 400, 400));
			window.Show();
		}

		private string GetGeneralPath() {
			string pathToProject = Path.GetDirectoryName(Application.dataPath);
			string pathToThisScript = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
			string pathToEditorFolder = Path.GetDirectoryName(pathToThisScript);
			string pathToGeneralFolder = Path.GetDirectoryName(pathToEditorFolder);
			return Path.Combine(pathToProject, pathToGeneralFolder);
		}

		void Awake() {
			overrideOriginalSaveLocation = EditorPrefs.GetBool("overrideOriginalSaveLocation", false);
			mainPath = EditorPrefs.GetString("mainpath", GetGeneralPath());
			doTypeCheck = EditorPrefs.GetBool("doTypeCheck", false);

			generalFoldout = mainPath.Equals("") || namespaceString.Equals("");
		}

		void OnDestroy() {
			EditorPrefs.SetString("mainPath", mainPath);
			EditorPrefs.SetBool("doTypeCheck", doTypeCheck);
		}

		private string Create() {
			Debug.Log("Create!");
			string errorString = "";

			errorString += CheckString(namespaceString, "namespace");
			errorString += CheckString(typeNameString, "type name");
			errorString += CheckString(mainPath, "event system path");

			if (!errorString.Equals("")) {
				generalFoldout = true;
			}

			if (doTypeCheck && Type.GetType(typeNameString, false) == null) {
				errorString += "Please enter a valid type\n";
			}

			if (!errorString.Equals(""))
				return errorString;

			errorString += WriteScript(UnityEventScript, "UnityEvent{0}.cs", Path.Combine(mainPath, "UnityEvents"));

			if (includeGameEvents) {

				// Write to file
				errorString += WriteScript(GameEventScript, "GameEvent{0}.cs", Path.Combine(mainPath, "GameEvents"));

				// write to file
				errorString += WriteScript(GameEventListenerScript, "GameEventListener{0}.cs", Path.Combine(mainPath, "GameEventListeners"));

			}

			if (includeVariablesAndReference) {
				errorString += WriteScript(VariableScript, "{0}Variable.cs", Path.Combine(mainPath, "Variables"));

				errorString += WriteScript(ReferenceScript, "{0}Reference.cs", Path.Combine(mainPath, "References"));

				errorString += WriteScript(ReferenceDrawerScript, "{0}PropertyDrawer.cs", Path.Combine(Path.Combine(mainPath, "Editor"), "ReferenceDrawers"));
			}

			if (includePersistentSet) {
				errorString += WriteScript(PersistentSetScript, "PersistentSet{0}.cs", Path.Combine(mainPath, "PersistentSets"));
			}

			if (includeRuntimeSet) {
				errorString += WriteScript(RuntimeSetScript, "RuntimeSet{0}.cs", Path.Combine(mainPath, "RuntimeSets"));
			}

			Debug.Log("Done writing scripts. Compiling now.");
			AssetDatabase.Refresh();
			return errorString;
		}

		private bool showCode = false;
		private Vector2 pos;

		void OnGUI() {
			EditorGUIUtility.labelWidth = 150;
			pos = EditorGUILayout.BeginScrollView(pos);
			generalFoldout = EditorGUILayout.Foldout(generalFoldout, "General");
			if (generalFoldout) {
				overrideOriginalSaveLocation = EditorGUILayout.Toggle("Override save locations?", overrideOriginalSaveLocation);
				if (!overrideOriginalSaveLocation)
				{
					mainPath = GetGeneralPath();
				}
				mainPath = PathField("Event System", mainPath);
				doTypeCheck = EditorGUILayout.Toggle("Do type check?", doTypeCheck);
			}

			EditorGUILayout.LabelField("");

			// Usings list
			ScriptableObject target = this;
			SerializedObject so = new SerializedObject(target);
			SerializedProperty stringsProperty = so.FindProperty("Usings");
			EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
			so.ApplyModifiedProperties(); // Remember to apply modified properties

			typeNameString = EditorGUILayout.TextField("Type Name: ", typeNameString);
			gameSpecific = EditorGUILayout.Toggle("Game Specific?", gameSpecific);
			forceOverwriteIfOthersExist = EditorGUILayout.Toggle("Force overwrite if exists?", forceOverwriteIfOthersExist);
			includeGameEvents = GUILayout.Toggle(includeGameEvents, "Include Game Events and Listeners?");
			includeVariablesAndReference = GUILayout.Toggle(includeVariablesAndReference, "Include Variables and References?");
			includePersistentSet = GUILayout.Toggle(includePersistentSet, "Include Persistent Set?");
			includeRuntimeSet = GUILayout.Toggle(includeRuntimeSet, "Include Runtime Set?");


			if (GUILayout.Button("Create Type")) {
				string error = Create();
				if (!error.Equals("")) {
					Debug.LogError(error);
				}
			}

			showCode = EditorGUILayout.Foldout(showCode, "Code");
			if (showCode) {
				EditorGUILayout.LabelField("Unity Event");
				EditorGUILayout.TextArea(FormatScript(UnityEventScript));
				EditorGUILayout.LabelField("");

				if (includeGameEvents) {
					EditorGUILayout.LabelField("Game Event Code");
					EditorGUILayout.TextArea(FormatScript(GameEventScript));
					EditorGUILayout.LabelField("Game Event Listener Code");
					EditorGUILayout.TextArea(FormatScript(GameEventScript));
					EditorGUILayout.LabelField("");
				}

				if (includeVariablesAndReference) {
					EditorGUILayout.LabelField("Variable Code");
					EditorGUILayout.TextArea(FormatScript(VariableScript));
					EditorGUILayout.LabelField("Reference Code");
					EditorGUILayout.TextArea(FormatScript(ReferenceScript));
					EditorGUILayout.LabelField("Reference Drawers Code");
					EditorGUILayout.TextArea(FormatScript(ReferenceDrawerScript));
					EditorGUILayout.LabelField("");
				}

				if (includePersistentSet) {
					EditorGUILayout.LabelField("Runtime Set Code");
					EditorGUILayout.TextArea(FormatScript(RuntimeSetScript));
					EditorGUILayout.LabelField("");
				}

				if (includeRuntimeSet) {
					EditorGUILayout.LabelField("Persistent Set Code");
					EditorGUILayout.TextArea(FormatScript(PersistentSetScript));
				}
			}

			EditorGUILayout.EndScrollView();
			Repaint();
		}


		#region HelperMethods

		private string PathField( string name, string val ) {
			EditorGUILayout.BeginHorizontal();
			val = EditorGUILayout.TextField(name + " Folder Path: ", val);
			if (GUILayout.Button("Set")) {
				val = EditorUtility.OpenFolderPanel(name + " Folder Path", "", "");
			}
			EditorGUILayout.EndHorizontal();
			return val;
		}

		private static string FormatType( string type ) {
			string formattedType = "";
			if (type.Length > 1)
				formattedType = char.ToUpper(type[0]) + type.Substring(1);
			else
				formattedType = type.ToUpper();

			formattedType = formattedType.Replace("[]", "Array");
			formattedType = new string(formattedType.Where(char.IsLetterOrDigit).ToArray());
			return formattedType;
		}

		private string CheckString( string toBeChecked, string name ) {
			string error = "";
			if (toBeChecked.Equals("")) {
				error = "Please enter a " + name + "\n";
			}
			return error;
		}

		private string WriteScript( string script, string fileName, string path ) {

			script = FormatScript(script);
			fileName = string.Format(fileName, FormattedType);

			string fullPath = Path.Combine(path, FolderType);
			fullPath = Path.Combine(fullPath, fileName);

			if (File.Exists(fullPath)) {
				if (forceOverwriteIfOthersExist) {
					File.Delete(fullPath);
				} else {
					return "File for " + fileName + " already exsits\n";
				}
			}

            string directory = Path.GetDirectoryName(fullPath);
            
            if(!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            File.AppendAllText(fullPath, script);

			return "";
		}

		private string FormatScript(string script) {
			return string.Format(script, UsingsString, namespaceString, typeNameString, FormattedType, FolderType);
		}

		#endregion
	}


}

