using System;
using G4AW2.Utils;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public static PlayerCombat Instance;

    public ObservableFloat MaxHealth = new ObservableFloat(30);
    public ObservableFloat CurrentHealth = new ObservableFloat(30);

    public ProgressBarControllerFloat HealthBar;

    public float Damage = 1;

    [Header("Misc")] public RobustLerperSerialized OnHitLerp;

    void Awake() {
        Instance = this;
    }

    void Start() {
        HealthBar.SetData(CurrentHealth, MaxHealth);
    }

    // Update is called once per frame
    void Update() {
        OnHitLerp.Update(Time.deltaTime);
        if (CurrentActionTime < TimeDoAction) {
            CurrentActionTime.Value += Time.deltaTime;
        }
        else {
            CurrentActionTime.Value = TimeDoAction;
        }
    }

    public float KnockbackForce = 10;
    public float KnockbackTime = 0.075f;

    [NonSerialized] public ObservableFloat TimeDoAction = new ObservableFloat(1f);
    [NonSerialized] public ObservableFloat CurrentActionTime = new ObservableFloat(0);

    public void GetHurtBy(Enemy source, float damageAmount) {
        CurrentHealth.Value -= damageAmount;
        DamageNumberSpawner.instance.SpawnNumber((int) damageAmount, Color.red, transform.position);
        PlayerMovement.Instance.Knockback(KnockbackTime, KnockbackForce, source.transform.position);
        OnHitLerp.StartLerping();
        if (CurrentHealth.Value <= 0) {
            Debug.Log("You have died!");
        }
    }
}
