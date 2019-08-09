using System;
using System.Collections;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement Instance;


    [Header("Autoset References")]
    [AutoSet] public Rigidbody2D body;
    [AutoSet(SetByNameInChildren = true)] public SpriteRenderer View;
    [AutoSet] public Transform t;
    [AutoSet(CheckChildrenForComponents = true)] public PlayerAnimations Animations;
    [AutoSet(CheckChildrenForComponents = true, SetByNameInChildren = true)] public Transform Weapon;

    [Header("Movement")]
    public float MovementForceStrength = 1f;

    [Header("Weapon")]
    public float WeaponDistanceFromPlayer = 0.15f;
    public Vector3 WeaponOffset = new Vector2(0, 0.05f);

    [Header("Dash")]
    public float DashForceStrength = 3f;
    public float MaxTimeBetweenButtonPushesForDash = 0.5f;
    public float DashingDuration = 0.1f;
    public float DashCooldownDuration = 0.4f;

    private bool dashing = false;
    private Vector2 dashDirection;
    private float dashingDurationEnd = 0;
    private float canDashAgainTime = 0;
    private bool CanDash => Time.time > canDashAgainTime;

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
        Vector3 vel;
        {
            if (dashing) {
                if (dashingDurationEnd <= Time.time) dashing = false;
                vel = dashDirection * DashForceStrength;
            }
            else {

                // Dash Checks
                if (CanDash) {
                    if(Input.GetKeyDown(KeyCode.D)) {
                        StartCoroutine(CheckDash(KeyCode.D));
                    }
                    if(Input.GetKeyDown(KeyCode.A)) {
                        StartCoroutine(CheckDash(KeyCode.A));
                    }
                    if(Input.GetKeyDown(KeyCode.W)) {
                        StartCoroutine(CheckDash(KeyCode.W));
                    }
                    if(Input.GetKeyDown(KeyCode.S)) {
                        StartCoroutine(CheckDash(KeyCode.S));
                    }
                }

                vel = new Vector2();
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

                vel = vel.normalized * MovementForceStrength;
            }
        }

        // Set Walking Animations
        {
            if(body.velocity.magnitude == 0 && vel.magnitude > 0) {
                // Started walking
                Animations.StartWalking();
            }
            if(body.velocity.magnitude != 0 && vel.magnitude == 0) {
                // Stopped walking
                Animations.StopWalking();
            }

            body.velocity = vel;
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

    #region Dashing


    IEnumerator CheckDash(KeyCode key) {
        float time = Time.time;


        // Key down -> Key up -> key down is a double tap 
        if(!Input.GetKeyDown(key))
            yield break;

        while(Time.time < time + MaxTimeBetweenButtonPushesForDash) {

            if(Input.GetKeyUp(key)) {
                break;
            }

            yield return null;
        }

        while(Time.time < time + MaxTimeBetweenButtonPushesForDash) {

            if(Input.GetKeyDown(key)) {
                break;
            }

            yield return null;
        }

        if(Time.time >= time + MaxTimeBetweenButtonPushesForDash || dashing)
            yield break;

        dashing = true;

        // Successful dash!
        if(key == KeyCode.A) {
            // Dash left
            dashDirection = Vector2.left;
        } else if(key == KeyCode.S) {
            // Dash down
            dashDirection = Vector2.down;
        } else if(key == KeyCode.D) {
            // Dash right
            dashDirection = Vector2.right;
        } else if(key == KeyCode.W) {
            // Dash up
            dashDirection = Vector2.up;
        }

        dashingDurationEnd = Time.time + DashingDuration;
        canDashAgainTime = Time.time + DashCooldownDuration;
    }

    #endregion


    private void OnCollisionEnter2D(Collision2D collision) {

    }

    private void OnCollisionExit2D(Collision2D collision) {

    }

    private void OnCollisionStay2D(Collision2D collision) {

    }

}
