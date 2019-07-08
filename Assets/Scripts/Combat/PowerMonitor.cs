using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.Events;

public class PowerMonitor : MonoBehaviour {

	public FloatReference Power;
	public FloatReference PowerDecreasePerSecond;

	public UnityEvent PowerDepleted;
	
	// Update is called once per frame
	void Update () {
		if (stunned) {
			Power.Value -= PowerDecreasePerSecond.Value * Time.deltaTime;
			if (Power <= 0) {
				Power.Value = 0;
				PowerDepleted.Invoke();
				stunned = false;
			}
		}
	}

	private bool stunned = false;
	public void MaxPowerAchieved() {
		stunned = true;
	}
}
