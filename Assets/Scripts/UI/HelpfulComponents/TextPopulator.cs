using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.UI {
	public class TextPopulator<TBase, TVar, TEvent> : MonoBehaviour where TEvent : UnityEvent<TBase>,new() where TVar : Variable<TBase, TEvent>  {

		public string DisplayText;
		public List<TVar> Reference;

		private TextMeshProUGUI Text;

		// Use this for initialization
		void Awake() {
			Text = GetComponentInChildren<TextMeshProUGUI>();
		    foreach (TVar va in Reference) {
		        va.OnChange.RemoveListener(UpdateUI); // just in case
		        va.OnChange.AddListener(UpdateUI);
            }
			
			UpdateUI(Reference[0].Value);
		}

		void UpdateUI( TBase num ) {
			Text.text = string.Format(DisplayText, Reference.Select(v => (object)v.Value).ToArray());
		}
	}
}

