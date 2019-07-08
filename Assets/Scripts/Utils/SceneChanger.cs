using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public LoadSceneMode LoadMode;

	public void ChangeScene(string sceneName) {
		SceneManager.LoadScene(sceneName, LoadMode);
	}

	public void ChangeScene(int sceneNumber) {
		SceneManager.LoadScene(sceneNumber, LoadMode);
	}
}
