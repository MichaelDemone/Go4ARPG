
using G4AW2.Data.Crafting;

namespace CustomEvents.Editor {
		[UnityEditor.CustomPropertyDrawer(typeof(Reference<CraftingRecipe, CraftingRecipeVariable, UnityEventCraftingRecipe>))]
		public class CraftingRecipePropertyDrawer : AbstractReferenceDrawer { }
}
