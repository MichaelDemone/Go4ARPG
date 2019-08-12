using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlphaOfAllChildren : MonoBehaviour {

	public void SetAlphaOfAllChildren(float alpha) {
        Image[] ims = GetComponentsInChildren<Image>();
        foreach(Image im in ims) {
            Color c = im.color;
            c.a = alpha;
            im.color = c;
        }

        TextMeshProUGUI []texts = GetComponentsInChildren<TextMeshProUGUI>();
	    foreach(TextMeshProUGUI text in texts) {
	        Color c = text.faceColor;
	        c.a = alpha;
	        text.faceColor = c;
	        Color o = text.outlineColor;
	        o.a = alpha;
	        text.outlineColor = o;
	    }
    }

    public void SetAlphaOfAllImages(float alpha) {
        Image[] ims = GetComponentsInChildren<Image>();
        foreach(Image im in ims) {
            Color c = im.color;
            c.a = alpha;
            im.color = c;
        }
    }
}
