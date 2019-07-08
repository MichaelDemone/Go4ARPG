using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Dialogue;
using TMPro;
using UnityEngine;

public class QuestViewer : MonoBehaviour {

    public TextMeshProUGUI Title;

    public ActiveQuestBaseVariable Quest;

    public Dialogue StartQuestDialogueBox;

    void Awake() {
        Refresh();
        Quest.OnChange.AddListener((e) => { Refresh(); });
    }


    public void Refresh() {
        Title.text = Quest.Value.DisplayName;
    }

    public void ViewBeginningText() {
        StartQuestDialogueBox.SetConversation(Quest.Value.StartConversation, () => { });
    }
}
