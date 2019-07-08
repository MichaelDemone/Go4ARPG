using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;
using UnityEngine.Events;

public class ChangeItemBase<T, TRef, TVar, TEvent> : MonoBehaviour 
    where T : Item, ITrashable
    where TEvent : UnityEvent<T>, new()
    where TVar : Variable<T, TEvent>
    where TRef : Reference<T, TVar, TEvent> {

    public InventoryItemDisplay IID;

    public Inventory Inventory;

    public TRef Item;

    public ItemViewer Viewer;
    public BoolReference ShowTrash;


    // Use this for initialization
    public void Awake() {

        Item.Variable.BeforeChange += () => { if(Item.Value != null) Item.Value.DataChanged -= Refresh; };
        Item.Variable.OnChange.AddListener((it) => { Refresh(); });

        Refresh();
    }

    public void Refresh() {
        if (Item.Value != null) {
            Item.Value.DataChanged -= Refresh;
            Item.Value.DataChanged += Refresh;
        }
        IID.SetData(Item.Value, 1, Onclick);
    }

    protected virtual void Onclick(InventoryItemDisplay inventoryItemDisplay) {

        Viewer.ShowItemsFromInventory<T>(false, ShowTrash, it => {
            PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", it.IsTrash() ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    if(Item.Value != null)
                        Inventory.Add(Item.Value);
                    Item.Value = it;
                    Inventory.Remove(it);
                    Viewer.Close();
                },
                () => {
                    it.SetTrash(!it.IsTrash());
                    Viewer.Close();
                    Onclick(null);
                },
                () => { }
            });
        });
    }
}
