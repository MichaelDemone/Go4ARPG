using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance;
    public Inventory Inventory = new Inventory();
    public PersistentSetItem AllItems;
    public CraftingTable CraftingTable;

    public Dictionary<int, Item> Items = new Dictionary<int, Item>();

    public SaveData SaveData;

    private void Awake() {
        Instance = this;
        foreach (var item in AllItems) {
            Items.Add(item.ID, item);
        }

        // Attempt to load the game. 
        LoadGame();
    }

    void Start()
    {
    }

    public void LoadGame() {
        string saveFile = Path.Combine(Application.persistentDataPath, "Saves", "G4ARPG.save");

        if(File.Exists(saveFile)) {
            string contents = File.ReadAllText(saveFile);
            JsonUtility.FromJsonOverwrite(contents, SaveData);

            Inventory = SaveData.Inventory;
            Inventory.InitItems();
        }
    }

    public void SaveGame() {
        string saveFile = Path.Combine(Application.persistentDataPath, "Saves", "G4ARPG.save");
        string backUp = saveFile + "_backup";

        SaveData.Inventory = Inventory;

        if (File.Exists(saveFile)) {
            string contents = JsonUtility.ToJson(SaveData);
            File.WriteAllText(backUp, contents);
        }
        else {
            string contents = JsonUtility.ToJson(SaveData);
            File.WriteAllText(saveFile, contents);
        }
    }
}

[Serializable]
public class SaveData {
    public Inventory Inventory;
}