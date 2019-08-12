using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Misc/CraftingRecipesMade")]
public class CraftingRecipesMade : ScriptableObject {
    public static HashSet<int> RecipesMade = new HashSet<int>();
}
