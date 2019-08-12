using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IconWithTextController : MonoBehaviour, IPointerClickHandler {

    public InventoryItemDisplay Item;
    public TextMeshProUGUI Text;

    private Action onClick;

    public void SetData(ItemInstance s, int amount, string text, Action onClick, Sprite overrideSprite = null, bool showText = true) {
        Item.SetData(s, amount, i => onClick(), overrideSprite, showText);
        Text.text = text;
        this.onClick = onClick;
    }

    public void OnPointerClick(PointerEventData eventData) {
        onClick?.Invoke();
    }
}
