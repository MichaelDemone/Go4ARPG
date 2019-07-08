using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.Dialogue {
	public class PopUp : MonoBehaviour {

		public TextMeshProUGUI Text;
		public Button SingleResponseButton;
		public Button[] TwoResponseButtons;
		public Button[] ThreeResponseButtons;

		public GameObject container;

	    public RobustLerperSerialized LerpOpen;

        private enum State { LerpingOpen, LerpingClosed, Open, Closed}

	    private State state = State.Closed;

		private static PopUp singleton;
		private bool inUse = false;

		void Awake() {
			singleton = this;
		}

	    void Update() {
	        LerpOpen.Update(Time.deltaTime);
	    }
	    public static void Close() {
	        singleton.container.SetActive(false);
	    }

		/// Set pop up text, returns a bool values based off of whether or not it is in use
		public static bool SetPopUp(string text, string[] options, Action[] responses) {
			return singleton.SetPopUpPriv(text, options, responses);
		}

		private bool SetPopUpPriv(string text, string[] options, Action[] responses) {
            transform.SetAsLastSibling();
			container.SetActive(true);

		    if (state != State.Closed) {
                LerpOpen.EndLerping(true);
		    }
		    else {
                state = State.LerpingOpen;
		        LerpOpen.StartLerping(() => {
		            state = State.Open;
		        });
		    }


			if (inUse)
				return false;
			if (options.Length > 3)
				throw new Exception("Options for pop up is larger than 3 elements");
			if (options.Length == 0)
				throw new Exception("Empty Options in pop up.");

			Text.text = text;

			Button[] single = {SingleResponseButton};

			single.ForEach(b => b.gameObject.SetActive(false));
			TwoResponseButtons.ForEach(b => b.gameObject.SetActive(false));
			ThreeResponseButtons.ForEach(b => b.gameObject.SetActive(false));

			Button[] response = null;
			if (options.Length == 1) response = single;
			else if (options.Length == 2) response = TwoResponseButtons;
			else if (options.Length == 3) response = ThreeResponseButtons;

			for (int i = 0; i < options.Length; i++) {
				response[i].gameObject.SetActive(true);
				response[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i];
				response[i].onClick.RemoveAllListeners();
				AddListener(response[i], i, responses);
			}

			return true;
		}

		private void AddListener(Button b, int i, Action[] responses) {
			b.onClick.AddListener(() => {
                state = State.LerpingClosed;
                LerpOpen.StartReverseLerp(() => {
                    state = State.Closed;
			        container.SetActive(false);
                });
            });
		    b.onClick.AddListener(() => responses[i]());
		}

    }
}

