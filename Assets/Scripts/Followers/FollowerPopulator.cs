using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using UnityEngine;

public class FollowerPopulator : MonoBehaviour {

	public RuntimeSetFollowerData From;
	public RuntimeSetFollowerData To;

	public Quest[] PossibleQuests;

	void Awake() {
		To.Value.Clear();
		To.Value.AddRange(From.Value);
	}

	
}
