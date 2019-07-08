using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;
using UnityEngine.Events;

public class ItemDropBubbleManager : MonoBehaviour {

    public Inventory Inventory;
    public WeaponVariable PlayerWeapon;

	public GameObject ItemDropperPrefab;

	public float SpawnDelay;

    private ObjectPrefabPool Pool;

    void Awake() {
        Pool = new ObjectPrefabPool(ItemDropperPrefab, transform, 5);
    }

    public void AddItems(IEnumerable<Item> items, Action onClick) {
        List<Item> itemsList = items.ToList();
        StartCoroutine(ShootItems(itemsList, onClick));
    }

	public void AddItems( IEnumerable<Item> items ) {
		List<Item> itemsList = items.ToList();
		StartCoroutine(ShootItems(itemsList, null));
	}

	private IEnumerator ShootItems( IEnumerable<Item> items, Action onClick) {
		foreach (Item it in items) {
			yield return new WaitForSeconds(SpawnDelay);
		    GameObject itemBubble = Pool.GetObject();
		    itemBubble.transform.localPosition = Vector3.zero;
            itemBubble.transform.rotation = Quaternion.identity;
            itemBubble.GetComponent<ItemDropBubble>().SetData(it, (i) => OnClick(i, onClick), (i) => OnAutoLoot(i, onClick));
			itemBubble.GetComponent<ItemDropBubble>().Shoot();
		}
	}

    public WeaponUI WeaponUI;

	private void OnClick(ItemDropBubble it, Action onClick) {

        SmoothPopUpManager.ShowPopUp(it.transform.localPosition, $"<color=green>+1</color> {it.Item.GetName()}", ConfigObject.GetColorFromRarity(it.Item.Rarity));

	    if (it.Item is Weapon) {

	        WeaponUI.SetWeaponWithDefaults((Weapon)it.Item, () => {
	            Pool.Return(it.gameObject);
	            onClick?.Invoke();
            });
	        return;
	    }

        Pool.Return(it.gameObject);
	    onClick?.Invoke();
    }

    private void OnAutoLoot(ItemDropBubble it, Action onLooted) {
        Pool.Return(it.gameObject);
        onLooted?.Invoke();
    }

    public Transform AutoLootNotificationPosition;

    public void Clear() {
        if (Pool.InUse.Count == 0) return;

        float delay = 0;

        foreach (var obj in Pool.InUse.ToArray()) {
            var bubble = obj.GetComponent<ItemDropBubble>();
            bubble.OnAutoLoot();
            Timer.StartTimer(delay, () => {
                SmoothPopUpManager.ShowPopUp(AutoLootNotificationPosition.position,
                    "<color=green>+1</color> " + bubble.Item.GetName(), Color.white, true);
            });
            delay += 1f;
        }

        Pool.Reset();
    }

    [Header("Debug")] public ItemDropper Dropper;
#if UNITY_EDITOR
	[ContextMenu("Drop Items")]
	public void DropItems() {
		AddItems(Dropper.GetItems(true));
	}

#endif
}
