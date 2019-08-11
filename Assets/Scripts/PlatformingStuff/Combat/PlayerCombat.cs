using System;
using G4AW2.Utils;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public static PlayerCombat Instance;

    public ObservableFloat MaxHealth = new ObservableFloat(30);
    public ObservableFloat CurrentHealth = new ObservableFloat(30);

    public ProgressBarControllerFloat ActionBar;
    public ProgressBarControllerFloat HealthBar;

    public float Damage = 1;

    void Awake() {
        Instance = this;
    }

    void Start() {
        //ActionBar.SetData(CurrentActionTime, TimeDoAction);
        HealthBar.SetData(CurrentHealth, MaxHealth);
    }

    // Update is called once per frame
    void Update() {
        if (CurrentActionTime < TimeDoAction) {
            CurrentActionTime.Value += Time.deltaTime;
        }
        else {
            CurrentActionTime.Value = TimeDoAction;
        }
    }


    [NonSerialized] public ObservableFloat TimeDoAction = new ObservableFloat(1f);
    [NonSerialized] public ObservableFloat CurrentActionTime = new ObservableFloat(0);

    public void GetHurtBy(Enemy source, float damageAmount) {
        CurrentHealth.Value -= damageAmount;
        if (CurrentHealth.Value <= 0) {
            Debug.Log("You have died!");
        }
    }
}
