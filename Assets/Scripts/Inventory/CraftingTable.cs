using System;
using CustomEvents;
using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CraftingTable")]
public class CraftingTable : ScriptableObject {

    public PersistentSetCraftingRecipe Recipes;
    public Inventory Inventory;

    public List<CraftingRecipe> GetPossibleRecipes() {
        return GetPossibleRecipesWhereResultIs<Item>();
    }

    public List<CraftingRecipe> GetPossibleRecipesWhereResultIs<T>() where T : Item {

        List<CraftingRecipe> recipes = new List<CraftingRecipe>();

        foreach(var recipe in Recipes) {
            if(!(recipe.Result.Item is T))
                continue;

            bool canMake = true;

            foreach(var component in recipe.Components) {
                InventoryEntry ie = Inventory.FirstOrDefault(e => e.Item.ID == component.Item.ID);
                if(ie == default(InventoryEntry) || ie.Amount < component.Amount) {
                    canMake = false;
                    break;
                }
            }
            if(canMake)
                recipes.Add(recipe);
        }

        return recipes;
    }

    public Item Make(CraftingRecipe cr) {
        if (cr.Components.Any(comp => !Inventory.Contains(comp))) {
            Debug.LogError("Tried to craft something you could not make");
            return null;
        }

        foreach(var comp in cr.Components) {
            Inventory.Remove(comp);
        }

        Item it = cr.Result.Item;
        
        if (it.ShouldCreateNewInstanceWhenPlayerObtained()) {
            it = ScriptableObject.Instantiate(cr.Result.Item);
            it.CreatedFromOriginal = true;
            it.OnAfterObtained();

            /*
            if (it is Weapon) {
                ((Weapon) it).Level = CurrentQuest.Value.Level;
            }

            if (it is Armor) {
                ((Armor) it).Level = CurrentQuest.Value.Level;
            }

            if(it is Headgear) {
                ((Headgear) it).Level = CurrentQuest.Value.Level;
            }*/
        }

        Inventory.Add(it, cr.Result.Amount);
        return it;
    }
}
