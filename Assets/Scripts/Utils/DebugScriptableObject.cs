using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Misc/Debug/Debug Object")]
public class DebugScriptableObject : ScriptableObject {

	public void Print(string text) {
        Debug.Log(text);
    }
}
