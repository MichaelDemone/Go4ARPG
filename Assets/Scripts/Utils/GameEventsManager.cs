using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameEventsManager : MonoBehaviour {

	public UnityEvent OnSceneExitEvent;
	public UnityEvent OnAwake;
    public UnityEvent OnOnEnable;
    public UnityEvent OnStart;

	public UnityEvent OnFocus;
	public UnityEvent OnUnFocus;

	public UnityEvent OnPause;
	public UnityEvent OnPlay;

	public UnityEvent OnQuit;


	void Awake() {
		OnAwake.Invoke();
	}

    void OnEnable() {
        OnOnEnable.Invoke();
    }

	// Use this for initialization
	void Start () {
        OnStart.Invoke();
		SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
	}

	private void SceneManager_sceneUnloaded( Scene arg0 ) {
		OnSceneExitEvent.Invoke();
	}

	private void OnApplicationFocus( bool focus ) {
		if (focus) {
			OnFocus.Invoke();
		}
		else {
			OnUnFocus.Invoke();
		}
	}

	private void OnApplicationPause( bool pause ) {
		if (pause) {
			OnPause.Invoke();
		}
		else {
			OnPlay.Invoke();
		}
	}

	private void OnApplicationQuit() {
		OnQuit.Invoke();
	}


}
