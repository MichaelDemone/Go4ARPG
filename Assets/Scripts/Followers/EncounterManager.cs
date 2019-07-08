using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterManager : MonoBehaviour {
	public EnemyDisplay Display;
	public RuntimeSetFollowerData CurrentFollowers;

	// Use this for initialization
	void Start () {
		if (Display.Enemy == null) {
			Display.Enemy = (EnemyData) CurrentFollowers.Value[0];
			CurrentFollowers.Value.RemoveAt(0);
		}	
	}
}
