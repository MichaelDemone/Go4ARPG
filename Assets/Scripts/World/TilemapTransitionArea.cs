using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapRenderer))]
public class TilemapTransitionArea : MonoBehaviour {

    [AutoSet] public Tilemap Tilemap;
    [AutoSet] public TilemapRenderer TilemapRenderer;
    [AutoSet(SetByNameInChildren = true)] public Collider2D TopCollider;
    [AutoSet(SetByNameInChildren = true)] public Collider2D BottomCollider;

    public Tilemap TopColliderTilemap;
    public Tilemap BottomColliderTilemap;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }
#endif

    public void FadeOutTopTilemap() {
        StartCoroutine(FadeOutTilemap(TopColliderTilemap));
    }

    public void FadeOutBottomTilemap() {
        StartCoroutine(FadeOutTilemap(BottomColliderTilemap));
    }

    public void FadeInTopTilemap() {
        StartCoroutine(FadeInTilemap(TopColliderTilemap));
    }

    public void FadeInBottomTilemap() {
        StartCoroutine(FadeInTilemap(BottomColliderTilemap));
    }

    IEnumerator FadeOutTilemap(Tilemap tmp) {
        float FadeTime = 1f;
        float f = 0;
        Color c = tmp.color;
        while(f < FadeTime) {
            c.a = 1.25f - f / FadeTime;
            tmp.color = c;
            f += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator FadeInTilemap(Tilemap tmp) {
        float FadeTime = 1f;
        float f = 0;
        Color c = tmp.color;
        while(f < FadeTime) {
            c.a = f / FadeTime + 0.25f;
            tmp.color = c;
            f += Time.deltaTime;
            yield return null;
        }
    }


    public bool currentlyInTop = true;

    public void PlayerEntered(PlayerMovement player) {
        if(currentlyInTop) {
            // Do nothing
        } else {
            // Fade out bottom and disable colliders
            currentlyInTop = true;
            FadeOutBottomTilemap();
            BottomColliderTilemap.GetComponent<TilemapCollider2D>().enabled = false;

            FadeInTopTilemap();
            TopColliderTilemap.GetComponent<TilemapCollider2D>().enabled = true;
        }
    }

    public void PlayerAttemptEnterBottom(PlayerMovement player) {
        if(!currentlyInTop) {
            Debug.Log("Trying to enter bottom tilemap but already there.");
            return;
        }

        currentlyInTop = false;

        FadeOutTopTilemap();
        TopColliderTilemap.GetComponent<TilemapCollider2D>().enabled = false;

        FadeInBottomTilemap();
        BottomColliderTilemap.GetComponent<TilemapCollider2D>().enabled = true;
    }

    public void PlayerExited(PlayerMovement player) {

    }
}
