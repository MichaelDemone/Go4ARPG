using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour {
    public string MyTag;
    public string[] TargetableTags;
    public Collider2D ViewCollider;
    public Action OnAttack;

    [NonSerialized] public CombatController Target;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private List<Collider2D> NearbyEnemies = new List<Collider2D>(2);

    private void OnTriggerEnter2D(Collider2D other) {
        foreach(var tag in TargetableTags) {
            if(other.CompareTag(tag)) {
                // It's targetable!
                NearbyEnemies.Add(other);
                if(Target == null)
                    Target = other.GetComponent<CombatController>();
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        NearbyEnemies.Remove(other);
        if(Target.gameObject == other.gameObject) {
            // Target is out of range, try and find other nearby target
            if(NearbyEnemies.Count > 0) {
                Target = NearbyEnemies[0].GetComponent<CombatController>();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {

    }


}
