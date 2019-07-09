using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFader : MonoBehaviour {

    [AutoSet] public Tilemap Tilemap;

    public float FadeTime = 0.5f;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }
#endif

    [ContextMenu("Reset")]
    public void ResetAlpha() {
        Color c = Tilemap.color;
        c.a = 1;
        Tilemap.color = c;
    }

    [ContextMenu("Fade")]
    public void Fade() {
        StartCoroutine(fade());
    }

    IEnumerator fade() {
        float f = 0;
        Color c = Tilemap.color;
        while (f < FadeTime) {
            c.a = 1 - f / FadeTime;
            Tilemap.color = c;
            f += Time.deltaTime;
            yield return null;
        }
    }
}
