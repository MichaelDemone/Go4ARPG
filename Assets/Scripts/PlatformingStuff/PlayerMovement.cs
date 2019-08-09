using System;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement Instance;

    public float MovementForceStrength = 10f;

    [AutoSet] public Rigidbody2D body;
    [AutoSet(SetByNameInChildren = true)] public SpriteRenderer View;

    [AutoSet] public Transform t;

    [AutoSet(CheckChildrenForComponents = true)] public PlayerAnimations Animations;
    [AutoSet(CheckChildrenForComponents = true, SetByNameInChildren = true)] public Transform Weapon;


    public float WeaponDistanceFromPlayer = 0.15f;
    public Vector3 WeaponOffset = new Vector2(0, 0.05f);

    private bool attacking = false;

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
        // Set Velocity
        {
            Vector2 vel = new Vector2();
            if(Input.GetKey(KeyCode.D)) {
                vel.x = 1;
            }
            if(Input.GetKey(KeyCode.A)) {
                vel.x += -1;
            }
            if(Input.GetKey(KeyCode.W)) {
                vel.y = 1;
            }
            if(Input.GetKey(KeyCode.S)) {
                vel.y += -1;
            }
            if(body.velocity.magnitude == 0 && vel.magnitude > 0) {
                // Started walking
                Animations.StartWalking();
            }
            if(body.velocity.magnitude != 0 && vel.magnitude == 0) {
                // Stopped walking
                Animations.StopWalking();
            }
            body.velocity = vel.normalized * MovementForceStrength;
        }

        // Set attack animations
        {
            if(Input.GetMouseButtonDown(0)) {
                Animations.Attack();
                attacking = true;
            }
            if(Input.GetMouseButtonUp(0)) {
                Animations.ResetAttack();
                attacking = false;
            }
        }


        Vector3 directionToMouseFromPlayer;

        // Set weapon rotation + position
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;
            Vector3 FakeMiddle = transform.position + WeaponOffset;
            directionToMouseFromPlayer = (mousePos - FakeMiddle).normalized;
            Vector3 weaponPosition = FakeMiddle + directionToMouseFromPlayer * WeaponDistanceFromPlayer;

            Weapon.position = weaponPosition;
            Weapon.right = directionToMouseFromPlayer;
            Weapon.localScale = Weapon.localScale.SetY(directionToMouseFromPlayer.x < 0 ? -1 : 1);
        }

        // Set body orientation
        {
            if(Math.Abs(body.velocity.x) > 0.01f)
                View.flipX = body.velocity.x < 0;

            if (attacking) {
                View.flipX = directionToMouseFromPlayer.x < 0;
            }
        }


    }

    private void OnCollisionEnter2D(Collision2D collision) {

    }

    private void OnCollisionExit2D(Collision2D collision) {

    }

    private void OnCollisionStay2D(Collision2D collision) {

    }

}
