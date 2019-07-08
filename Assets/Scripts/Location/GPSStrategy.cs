using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSStrategy : ScriptableObject {

	public virtual void Initialize() {
		throw new NotImplementedException();
	}

	public virtual void GPSUpdated(Action<float, float> OnDistanceUpdate /*Distance and time*/) {
		throw new NotImplementedException();
	}

	public virtual string GetTextUpdate() {
		throw new NotImplementedException();
	}

	private const float EARTH_RADIUS = 6371;

	// The Haversine formula
	// Veness, C. (2014). Calculate distance, bearing and more between
	//  Latitude/Longitude points. Movable Type Scripts. Retrieved from
	//  http://www.movable-type.co.uk/scripts/latlong.html
	public float Haversine( float lastLongitude, float lastLatitude, float currLongitude, float currLatitude ) {
		float deltaLatitude = (currLatitude - lastLatitude) * Mathf.Deg2Rad;
		float deltaLongitude = (currLongitude - lastLongitude) * Mathf.Deg2Rad;
		float a = Mathf.Pow(Mathf.Sin(deltaLatitude / 2), 2) +
		          Mathf.Cos(lastLatitude * Mathf.Deg2Rad) * Mathf.Cos(currLatitude * Mathf.Deg2Rad) *
		          Mathf.Pow(Mathf.Sin(deltaLongitude / 2), 2);
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
		return EARTH_RADIUS * c * 1000;
	}
}
