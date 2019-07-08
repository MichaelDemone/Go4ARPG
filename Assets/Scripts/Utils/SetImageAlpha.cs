using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SetImageAlpha : MonoBehaviour {

    private Image im;
    private Image Image {
        get {
            if (im == null) im = GetComponent<Image>();
            return im;
        }
    }

    public void SetAlpha(float val) {
        Color c = Image.color;
        c.a = val;
        Image.color = c;
    }
}
