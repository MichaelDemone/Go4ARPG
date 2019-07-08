using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public BoolVariable ShowParryAndBlockColor;
    public ActiveQuestBase BlockingAndParryingTutorialStart;
    public ActiveQuestBase BlockingAndParryingTutorialEnd;

    public void QuestUpdated(ActiveQuestBase Quest) {
        if (Quest == BlockingAndParryingTutorialStart) {
            ShowParryAndBlockColor.Value = true;
        }
        if (Quest == BlockingAndParryingTutorialEnd) {
            ShowParryAndBlockColor.Value = false;
        }
    }
}
