using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Utils {

	public static class VectorUtils {

		public static Vector3 BoundVector3( this Vector3 vec, Vector3 minBound, Vector3 maxBound ) {
			if (vec.x > maxBound.x) {
				vec.x = maxBound.x;
			}
			if (vec.x < minBound.x) {
				vec.x = minBound.x;
			}
			if (vec.y > maxBound.y) {
				vec.y = maxBound.y;
			}
			if (vec.y < minBound.y) {
				vec.y = minBound.y;
			}
			return vec;
		}

	    public static Vector2 GetRandomDir(float minMagnitude = 1f, float maxMagnitude = 1f) {
	        float x = Random.value - 0.5f;
	        float y = Random.value - 0.5f;
	        return new Vector2(x, y).normalized * Random.Range(minMagnitude, maxMagnitude);
	    }

        public static Vector3 SetX(this Vector3 vec, float val) {
	        vec.x = val;
	        return vec;
	    }

	    public static Vector3 SetY(this Vector3 vec, float val) {
	        vec.y = val;
	        return vec;
	    }

	    public static Vector3 SetZ(this Vector3 vec, float val) {
	        vec.y = val;
	        return vec;
	    }
    }
}

