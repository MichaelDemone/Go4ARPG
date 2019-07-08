using System;
using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolderWithText : MonoBehaviour {

    public ItemViewer ItemSelector;

    public Button ButtonToSelectNewItem;
    public Image ItemImage;
    public TextMeshProUGUI ItemDescription;

    public ItemReference CurrentItem;

    private Action<Item> OnClick;

    public void Awake() {
        ButtonToSelectNewItem.onClick.AddListener(ItemClick);
    }

    public void SetItem(Item it, Action<Item> onClick) {
        OnClick = onClick;
        CurrentItem.Value = it;
        ItemImage.sprite = it.Image;
        ItemDescription.text = it.GetName() + "\n<size=80%>" + it.Description;
    }

    public void ItemClick() {
        OnClick?.Invoke(CurrentItem.Value);
    }
}
