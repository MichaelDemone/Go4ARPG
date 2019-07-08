using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Saving;
using UnityEngine;

public class PlayerHealthIncreaser : MonoBehaviour {

    public IntVariable PlayerHealth;
    public IntVariable PlayerMaxHealth;
    public FloatVariable DistanceWalked;

    private float distWalkedInternal;

    public void OnGameStateLoaded() {
        DistanceWalked.OnChange.AddListener(AfterDistWalkedUpdate);
        distWalkedInternal = DistanceWalked;

        // Fin
        DateTime lastTimePlayedUTC = SaveManager.LastTimePlayedUTC;
        TimeSpan TimeSinceLastPlayed = DateTime.UtcNow - lastTimePlayedUTC;
        double secondsSinceLastPlayed = TimeSinceLastPlayed.TotalSeconds;
        IncreaseHealthByTime((int)secondsSinceLastPlayed);
    }

    private void AfterDistWalkedUpdate(float amt) {
        float change = amt - distWalkedInternal;
        distWalkedInternal = amt;
        PlayerHealth.Value = Mathf.Min(Mathf.RoundToInt(PlayerHealth.Value + change), PlayerMaxHealth.Value);
    }

    public int HealthPerSecond = 1;
    private float updateTime = 1f;

	// Update is called once per frame
	void Update () {
	    if (Time.time > updateTime) {
	        updateTime += 1f;
	        IncreaseHealthByTime(1);
	    }
	}

    public void IncreaseHealthByTime(float time) {
	    PlayerHealth.Value = Mathf.Min(Mathf.RoundToInt(PlayerHealth.Value + time * HealthPerSecond), PlayerMaxHealth.Value);
    }

    private DateTime PauseTime = DateTime.MaxValue;

    private void OnApplicationFocus(bool focus) {
        if (focus) {
            // Played
            if (PauseTime == DateTime.MaxValue) {
                Debug.LogWarning("Just played without pausing");
                return;
            }

            TimeSpan diff = DateTime.Now - PauseTime;
            IncreaseHealthByTime((float)diff.TotalSeconds);
        }
        else {
            // Paused
            PauseTime = DateTime.Now;
        }
    }

    private void OnApplicationPause(bool pause) {
        OnApplicationFocus(!pause);
    }
}
