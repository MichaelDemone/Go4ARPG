using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Utils;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public ProgressBarControllerFloat HealthBar;
    public ProgressBarControllerFloat ActionBar;
    public ObservableFloat MaxHealth = new ObservableFloat(20);
    public ObservableFloat ActionTime = new ObservableFloat(1.5f);
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents AggroRange;
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents AwareRange;
    [AutoSet(SetByNameInChildren = true)] public ColliderEvents DamageRange;
    public string[] EnemyTags;


    [NonSerialized] public ObservableFloat CurrentHealth = new ObservableFloat(20);
    [NonSerialized] public ObservableFloat CurrentActionTime = new ObservableFloat(0);


    public float Damage;


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

    // Update is called once per frame
    void Update() {
        UpdateTargets();

        if (CurrentActionTime < ActionTime) {
            CurrentActionTime.Value += Time.deltaTime;
            if (CurrentActionTime >= ActionTime) {
                CurrentActionTime.Value = ActionTime;
                UpdateTargets();
            }
        }
    }

    public void UpdateTargets() {
        if (damageTargets.Count == 0) return;
        if (CurrentActionTime >= ActionTime) StartCoroutine(DoAttack());

    }

    public IEnumerator DoAttack() {
        yield return new WaitForSeconds(0.5f);

        if (damageTargets.Count == 0) yield break;
        if (CurrentActionTime < ActionTime) {
            Debug.LogWarning("Called Do Attack twice");
            yield break;
        }

        damageTargets.ForEach(t => t.GetComponent<PlayerCombat>().GetHurtBy(this, Damage));
        CurrentActionTime.Value = 0;
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

    private List<GameObject> aggroTargets = new List<GameObject>();
    private void AggroZoneEntered(Collider2D other) {
        if(EnemyTags.Contains(other.tag)) {
            aggroTargets.Add(other.gameObject);
        }
        UpdateTargets();
    }

    private void AggroZoneStayed(Collider2D other) {
    }

    private void AggroZoneExited(Collider2D other) {
        aggroTargets.Remove(other.gameObject);
    }

    #endregion

    #region AwareColliders

    private List<GameObject> awareTargets = new List<GameObject>();
    private void AwareZoneEntered(Collider2D other) {
        if(EnemyTags.Contains(other.tag)) {
            awareTargets.Add(other.gameObject);
        }
        UpdateTargets();
    }

    private void AwareZoneStayed(Collider2D other) {
    }

    private void AwareZoneExited(Collider2D other) {
        awareTargets.Remove(other.gameObject);
    }

    #endregion

    #region DamageColliders

    private List<GameObject> damageTargets = new List<GameObject>();
    private void DamageZoneEntered(Collider2D other) {
        if(EnemyTags.Contains(other.tag)) {
            damageTargets.Add(other.gameObject);
        }
        UpdateTargets();
    }

    private void DamageZoneStayed(Collider2D other) {
    }

    private void DamageZoneExited(Collider2D other) {
        damageTargets.Remove(other.gameObject);
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
