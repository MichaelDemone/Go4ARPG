using UnityEngine;
using System.Collections;
using System;
using CustomEvents;
using G4AW2.Utils;
using UnityEngine.Events;

namespace G4AW2.GPS
{
	public enum LocationState {
		Disabled,
		TimedOut,
		Failed,
		Enabled,
		Stopped,
		Initializing
	}

	public class GPS : MonoBehaviour {

		public float desiredAccuracyInMeters = 1f;
		public float updateDistanceInMeters = 0f;
		public float timeBetweenChecks = 1f;
		public int maxInitializationTime = 15;

		public UnityEventFloat GPSUpdated;
		public UnityEventString GPSTextUpdate;
		public UnityEvent StatusUpdated;
		public GPSStrategy GpsStrategy;

		// Approximate radius of the earth (in kilometers)
		[ReadOnly] public LocationState state = LocationState.Initializing;

		// Timestamp of last data
		private double timestamp;

		private bool initialized = false;

		// Use this for initialization
		private void Awake() {
			state = LocationState.Initializing;
		}

		private IEnumerator Start() {
			yield return 0; // Delay for a frame so OnStateChange isn't called right away.
			yield return StartCoroutine(InitializeGPS());
		}

		private IEnumerator InitializeGPS() {
			if (initialized) {
				state = LocationState.Enabled;
				GpsStrategy.Initialize();
				StartCoroutine(CheckForUpdates());
				yield break;
			}

			if (!Input.location.isEnabledByUser) {
				state = LocationState.Disabled;
				OnStateUpdate();
				initialized = true;
				yield break;
			}

			Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
			int maxInitializationTime = this.maxInitializationTime;

			while (Input.location.status == LocationServiceStatus.Initializing && maxInitializationTime > 0) {
				yield return new WaitForSecondsRealtime(1);
				maxInitializationTime--;
			}

			if (maxInitializationTime <= 0) {
				state = LocationState.TimedOut;
			} else if (Input.location.status == LocationServiceStatus.Failed) {
				state = LocationState.Failed;
			} else {
				state = LocationState.Enabled;
				GpsStrategy.Initialize();
				StartCoroutine(CheckForUpdates());
			}
			OnStateUpdate();
			initialized = true;
		}

		private void OnStateUpdate() {
			if (state != LocationState.Enabled && state != LocationState.Initializing) {
				print("Could not connect to GPS!");
				//PopUp.instance.showPopUp("Could not connect to GPS!", new string[] { "Okay" });
			}
			StatusUpdated.Invoke();
		}

		private void OnApplicationPause( bool pauseState ) {

			StopAllCoroutines();

			if (pauseState) {
				// In background
				Debug.Log("In background");
				initialized = false;
				Input.location.Stop();
				state = LocationState.Stopped;
			} else {
				// No longer in background
				Debug.Log("No longer in background");
				state = LocationState.Initializing;
				StartCoroutine(InitializeGPS());
			}
		}

		private string lastString = "";
		void Update() {
			string s = GetGpsData();
			if (lastString != s) {
				lastString = s;
				GPSTextUpdate.Invoke(s);
			}
		}

		private IEnumerator CheckForUpdates() {
			timestamp = Input.location.lastData.timestamp;
			while (state == LocationState.Enabled) {
				if (timestamp >= Input.location.lastData.timestamp) {
					yield return new WaitForSecondsRealtime(timeBetweenChecks);
					continue;
				}
				timestamp = Input.location.lastData.timestamp;

				GpsStrategy.GPSUpdated(OnStrategyUpdate);
			}
		}

		private void OnStrategyUpdate(float distanceMoved, float timeTaken) {
		    if (distanceMoved < 5) {
		        // Moved less than x meters, assume stationary.
		    } else if (distanceMoved / timeTaken > 5.556f) { 
                // speed > 20 km / h, assume in vehicle
		        
		    } else if (timeTaken > 60 * 10) {
		        // > 10 min since last update, ignore this update. and reinitialize gps strat
                GpsStrategy.Initialize();
		    }
		    else {
		        GPSUpdated.Invoke(distanceMoved);
            }
        }

		private string GetGpsData() {

			string text;
			switch (state) {
				case LocationState.Enabled:
					DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0,
						DateTimeKind.Utc);

					double curTime = (DateTime.UtcNow - epochStart).TotalSeconds;
					float timeChanged = (float)(curTime - timestamp);
					int timeChange = Mathf.CeilToInt(timeChanged);

					text = "Time since last update: " + timeChange + "\n" +
					       GpsStrategy.GetTextUpdate();

					break;
				case LocationState.Disabled:
					text = "GPS disabled";
					break;
				case LocationState.Failed:
					text = "GPS failed";
					break;
				case LocationState.TimedOut:
					text = "GPS timed out";
					break;
				case LocationState.Stopped:
					text = "GPS stopped";
					break;
				case LocationState.Initializing:
					text = "GPS initializing";
					break;
				default:
					text = "GPS error occurred";
					break;
			}

			return text;
		}
	}

}
