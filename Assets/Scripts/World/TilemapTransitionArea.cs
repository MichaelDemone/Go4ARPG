using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
[RequireComponent(typeof(TilemapCollider2D))]
[RequireComponent(typeof(TilemapRenderer))]
public class TilemapTransitionArea : MonoBehaviour {

    [AutoSet] public Tilemap Tilemap;
    [AutoSet] public TilemapCollider2D TilemapCollider;
    [AutoSet] public TilemapRenderer TilemapRenderer;
    [AutoSet(SetByNameInChildren = true)] public Collider2D TopCollider;
    [AutoSet(SetByNameInChildren = true)] public Collider2D BottomCollider;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }
#endif

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
