using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Misc/CraftingRecipesMade")]
public class CraftingRecipesMade : ScriptableObject, ISaveable {

    public static HashSet<int> RecipesMade = new HashSet<int>();

    private class SaveObject {
        public List<int> made = new List<int>();
    }

    public string GetSaveString() {
        return JsonUtility.ToJson(new SaveObject() {made = RecipesMade.ToList()});
    }

    public void SetData(string saveString, params object[] otherData) {
        SaveObject so = JsonUtility.FromJson<SaveObject>(saveString);

        foreach (int id in so.made) {
            RecipesMade.Add(id);
        }
    }
}
