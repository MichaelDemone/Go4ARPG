using System;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class DroppedItem : MonoBehaviour {

    public SpriteRenderer Renderer;
    public float PickUpRadius = 0.5f;

    [NonSerialized] public int Amount;
    [NonSerialized] public Item Item;

    public void SetItem(Item item, int amount) {
        Item = item;
        Amount = amount;
    }

    void Update() {
        if ((PlayerMovement.Instance.transform.position - transform.position).magnitude < PickUpRadius) {
            // Pick up
            Inventory.Instance.Add(Item, Amount);

            // Give feedback
            SmoothPopUpManager.ShowPopUp(transform.position, $"<color=green>+{Amount}</color> {Item.Name}", Color.white, true);
            Destroy(gameObject);
        }    
    }
}
