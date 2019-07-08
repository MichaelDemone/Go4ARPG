using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryEntry {
    public Item Item;
    public int Amount;

    public InventoryEntryWithID GetIdEntry() {

        string AdditionalInfo = "";
        if (Item is ISaveable) {
            ISaveable saveable = (ISaveable) Item;
            AdditionalInfo = saveable.GetSaveString();
        }

        return new InventoryEntryWithID() {
            Id = Item.ID,
            Amount = Amount,
            AdditionalInfo = AdditionalInfo
        };
    }

    [System.Serializable]
    public class InventoryEntryWithID {
        public int Id;
        public int Amount;
        public string AdditionalInfo;
    }
}
