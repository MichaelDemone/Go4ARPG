using System;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class DroppedItem : MonoBehaviour {

    public SpriteRenderer Renderer;
    public float PickUpRadius = 0.5f;

    [NonSerialized] public IItem Item;

    public void SetItem(IItem item) {
        Item = item;
        Renderer.sprite = item.GetItem().Image;
    }

    void Update() {
        if ((PlayerMovement.Instance.transform.position - transform.position).magnitude < PickUpRadius) {
            // Pick up
            Game.Instance.Inventory.Add(Item);

            // Give feedback
            SmoothPopUpManager.ShowPopUp(transform.position, $"<color=green>+{1}</color> {Item.GetName()}", Color.white, true);
            Destroy(gameObject);
        }    
    }
}
