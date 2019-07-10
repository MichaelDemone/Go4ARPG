using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

public class IDUtils {

#if UNITY_EDITOR
	public static int PickID<T>() where T : Object, IID {
		string[] paths = AssetDatabase.FindAssets("t:" + typeof(T).Name);
		for (int i = 0; i < paths.Length; i++) {
			paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
		}

		List<int> ids = paths.Select(AssetDatabase.LoadAssetAtPath<T>).Select(q => q.GetID()).ToList();

		for (int i = 1; i < paths.Length + 1; i++) {
			if (!ids.Contains(i)) {
				return i;
			}
		}
		throw new Exception("How is that possible?");
	}
#endif
}
