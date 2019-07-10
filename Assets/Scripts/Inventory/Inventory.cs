using System;
using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Crafting;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Inventory")]
public class Inventory : ScriptableObject, IEnumerable<InventoryEntry>, ISaveable {

    private List<InventoryEntry> InventoryEntries = new List<InventoryEntry>();
    public PersistentSetItem AllItems;
    public CraftingTable CraftingTable;
    public Sprite QuestionMark;

    public int GetAmountOf(Item it) {
        return InventoryEntries.Sum(i => i.Item.ID == it.ID ? i.Amount : 0);
    }

    public void AddItems(IEnumerable<Item> items) {
        foreach(Item item in items) Add(item);
    }

    public void Add(Item it) {
        Add(it, 1);
    }

    private List<CraftingRecipe> currentRecipes = null;

    public void Add(Item it, int amount) {

        InventoryEntry entry = InventoryEntries.FirstOrDefault(e => e.Item == it);
        GameEventHandler.Singleton.OnLootObtained(it, amount);

        if(entry == default(InventoryEntry)) {
            InventoryEntries.Add(new InventoryEntry() { Item = it, Amount = amount});
        } else {
            entry.Amount += amount;
        }


        // Check if a new recipe is makeable
        if (currentRecipes == null) {
            currentRecipes = CraftingTable.GetPossibleRecipes();
        } else {
            List<CraftingRecipe> recipes = CraftingTable.GetPossibleRecipes();
            foreach(var recipe in recipes) {
                if(!currentRecipes.Contains(recipe) && !CraftingRecipesMade.RecipesMade.Contains(recipe.ID)) {
                    string postText = "";
                    foreach(var component in recipe.Components) {
                        postText +=
                            $"{component.Amount} {component.Item.GetName()}{(component.Amount > 1 ? "s" : "")}\n";
                    }
                    QuickPopUp.Show(QuestionMark, $"<size=150%>New Craftable Recipe!</size>\nA new recipe is now craftable!\nRequires:{postText}");
                }
            }
            currentRecipes = recipes;
        }
        
    }

    public void Add(InventoryEntry it) {
        Add(it.Item, it.Amount);
    }

    public bool Remove(InventoryEntry it) {
        return Remove(it.Item, it.Amount);
    }

    public bool Remove(Item it) {
        return Remove(it, 1);
    }

    public bool Remove(Item it, int amount) {
        InventoryEntry entry = InventoryEntries.FirstOrDefault(e => e.Item == it);
        if(entry == default(InventoryEntry)) {
            return false;
        } else {
            if(entry.Amount - amount < 0) {
                return false;
            }

            entry.Amount -= amount;
            if(entry.Amount == 0) {
                InventoryEntries.Remove(entry);
            }
        }

        return true;
    }

    public bool Contains(Item it, int amount) {
        InventoryEntry entry = InventoryEntries.FirstOrDefault(e => e.Item.ID == it.ID);
        if(entry == default(InventoryEntry) || entry.Amount - amount < 0) {
            return false;
        }

        return true;
    }
    public bool Contains(Item it) => Contains(it, 1);
    public bool Contains(InventoryEntry it) => Contains(it.Item, it.Amount);

    public string GetSaveString() {
        DummySave ds = new DummySave();
        InventoryEntries.ForEach(e => ds.Entries.Add(e.GetIdEntry()));
        return JsonUtility.ToJson(ds);
    }

    public void SetData(string saveString, params object[] otherData) {
        DummySave entries = JsonUtility.FromJson<DummySave>(saveString);
        InventoryEntries.Clear();
        foreach(InventoryEntry.InventoryEntryWithID entry in entries.Entries) {

            Item it = AllItems.First(d => d.ID == entry.Id);

            if (it is ISaveable) {
                it = Instantiate(it);
                it.CreatedFromOriginal = true;
                ((ISaveable) it).SetData(entry.AdditionalInfo, it);
            }

            InventoryEntry ie = new InventoryEntry() {
                Item = it,
                Amount = entry.Amount,
            };

            InventoryEntries.Add(ie);
        }
    }

    private class DummySave {
        public List<InventoryEntry.InventoryEntryWithID> Entries = new List<InventoryEntry.InventoryEntryWithID>();
    }

    public IEnumerator<InventoryEntry> GetEnumerator() {
        return InventoryEntries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return InventoryEntries.GetEnumerator();
    }

    public void Clear() {
        InventoryEntries.Clear();
    }


    public Item ItemToAdd;
    [ContextMenu("Add Item")]
    ///THIS IS FOR TESTING ONLY.
    public void Add() {
        if(ItemToAdd != null) {
            Add(ItemToAdd);
        }
    }

    public string GetName() {
        throw new NotImplementedException();
    }

    public object GetSaveObject() {
        throw new NotImplementedException();
    }

    public Type GetSaveType() {
        throw new NotImplementedException();
    }

    public void SetData(object saveObject) {
        throw new NotImplementedException();
    }
}
