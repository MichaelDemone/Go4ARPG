using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

public static class ItemDropManager {

    public static List<ItemAndRarity> Items = new List<ItemAndRarity>();

    public static void AddToList(ItemAndRarity Item) {
        Items.Add(Item);
    }

    public static void RemoveFromList(ItemAndRarity Item) {
        Items.Remove(Item);
    }

    public static List<Item> GetRandomDrop() {
        List<Item> droppedItems = new List<Item>();
        foreach(var item in Items) {
            float value = Random.value;
            if(item.dropChance > value) {
                Item itemThatGoesToInventory = item.item;
                if (item.item.ShouldCreateNewInstanceWhenPlayerObtained()) {
                    itemThatGoesToInventory = Object.Instantiate(item.item);
                    itemThatGoesToInventory.OnAfterObtained();
                }
                droppedItems.Add(itemThatGoesToInventory);
            }
        }
        return droppedItems;
    }
}
