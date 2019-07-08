using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Misc/GPSStrategies/Average")]
public class GPSStrategyAverage : GPSStrategy {

	private int GPSUpdatesBeforeAverage = 5;

	// Total lat and long for averaging
	private float totalLat = 0;
	private float totalLong = 0;

	// Amount of times GPS has updated
	private int gpsUpdates = 1;

	private float deltaTime = 0;
	private float deltaDistance;

	// Previous average lat and long
	private float prevLatitude, prevLongitude;
	private double timeOfLastDistanceUpdate;

	// Position on earth (in degrees)
	private float latitude;
	private float longitude;

	public override void Initialize() {
		gpsUpdates = 1;
		latitude = Input.location.lastData.latitude;
		longitude = Input.location.lastData.longitude;
		prevLatitude = latitude;
		prevLongitude = longitude;
		totalLong = prevLongitude;
		totalLat = prevLatitude;

		DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		timeOfLastDistanceUpdate = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
	}

	public override void GPSUpdated( Action<float, float> OnDistanceUpdate /*Distance and time*/) {
		totalLat += Input.location.lastData.latitude;
		totalLong += Input.location.lastData.longitude;
		gpsUpdates++;

		if (gpsUpdates == GPSUpdatesBeforeAverage) {
			longitude = totalLong / GPSUpdatesBeforeAverage;
			latitude = totalLat / GPSUpdatesBeforeAverage;

			updateChangeInTime();
			deltaDistance = Haversine(prevLongitude, prevLatitude, longitude, latitude) * 1000f;

			prevLongitude = longitude;
			prevLatitude = latitude;

			gpsUpdates = 1;
			totalLong = Input.location.lastData.longitude;
			totalLat = Input.location.lastData.latitude;

			OnDistanceUpdate(deltaDistance, deltaTime);
		}

		latitude = Input.location.lastData.latitude;
		longitude = Input.location.lastData.longitude;
	}

	private void updateChangeInTime() {
		DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		double currentTime = (DateTime.UtcNow - epochStart).TotalSeconds;
		deltaTime = (float)(currentTime - timeOfLastDistanceUpdate);
		timeOfLastDistanceUpdate = currentTime;
	}

	public override string GetTextUpdate() {
		return "Previous Latitude: " + prevLatitude + "\n" +
		       "Previous Longitude: " + prevLongitude + "\n" +
		       "Current Latitude: " + latitude + "\n" +
		       "Current Longitude: " + longitude + "\n" +
		       "Delta Distance: " + deltaDistance + "\n" +
		       "GPS updates: " + gpsUpdates;
	}
}
