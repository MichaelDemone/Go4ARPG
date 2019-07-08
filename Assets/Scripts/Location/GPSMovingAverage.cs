using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Misc/GPSStrategies/Moving Average")]
public class GPSMovingAverage : GPSStrategy {

	private struct GPSUpdate {
		public float Latitude;
		public float Longitude;
		public double Timestamp;

		public GPSUpdate(float latitude, float longitude, double timestamp ) {
			Latitude = latitude;
			Longitude = longitude;
			Timestamp = timestamp;
		}
	}

	public int WindowSize = 20;
	public float TimeBetweenAverages = 30;

	private float currentAverageLat;
	private float currentAverageLong;
	private float lastGPSUpdateTime;
    private float lastDistanceUpdateTime;

	Queue<GPSUpdate> Updates = new Queue<GPSUpdate>();

	public override void Initialize() {
		Updates.Clear();
		lastGPSUpdateTime = Time.unscaledTime;
	    lastDistanceUpdateTime = Time.unscaledTime;

        currentAverageLat = Input.location.lastData.latitude;
		currentAverageLong = Input.location.lastData.longitude;
	}

	public override void GPSUpdated( Action<float, float> OnDistanceUpdate ) {
		Updates.Enqueue(new GPSUpdate(Input.location.lastData.latitude, Input.location.lastData.longitude, Input.location.lastData.timestamp));

		if (Updates.Count > WindowSize) {
			Updates.Dequeue();
		}

		if (Time.unscaledTime > lastGPSUpdateTime + TimeBetweenAverages) {
			int count = Updates.Count;
			float totalLat = 0;
			float totalLong = 0;

			for (int i = 0; i < count; i++) {
				GPSUpdate update = Updates.Dequeue();
				totalLat += update.Latitude;
				totalLong += update.Longitude;

				Updates.Enqueue(update);
			}

			float averageLat = totalLat / count;
			float averageLong = totalLong / count;

			float stdevLatSum = 0;
			float stdevLongSum = 0;

			for (int i = 0; i < count; i++) {
				GPSUpdate update = Updates.Dequeue();
				stdevLatSum += Mathf.Pow(update.Latitude - averageLat, 2); 
				stdevLongSum += Mathf.Pow(update.Longitude - averageLong, 2);
			}

			stdevLat = count == 1 ? 0 : Mathf.Sqrt(stdevLatSum / ((float)count - 1f)) * 2;
			stdevLong = count == 1 ? 0 : Mathf.Sqrt(stdevLongSum / ((float)count - 1f)) * 2;

			if (Mathf.Abs(averageLat - currentAverageLat) > stdevLat ||
			    Mathf.Abs(averageLong - currentAverageLong) > stdevLong) {

				// You've moved!
				distanceMoved = Haversine(currentAverageLong, currentAverageLat, averageLong, averageLat);

				OnDistanceUpdate(distanceMoved, Time.unscaledTime - lastDistanceUpdateTime);

				currentAverageLat = averageLat;
				currentAverageLong = averageLong;

			    lastDistanceUpdateTime = Time.unscaledTime;

			}
			else {
				distanceMoved = 0;
			}

			lastGPSUpdateTime = Time.unscaledTime;
		}

	}

	private float stdevLat, stdevLong;
	private float distanceMoved = 0;

	public override string GetTextUpdate() {
		return "Number of Updates in window: " + Updates.Count + "\n" +
		       "lastUpdateTime: " + lastGPSUpdateTime + "\n" +
		       "last amount of distance moved: " + distanceMoved + "m\n";
	}
}
