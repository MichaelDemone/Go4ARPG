using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Sprite))]
public class Checkbox : MonoBehaviour, IPointerDownHandler {

    public Sprite NormalImage;
    public Sprite SelectedImage;

    public BoolReference Selected;

    private Image im;

    void Awake() {
        im = GetComponent<Image>();
        im.sprite = Selected ? SelectedImage : NormalImage;
    }

	// Use this for initialization
	public void OnAfterLoad () {
	    im.sprite = Selected ? SelectedImage : NormalImage;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Selected.Value = !Selected.Value;
        im.sprite = Selected ? SelectedImage : NormalImage;
    }
}
