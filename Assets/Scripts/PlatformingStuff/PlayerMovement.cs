using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement Instance;

    public float MovementForceStrength = 10f;

    [AutoSet] public Rigidbody2D body;
    [AutoSet(SetByNameInChildren = true)] public SpriteRenderer View;

    [AutoSet] public Transform t;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }

    void OnEnable() {
        AutoSet.Init(this);
    }
#endif

    void Awake() {
        Instance = this;
    }

    // Update is called once per frame
    void Update() {
        Vector2 vel = new Vector2();
        if (Input.GetKey(KeyCode.D)) {
            vel.x = 1;
        } else if (Input.GetKey(KeyCode.A)) {
            vel.x = -1;
        } else if (Input.GetKey(KeyCode.W)) {
            vel.y = 1;
        } else if (Input.GetKey(KeyCode.S)) {
            vel.y = -1;
        }
        body.velocity = vel.normalized * MovementForceStrength;
        
        if(Math.Abs(vel.x) > 0.01f)
            View.flipX = vel.x < 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

    }

    private void OnCollisionExit2D(Collision2D collision) {

    }

    private void OnCollisionStay2D(Collision2D collision) {

    }

}
