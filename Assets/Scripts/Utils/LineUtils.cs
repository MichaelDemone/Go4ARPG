using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Utils {

	public static class LineUtils {

		public static void GetPointAlongLine( Vector3[] original, float speed, float time, Vector3[] destination ) {
			Debug.Assert(original.Length == destination.Length);
			float finalLen = time * speed;
			float len = 0;
			destination[0] = original[0];
			for (int i = 1; i < original.Length; i++) {
				// Travel along line adding points as you go
				destination[i] = original[i];
				len += (original[i] - original[i - 1]).magnitude;

				// If the line is suddenly too long
				if (len > finalLen) {
					// Figure out where it should en
					Vector3 diff = original[i] - original[i - 1];
					len -= diff.magnitude;
					float distToGo = finalLen - len;

					destination[i] = destination[i - 1] + (original[i] - original[i - 1]).normalized * distToGo;

					// Fill in the rest of the points with the last value
					int finalPoint = i;
					for (; i < original.Length; i++) {
						destination[i] = destination[finalPoint];
					}
					return;
				}

			}
		}

		public static float GetLengthOfLine(Vector3[] line) {
			float len = 0;
			for (int i = 1; i < line.Length; i++) {
				len += (line[i] - line[i - 1]).magnitude;
			}
			return len;
		}

		/// <summary>
		/// Checks if the line renderer intersects with the line between p0 and p1.
		/// </summary>
		/// <param name="lr"></param>
		/// <param name="p0"></param>
		/// <param name="p1"></param>
		/// <returns></returns>
		public static bool IsLineBetweenPoints( LineRenderer lr, Vector2 p0, Vector2 p1 ) {

			for (int i = 1; i < lr.positionCount; i++) {
				Vector2 p2 = lr.GetPosition(i - 1);
				Vector2 p3 = lr.GetPosition(i);

				Vector2 s1 = p1 - p0;
				Vector2 s2 = p3 - p2;

				// lel wtf
				float s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / (-s2.x * s1.y + s1.x * s2.y);
				float t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / (-s2.x * s1.y + s1.x * s2.y);

				if (s >= 0f && s <= 1f && t >= 0f && t <= 1f) {
					return true;
				}
			}
			return false;
		}
	}
}

