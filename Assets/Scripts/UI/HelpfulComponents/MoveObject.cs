using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveObject : MonoBehaviour {

    public Vector2 StartMinVelocity;
    public Vector2 StartMaxVelocity;

	// Use this for initialization
	void OnEnable () {
	    float rand = Random.Range(0f, 1f);
	    Vector2 velocity = StartMinVelocity + rand * (StartMaxVelocity - StartMinVelocity);
	    GetComponent<Rigidbody2D>().velocity = velocity;
	}
}
