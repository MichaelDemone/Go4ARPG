
using G4AW2.Data.Crafting;

namespace CustomEvents {
    [System.Serializable]
    [UnityEngine.CreateAssetMenu(menuName = "SO Architecture/Events/Specific/CraftingRecipe")]
	public class GameEventCraftingRecipe : GameEventGeneric<CraftingRecipe, GameEventCraftingRecipe, UnityEventCraftingRecipe> {
	}
}
