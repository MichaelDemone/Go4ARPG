using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConversationPopUp : MonoBehaviour {

    public static ConversationPopUp Instance;

    public TextMeshProUGUI Text;

    public bool Open = false;
    private void Awake() {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void ShowPopUp(string text, Action OnDone) {
        if(Open) {
            Debug.LogError("Show pop up called while text is already showing");
            return;
        }

        OpenDialog();
        StartCoroutine(CheckForMouseClick(OnDone));
        Text.text = text;
    }

    IEnumerator CheckForMouseClick(Action onDone) {
        while(true) {
            // Need to wait a frame so Input.GetMouseButtonUp isn't still true from frame where this was clicked.
            yield return null;

            if(!Open)
                break;

            if(Input.GetMouseButtonUp(0)) {
                onDone();
                CloseDialog();
                break;
            }
        }
    }

    public void OpenDialog() {
        gameObject.SetActive(true);
        Open = true;
    }

    public void CloseDialog() {
        Open = false;
        gameObject.SetActive(false);
    }
}
