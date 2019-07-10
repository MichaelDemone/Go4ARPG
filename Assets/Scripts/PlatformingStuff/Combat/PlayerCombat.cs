using System;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {

    public static PlayerCombat Instance;

    void Awake() {
        Instance = this;
    }

    public float Damage = 1;

    [NonSerialized] public float TimeDoAction = 1f;
    [NonSerialized] public float CurrentActionTime = 0f;

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
        CurrentActionTime = 0;
        Debug.Log("Smacked enemy!");
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (CurrentActionTime < TimeDoAction) {
            Debug.Log("Recovering action: " + CurrentActionTime);
            CurrentActionTime += Time.deltaTime;
        }
    }
}
