using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ItemDropBubble : MonoBehaviour, IPointerClickHandler {

	public Item Item { get { return item; }}
	private Item item;

	public Image image;
	public Image background;

	public Action<ItemDropBubble> ItemClicked;
    public Action<ItemDropBubble> ItemAutoLooted;

    public float MaxVelocity = 50;
    public float MinVelocity = 20;
    public float DecelerationMagnitude = 50f;

    public void SetData(Item it, Action<ItemDropBubble> OnClick, Action<ItemDropBubble> OnAutoLoot) {
		item = it;
        image.sprite = item.Image;
        ItemClicked = OnClick;
        ItemAutoLooted = OnAutoLoot;
        
        image.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, item.Image.rect.width);
        image.gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, item.Image.rect.height);
		background.color = ConfigObject.GetColorFromRarity(it.Rarity);
	}

	[ContextMenu("Shoot")]
	public void Shoot() {
	    float magnitude = Random.value * (MaxVelocity - MinVelocity) + MinVelocity;
        StartCoroutine(_Shoot(VectorUtils.GetRandomDir() * magnitude));
	}

    private IEnumerator _Shoot(Vector2 velocity) {

        Vector2 dir = velocity.normalized;

        while (Vector2.Dot(velocity, dir) > 0) {
            velocity -= dir * DecelerationMagnitude * Time.deltaTime;
            ((RectTransform)transform).anchoredPosition += velocity * Time.deltaTime;
            yield return null;
        }
    }

	public void OnPointerClick(PointerEventData eventData) {
		ItemClicked.Invoke(this);
	}

    public void OnAutoLoot() {
        ItemAutoLooted.Invoke(this);
    }
}
