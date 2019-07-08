using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Dialogue {
	[System.Serializable]
	public class Conversation {
        [Multiline(10)]
		public string Text;
		public Sprite Speaker;
	}
}


