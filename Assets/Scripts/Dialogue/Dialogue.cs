using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.Dialogue {
	public class Dialogue : MonoBehaviour {

		public Image Person;
		public TextMeshProUGUI Text;
		private bool playingConversation = false;
		private ConvoWithReturn currentConversation;
		private Queue<ConvoWithReturn> dialogueQueue = new Queue<ConvoWithReturn>();

		public void AdvanceConversation() {
			if (currentConversation == null) {
				Debug.LogError("Tried to advance a non-existent conversation");
				return;
			}
			currentConversation.OnReturn();
			playingConversation = false;
			ProcessQueue();
		}

		public void SetConversation( Conversation convo, Action onReturn ) {
			dialogueQueue.Enqueue(new ConvoWithReturn {convo = convo, OnReturn = onReturn});
			ProcessQueue();
		}

		private void ProcessQueue() {
			if (playingConversation) {
				return;
			}
			if (dialogueQueue.Count == 0) {
			    Close();
				return;
			}

			playingConversation = true;
			currentConversation = dialogueQueue.Dequeue();

			Person.sprite = currentConversation.convo.Speaker;
			Text.text = currentConversation.convo.Text;

		    Open();
		}

		private class ConvoWithReturn {
			public Conversation convo;
			public Action OnReturn;
		}

	    public RobustLerperSerialized OpenLerper;
	    private enum State { LerpingOpen, LerpingClosed, Open, Closed }
	    private State state = State.Closed;

        void Update() {
	        OpenLerper.Update(Time.deltaTime);
	    }

	    public void Open() {
	        gameObject.SetActive(true);

	        if(state != State.Closed) {
	            OpenLerper.EndLerping(true);
	        } else {
	            state = State.LerpingOpen;
	            OpenLerper.StartLerping(() => {
	                state = State.Open;
	            });
	        }
	    }

	    public void Close() {
	        state = State.LerpingClosed;
	        OpenLerper.StartReverseLerp(() => {
	            state = State.Closed;
	            gameObject.SetActive(false);
	        });
	    }
    }
}