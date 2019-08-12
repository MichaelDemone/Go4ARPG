using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

[Serializable]
public struct ItemInstance : IItem {
    [NonSerialized] public Item Data;
    public int DataId;
    public int Amount;

    public ItemInstance(Item it, int amount) {
        Data = it;
        DataId = it.ID;
        Amount = amount;
    }

    public string GetName() {
        return Data.Name;
    }

    public string GetDescription() {
        return Data.Description;
    }

    public int GetValue() {
        return Data.Value;
    }

    public Item GetItem() {
        return Data;
    }
}

public interface IItem {
    string GetName();
    string GetDescription();
    int GetValue();
    Item GetItem();
}