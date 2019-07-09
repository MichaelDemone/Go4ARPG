using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvent : MonoBehaviour {
    public string Text;

    private void OnMouseUp() {
        ConversationPopUp.Instance.ShowPopUp(Text, () => { });
    }
}
