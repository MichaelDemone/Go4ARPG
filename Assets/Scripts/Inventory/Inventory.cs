using G4AW2.Data.Crafting;
using G4AW2.Data.DropSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory {

    public List<ItemInstance> Items = new List<ItemInstance>();
    public List<WeaponInstance> Weapons = new List<WeaponInstance>();
    public List<ArmorInstance> Armors = new List<ArmorInstance>();
    public List<HeadgearInstance> Headgears = new List<HeadgearInstance>();

    public int GetAmountOf(Item it) {
        return Items.Sum(i => i.DataId == it.ID ? i.Amount : 0);
    }

    public void Add(Item it) {
        Add(it, 1);
    }

    private List<CraftingRecipe> currentRecipes = null;

    public void Add(IItem it) {
        if(it is ItemInstance) {
            Add(it.GetItem(), ((ItemInstance) it).Amount);
        } else if(it is WeaponInstance) {
            Weapons.Add((WeaponInstance) it);
        } else if(it is ArmorInstance) {
            Armors.Add((ArmorInstance) it);
        } else if(it is HeadgearInstance) {
            Headgears.Add((HeadgearInstance) it);
        }
    }

    public void Add(Item it, int amount) {

        int i = Items.FindIndex(item => item.DataId == it.ID);
        if(i == -1) {
            ItemInstance ii = new ItemInstance(it, amount);
            Items.Add(ii);
        } else {
            ItemInstance ii = Items[i];
            ii.Amount += amount;
            Items[i] = ii;
        }
    }

    private void CheckForNewRecipes() {
        if(currentRecipes == null) {
            currentRecipes = Game.Instance.CraftingTable.GetPossibleRecipes();
        } else {
            List<CraftingRecipe> recipes = Game.Instance.CraftingTable.GetPossibleRecipes();
            foreach(CraftingRecipe recipe in recipes) {
                if(!currentRecipes.Contains(recipe) && !CraftingRecipesMade.RecipesMade.Contains(recipe.ID)) {
                    string postText = "";
                    foreach((Item, int) component in recipe.Components) {
                        postText +=
                            $"{component.Item2} {component.Item1.Name}{(component.Item2 > 1 ? "s" : "")}\n";
                    }
                    //QuickPopUp.Show(QuestionMark, $"<size=150%>New Craftable Recipe!</size>\nA new recipe is now craftable!\nRequires:{postText}");
                }
            }
            currentRecipes = recipes;
        }
    }

    public bool Remove(Item it) {
        return Remove(it, 1);
    }

    public bool Remove(Item it, int amount) {

        int i = -1;
        if(it is Weapon) {
            i = Weapons.FindIndex(e => e.DataID == it.ID);
            if(i != -1) {
                Weapons.RemoveAt(i);
            }

            return i != -1 && amount == 1;
        }

        if(it is Armor) {
            return false;
        }

        if(it is Headgear) {
            return false;
        }

        i = Items.FindIndex(e => e.DataId == it.ID);
        if(i != -1 && Items[i].Amount >= amount) {
            ItemInstance t = Items[i];
            t.Amount -= amount;
            if(t.Amount == 0) {
                Items.RemoveAt(i);
            } else {
                Items[i] = t;
            }

            return true;
        }
        return false;
    }

    public bool Contains(Item it, int amount) {

        int i = -1;
        if(it is Weapon) {
            i = Weapons.FindIndex(e => e.DataID == it.ID);
            return i != -1 && amount == 1;
        }

        if(it is Armor) {
            return false;
        }

        if(it is Headgear) {
            return false;
        }

        i = Items.FindIndex(e => e.DataId == it.ID);
        return i != -1 && Items[i].Amount >= amount;
    }
    public bool Contains(Item it) {
        return Contains(it, 1);
    }

    public Item ItemToAdd;
    [ContextMenu("Add Item")]
    ///THIS IS FOR TESTING ONLY.
    public void Add() {
        if(ItemToAdd != null) {
            Add(ItemToAdd);
        }
    }
}
