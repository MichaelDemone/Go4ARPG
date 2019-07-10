using UnityEngine;

public class Enemy : MonoBehaviour {
    public float Health;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnMouseDown() {
        PlayerCombat.Instance.EnemyClicked(this);
    }

    public void InflictDamage(float damage) {
        Health -= damage;
        if (Health <= 0) {
            Debug.Log("Death!");
            Destroy(gameObject);
        }
    }
}
