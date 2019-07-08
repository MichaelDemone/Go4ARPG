using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils {
	public static class ListUtils {

		public static List<int> GetIndexes<T>(this List<T> list, T item) {
			return Enumerable.Range(0, list.Count).Where(i => list[i].Equals(item)).ToList();
		}
		
	}
}


