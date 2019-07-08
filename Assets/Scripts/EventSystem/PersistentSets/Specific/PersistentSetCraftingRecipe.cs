
using System.Linq;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using UnityEditor;
using UnityEngine;

namespace CustomEvents {
	[UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Persistent Set/Specific/CraftingRecipe")]
		public class PersistentSetCraftingRecipe : PersistentSetGeneric<CraftingRecipe, UnityEventCraftingRecipe> {
#if UNITY_EDITOR
	    [ContextMenu("Add all Recipes")]
	    public void AddAllOfType() {
	        string[] paths = AssetDatabase.FindAssets("t:" + typeof(CraftingRecipe).Name);
	        for(int i = 0; i < paths.Length; i++) {
	            paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
	        }

	        paths.Select(AssetDatabase.LoadAssetAtPath<CraftingRecipe>).ForEach(Add);
	    }
#endif
    }
}
