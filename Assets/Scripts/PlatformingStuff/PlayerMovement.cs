using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public float MovementForceStrength = 10f;

    [AutoSet] public Rigidbody2D body;
    [AutoSet(SetByNameInChildren = true)] public SpriteRenderer View;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }
#endif

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

    private bool withinTopCollider = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("TransitionArea")) {
            other.GetComponentInParent<TilemapTransitionArea>().PlayerEntered(this);
            withinTopCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("TransitionArea")) {
            withinTopCollider = false;
            other.GetComponentInParent<TilemapTransitionArea>().PlayerExited(this);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(withinTopCollider && Input.GetKey(KeyCode.S)) {
            other.GetComponentInParent<TilemapTransitionArea>().PlayerAttemptEnterBottom(this);
        }
    }

    private void OnTransition(TilemapTransitionArea tta, Collider2D other) {
        if(other == tta.TopCollider) {
            tta.FadeOutTopTilemap();
            tta.TopColliderTilemap.GetComponent<TilemapCollider2D>().enabled = false;
        } else {
            tta.FadeOutBottomTilemap();
            tta.BottomColliderTilemap.GetComponent<TilemapCollider2D>().enabled = false;
        }
    }


}
