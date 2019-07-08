using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour {

    public Inventory Inventory;
    public Transform ItemsParent;
    public GameObject ItemPrefab;
    public Player p;

    public InventoryItemDisplay ItemDisplay;
    public TextMeshProUGUI ItemText;

    private ObjectPrefabPool _pool = null;
    private ObjectPrefabPool pool {
        get {
            if(_pool == null)
                _pool = new ObjectPrefabPool(ItemPrefab, ItemsParent);
            return _pool;
        }
    }

    private void OnEnable() {
        Refresh();
        Debug.Log("OnENable");
    }

    public void Refresh() {
        pool.Reset();

        foreach(var item in Inventory) {
            var go = pool.GetObject();
            var id = go.GetComponent<InventoryItemDisplay>();
            id.SetData(item.Item, item.Amount, ItemClicked);
        }
    }

    public void ItemClicked(InventoryItemDisplay it) {
        ItemDisplay.SetData(it.Item, 1, null);
        string text = "";
        text += $"Name: {it.Item.GetName()}\n";
        text += $"Type: {it.Item.GetType().Name}\n";
        text += $"Value: {it.Item.GetValue()}\n";
        text += $"'{it.Item.Description}'";
        ItemText.SetText(text);
    }

}
