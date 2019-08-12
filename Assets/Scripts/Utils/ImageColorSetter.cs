using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ImageColorSetter : MonoBehaviour {

    [AutoSet] public SpriteRenderer Sr;

    void Awake() {
        AutoSet.Init(this);
    }

    public void SetR(float r) {
        Sr.color = Sr.color.SetR(r);
    }

    public void SetG(float g) {
        Sr.color = Sr.color.SetG(g);
    }

    public void SetB(float b) {
        Sr.color = Sr.color.SetB(b);
    }
}


public static class ColorExtension {
    public static Color SetR(this Color c, float r) {
        c.r = r;
        return c;
    }
    public static Color SetG(this Color c, float b) {
        c.b = b;
        return c;
    }
    public static Color SetB(this Color c, float g) {
        c.g = g;
        return c;
    }
}