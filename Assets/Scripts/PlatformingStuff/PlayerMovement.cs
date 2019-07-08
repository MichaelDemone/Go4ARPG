using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {


    public float MovementForceStrength = 10f;

    [AutoSet] public Rigidbody2D body;
    [AutoSet(SetByNameInChildren = true)] public SpriteRenderer View;


    // Start is called before the first frame update
    void Start() {
        AutoSet.Init(this);
    }

    // Update is called once per frame
    void Update() {
        Vector2 vel = body.velocity;
        if (Input.GetKey(KeyCode.D)) {
            vel.x = MovementForceStrength;
        }
        else if (Input.GetKey(KeyCode.A)) {
            vel.x = -MovementForceStrength;

        } else {
            vel.x = 0;
        }
        body.velocity = vel;
        
        if(Math.Abs(vel.x) > 0.01f)
            View.flipX = vel.x < 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

    }

    private void OnCollisionExit2D(Collision2D collision) {

    }

    private void OnCollisionStay2D(Collision2D collision) {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("TransitionArea")) {

            other.gameObject.GetComponentInParent<TilemapCollider2D>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {

    }

    private void OnTriggerStay2D(Collider2D other) {

    }


}
