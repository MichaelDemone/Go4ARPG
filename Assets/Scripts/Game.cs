using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance;
    public Inventory Inventory = new Inventory();
    public PersistentSetItem AllItems;
    public CraftingTable CraftingTable;

    public Dictionary<int, Item> Items = new Dictionary<int, Item>();

    private void Awake() {
        Instance = this;
        foreach (var item in AllItems) {
            Items.Add(item.ID, item);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Attempt to load the game. 
        Inventory.Add(Items[13]);
        Inventory.Add(Items[13]);
        Inventory.Add(Items[13]);
        Inventory.Add(Items[13], 4); // 7 overall

        Inventory.Add(new WeaponInstance((Weapon)Items[71], 10, null));

        string s = JsonUtility.ToJson(Inventory);
        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
