using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EditorUtils;
using G4AW2.Utils;
using UnityEngine;

/// <summary>
/// This enemy will walk towards targets within their aggro area
/// if the target is within the damage zone and they have their action ready, the enemy will attack
/// If a target aggro'd this enemy, this enemy will follow it at full speed within the aggro area, and then follow it
/// at half speed in the aware area, and stop following it once it's out of all areas.
/// 
/// </summary>
public class Enemy : MonoBehaviour {

    [Header("Health and Action bar Stuff")]
    public ProgressBarControllerFloat HealthBar;
    public ProgressBarControllerFloat ActionBar;
    public ObservableFloat MaxHealth = new ObservableFloat(20);
    [NonSerialized] public ObservableFloat CurrentHealth = new ObservableFloat(20);


    [Header("Auto Set Items")]
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents AggroRange;
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents AwareRange;
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents DamageRange;
    [AutoSet] public Rigidbody2D Body;


    [Header("Combat")]
    public string[] EnemyTags;

    public float Speed = 0.75f;

    [Header("Attacking")]
    public float WarmUpTime = 0.5f; // i.e gnome chompy opens his mouth and a circle appears around him
    public float ExecuteAttackTime = 0.1f; // He slams his mouth down and the damage is done
    public float CooldownTime = 3f;
    public float Damage;
    private float timeBeforeCanAttack = 0;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }
    void OnEnable() {
        AutoSet.Init(this);
    }
#endif

    void Awake() {
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
        //ActionBar.SetData(CurrentActionTime, ActionTime);
        //HealthBar.SetData(CurrentHealth, MaxHealth);
    }

    private bool attacking = false;

    // Update is called once per frame
    void Update() {

        if (attacking) return;

        Vector2 velocity = Vector2.zero;

        if (target != null) {
            if (targetType == TargetType.Damage) {
                if(Time.time >= timeBeforeCanAttack) {
                    // Attack!
                    attacking = true;
                    StartCoroutine(DoAttack());
                } else {
                    // Keep following target.
                    velocity = (target.transform.position - transform.position).normalized;
                }
            } else if (targetType == TargetType.Aggro) {
                // Follow Target
                velocity = (target.transform.position - transform.position).normalized;
            }
        }

        Body.velocity = velocity * Speed;

    }

    public IEnumerator DoAttack() {

        // Warm up
        Debug.Log("Warming up");
        yield return new WaitForSeconds(WarmUpTime);

        // Execute attack
        Debug.Log("Executing");
        yield return new WaitForSeconds(ExecuteAttackTime);

        Debug.Log("Attacking");
        if (target != null && targetType == TargetType.Damage) {
            Debug.Log("Hit: " + target);
            target.GetComponent<PlayerCombat>().GetHurtBy(this, Damage);
        }
        
        // Reset action time
        timeBeforeCanAttack = Time.time + CooldownTime;
        attacking = false;
    }

    public void InflictDamage(float damage) {
        CurrentHealth.Value -= damage;
        Debug.Log("Current Health: " + CurrentHealth.Value);
        if(CurrentHealth <= 0) {
            Debug.Log("Death!");
            Destroy(gameObject);
        }
    }

    #region AggroColliders

    [SerializeField] [ReadOnly] private GameObject target = null;
    [SerializeField] [ReadOnly] private TargetType targetType = TargetType.None;
    private enum TargetType { Damage = 3, Aggro = 2, None = 0 }
    private void AggroZoneEntered(Collider2D other) {
        if (target != null && targetType >= TargetType.Aggro) return;

        if(EnemyTags.Contains(other.tag)) {
            target = other.gameObject;
            targetType = TargetType.Aggro;
        }
    }

    private void AggroZoneStayed(Collider2D other) {
    }

    private void AggroZoneExited(Collider2D other) {
        // Once the target has aggro'd the enemy, the enemy will be aggro'd until the enemy leaves the aware collider
        //if(other.gameObject == target)
        //    targetType = TargetType.Aware;
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

        if(EnemyTags.Contains(other.tag)) {
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
