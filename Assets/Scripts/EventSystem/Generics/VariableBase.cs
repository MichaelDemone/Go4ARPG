using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VariableBase : ScriptableObject {
	public virtual string GetSaveData() {
		throw new NotImplementedException();
	}

	public virtual void LoadString(string data) {
		throw new NotImplementedException();
	}
}
