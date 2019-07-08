
using System.Linq;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using UnityEditor;
using UnityEngine;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Persistent Set/Specific/Item")]
		public class PersistentSetItem : PersistentSetGeneric<Item, UnityEventItem> {
#if UNITY_EDITOR
		[ContextMenu("Add all items")]
		public void AddAllOfType() {
			string[] paths = AssetDatabase.FindAssets("t:" + typeof(Item).Name);
			for (int i = 0; i < paths.Length; i++) {
				paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
			}

			paths.Select(AssetDatabase.LoadAssetAtPath<Item>).ForEach(Add);
		}
#endif
	}
}
