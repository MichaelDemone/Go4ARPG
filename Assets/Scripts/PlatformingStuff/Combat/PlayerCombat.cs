using System;
using G4AW2.Utils;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public static PlayerCombat Instance;

    public ProgressBarControllerFloat ActionBar;

    void Awake() {
        Instance = this;
    }

    void Start() {
        ActionBar.SetData(CurrentActionTime, TimeDoAction);
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

    public float Damage = 1;

    [NonSerialized] public ObservableFloat TimeDoAction = new ObservableFloat(1f);
    [NonSerialized] public ObservableFloat CurrentActionTime = new ObservableFloat(0);

    public void EnemyClicked(Enemy e) {
        // Walk up & smack enemy
        PlayerMovement.Instance.AttemptToWalkToPoint(e.transform.position, GlobalDefines.GRID_SIZE, () => {
            if(CurrentActionTime >= TimeDoAction) {
                Smack(e);
            }
        });
    }

    public void Smack(Enemy e) {
        e.InflictDamage(Damage);
        CurrentActionTime.Value = 0;
        Debug.Log("Smacked enemy!");
    }
}
