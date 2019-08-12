using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EditorUtils;
using G4AW2.Data.Combat;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This enemy will walk towards targets within their aggro area
/// if the target is within the damage zone and they have their action ready, the enemy will attack
/// If a target aggro'd this enemy, this enemy will follow it at full speed within the aggro area, and then follow it
/// at half speed in the aware area, and stop following it once it's out of all areas.
/// 
/// </summary>
public class Enemy : MonoBehaviour {



    [Header("Auto Set Items")]
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents AggroRange;
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents AwareRange;
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents DamageRange;
    [AutoSet] public Rigidbody2D Body;
    [AutoSet] public Animator Animator;
    [AutoSet] public SpriteRenderer Renderer;

    [Header("Misc")] public RobustLerperSerialized DeathFade;
    public RobustLerperSerialized OnHit;
    public ProgressBarControllerFloat HealthBar;
    public EnemyDataInstance EnemyInfo;

    [Header("Item Dropping")]
    public DroppedItem DroppedItemPrefab;
    public Transform DroppedItemParent;
    public float ItemSpawnDistance;

    private float timeBeforeCanAttack = 0;
    private bool attacking = false;
    private bool dead = false;

    void Awake() {
        AutoSet.Init(this);
        AggroRange.TriggerEntered2D += AggroZoneEntered;
        AggroRange.TriggerStayed2D += AggroZoneStayed;
        AggroRange.TriggerExited2D += AggroZoneExited;

        AwareRange.TriggerEntered2D += AwareZoneEntered;
        AwareRange.TriggerStayed2D += AwareZoneStayed;
        AwareRange.TriggerExited2D += AwareZoneExited;

        DamageRange.TriggerEntered2D += DamageZoneEntered;
        DamageRange.TriggerStayed2D += DamageZoneStayed;
        DamageRange.TriggerExited2D += DamageZoneExited;
    }

    // Start is called before the first frame update
    void Start() {
        HealthBar.SetData(EnemyInfo.CurrentHealth, EnemyInfo.MaxHealth);

        if (EnemyInfo.Data != null) {
            AnimatorOverrideController aoc = Animator.runtimeAnimatorController as AnimatorOverrideController;
            AnimatorOverrideController other = new AnimatorOverrideController(aoc);

            other["AfterAttack"] = EnemyInfo.Data.AfterAttack;
            other["AttackExecute"] = EnemyInfo.Data.AttackExecute;
            other["BeforeAttack"] = EnemyInfo.Data.BeforeAttack;
            other["Dead"] = EnemyInfo.Data.Dead;
            other["Death"] = EnemyInfo.Data.Death;
            other["Flinch"] = EnemyInfo.Data.Flinch;
            other["Idle"] = EnemyInfo.Data.Idle;
            other["Walking"] = EnemyInfo.Data.Walking;

            Animator.runtimeAnimatorController = other;
        }
    }


    // Update is called once per frame
    void Update() {
        DeathFade.Update(Time.deltaTime);
        if (attacking || dead) return;

        OnHit.Update(Time.deltaTime);

        Vector2 velocity = Vector2.zero;

        if (target != null) {
            if (targetType == TargetType.Damage) {
                if(Time.time >= timeBeforeCanAttack) {
                    // Attack!
                    attacking = true;
                    StartCoroutine(DoAttack());
                } else {
                    // Keep following target.
                    //velocity = (target.transform.position - transform.position).normalized;
                }
            } else if (targetType == TargetType.Aggro) {
                // Follow Target
                velocity = (target.transform.position - transform.position).normalized;
            } else if (targetType == TargetType.Following) {
                velocity = (target.transform.position - transform.position).normalized / 2f;
            }
        }

        Body.velocity = velocity * EnemyInfo.Data.Speed;
        if (velocity.magnitude > 00.01f) {
            Renderer.flipX = velocity.x < 0;
        }
        Animator.SetFloat("Speed", velocity.magnitude);
    }

    public IEnumerator DoAttack() {

        // Warm up
        Animator.SetTrigger("AttackStart");
        yield return new WaitForSeconds(EnemyInfo.Data.AttackPrepTime);

        // Execute attack
        Animator.SetTrigger("AttackExecute");
        yield return new WaitForSeconds(EnemyInfo.Data.AttackExecuteTime);

        Animator.SetTrigger("AttackEnd");
        if (target != null && targetType == TargetType.Damage) {
            Debug.Log("Hit: " + target);
            target.GetComponent<PlayerCombat>().GetHurtBy(this, EnemyInfo.Damage);
        }
        yield return new WaitForSeconds(0.5f);

        // Reset action time
        timeBeforeCanAttack = Time.time + EnemyInfo.Data.TimeBetweenAttacks;
        attacking = false;
    }

    public void InflictDamage(float damage) {
        if (dead) return;

        OnHit.StartLerping();
        Animator.SetTrigger("Flinch");

        EnemyInfo.CurrentHealth.Value -= damage;
        if(EnemyInfo.CurrentHealth <= 0) {
            Animator.SetTrigger("Death");
            StopAllCoroutines();
            Body.velocity = Vector2.zero;
            dead = true;
            DeathFade.StartLerping();

            foreach (var item in EnemyInfo.Data.Drops.GetItems(false)) {
                var go = Instantiate(DroppedItemPrefab, transform.position + (Vector3) VectorUtils.GetRandomDir(0, ItemSpawnDistance), Quaternion.identity, DroppedItemParent);
                go.SetItem(item);
            }
        }
    }

    #region AggroColliders

    [SerializeField] [ReadOnly] private GameObject target = null;
    [SerializeField] [ReadOnly] private TargetType targetType = TargetType.None;
    private enum TargetType { Damage = 3, Aggro = 2, Following = 1, None = 0 }
    private void AggroZoneEntered(Collider2D other) {
        if (target != null && targetType >= TargetType.Aggro) return;

        if(other.CompareTag("Player")) {
            target = other.gameObject;
            targetType = TargetType.Aggro;
        }
    }

    private void AggroZoneStayed(Collider2D other) {
    }

    private void AggroZoneExited(Collider2D other) {
        if(other.gameObject == target)
            targetType = TargetType.Following;
    }

    #endregion

    #region AwareColliders

    private void AwareZoneEntered(Collider2D other) {
    }

    private void AwareZoneStayed(Collider2D other) {
    }

    private void AwareZoneExited(Collider2D other) {
        if (other.gameObject == target)
            target = null;
    }

    #endregion

    #region DamageColliders

    private void DamageZoneEntered(Collider2D other) {
        if(target != null && targetType >= TargetType.Damage)
            return;

        if(other.CompareTag("Player")) {
            target = other.gameObject;
            targetType = TargetType.Damage;
        }
    }

    private void DamageZoneStayed(Collider2D other) {
    }

    private void DamageZoneExited(Collider2D other) {
        if(other.gameObject == target) targetType = TargetType.Aggro;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerWeapon")) {
            // You're getting hit. RIP.
            InflictDamage(PlayerCombat.Instance.Damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {

    }

    private void OnTriggerExit2D(Collider2D other) {

    }
}
