using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Utils;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public ObservableFloat MaxHealth = new ObservableFloat(20);
    [NonSerialized] public ObservableFloat CurrentHealth = new ObservableFloat(20);


    [AutoSet(true)] public ColliderEvents Range;
    public string[] EnemyTags;

    public ObservableFloat ActionTime = new ObservableFloat(1.5f);
    [NonSerialized] public ObservableFloat CurrentActionTime = new ObservableFloat(0);

    public ProgressBarControllerFloat HealthBar;
    public ProgressBarControllerFloat ActionBar;

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
        Range.TriggerEntered2D += ColliderEnteredRange;
        Range.TriggerStayed2D += ColliderStayedRange;
        Range.TriggerExited2D += ColliderExitedRange;
    }

    // Start is called before the first frame update
    void Start() {
        ActionBar.SetData(CurrentActionTime, ActionTime);
        HealthBar.SetData(CurrentHealth, MaxHealth);
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
        if (targets.Count == 0) return;
        if (CurrentActionTime >= ActionTime) StartCoroutine(DoAttack());

    }

    private bool attacking = false;
    public IEnumerator DoAttack() {
        yield return new WaitForSeconds(0.5f);

        if (targets.Count == 0) yield break;
        if (CurrentActionTime < ActionTime) {
            Debug.LogWarning("Called Do Attack twice");
            yield break;
        }

        GameObject target = targets.GetRandom();
        target.GetComponent<PlayerCombat>().GetHurtBy(this, Damage);
        CurrentActionTime.Value = 0;
    }

    private List<GameObject> targets = new List<GameObject>();
    private void ColliderEnteredRange(Collider2D other) {
        if (EnemyTags.Contains(other.tag)) {
            targets.Add(other.gameObject);
        }
        UpdateTargets();
    }

    private void ColliderStayedRange(Collider2D other) {
    }

    private void ColliderExitedRange(Collider2D other) {
        targets.Remove(other.gameObject);
    }


    void OnMouseDown() {
        PlayerCombat.Instance.EnemyClicked(this);
    }

    public void InflictDamage(float damage) {
        CurrentHealth.Value -= damage;
        if (CurrentHealth <= 0) {
            Debug.Log("Death!");
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
