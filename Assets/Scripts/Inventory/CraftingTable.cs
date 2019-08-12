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
            if(!(recipe.Result.Item1 is T))
                continue;

            bool canMake = true;

            foreach(var component in recipe.Components) {
                
                if(!Game.Instance.Inventory.Contains(component.Item1, component.Item2)) {
                    canMake = false;
                    break;
                }
            }
            if(canMake)
                recipes.Add(recipe);
        }

        return recipes;
    }

    public List<IItem> Make(CraftingRecipe cr) {
        if (cr.Components.Any(comp => !Inventory.Contains(comp.Item1, comp.Item2))) {
            Debug.LogError("Tried to craft something you could not make");
            return null;
        }

        foreach(var comp in cr.Components) {
            Inventory.Remove(comp.Item1, comp.Item2);
        }

        Item it = cr.Result.Item1;
        int amount = cr.Result.Item2;
        List<IItem> items = new List<IItem>();
        /*
        for (int i = 0; i < amount; i++) {

            ItemInstance instance = new ItemInstance();

            if (it is Weapon) {

                ((Weapon) it).Level = CurrentQuest.Value.Level;
            }

            if (it is Armor) {
                ((Armor) it).Level = CurrentQuest.Value.Level;
            }

            if(it is Headgear) {
                ((Headgear) it).Level = CurrentQuest.Value.Level;
            }
        }*/

        Inventory.Add(it, cr.Result.Item2);
        return items;
    }
}
