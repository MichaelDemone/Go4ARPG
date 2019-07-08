using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartUpEvent : MonoBehaviour {

    public UnityEvent OnStart;
    public UnityEvent OnAwake;

	// Use this for initialization
	void Start () {
        OnStart.Invoke();
	}
	
	// Update is called once per frame
	void Awake() {
        OnAwake.Invoke();
	}
}
