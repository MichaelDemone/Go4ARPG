using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.Events;

public class Lerper : MonoBehaviour {

	public float StartValue = 0;
	public float EndValue = 1;

	public float UnitPerSecond = 1;

	public bool currentlyLerping = false;

	public UnityEventFloat LerpUpdate;
	public UnityEvent DoneLerping;

	public void StartLerping() {
		if (currentlyLerping) {
			Debug.LogWarning("Already lerping...");
			return;
		}
		currentlyLerping = true;
		StartCoroutine(Lerp(StartValue, EndValue, UnitPerSecond));
	}

	private IEnumerator Lerp(float start, float end, float unitPerSecond) {
		float val = start;

        bool startLessThanEnd = start < end;

		while (true) {
			val += unitPerSecond * Time.deltaTime;
            LerpUpdate.Invoke(val);
			if ((startLessThanEnd && val >= end) || (!startLessThanEnd && val <= end)) {
				currentlyLerping = false;
				DoneLerping.Invoke();
				break;
			}
			yield return null;
		}

	}

	public void StopLerping() {
		currentlyLerping = false;
		StopAllCoroutines();
	}
}
