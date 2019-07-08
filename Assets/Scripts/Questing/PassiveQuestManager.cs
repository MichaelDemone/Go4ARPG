using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using UnityEngine;

public class PassiveQuestManager : MonoBehaviour {

    public RuntimeSetPassiveQuest CurrentPassiveQuests;

	public void Initialize () {
	    foreach (var quest in CurrentPassiveQuests) {
	        quest.ResumeQuest(QuestComplete);
	    }
	}

    void QuestComplete(PassiveQuest quest) {
        Debug.Log("Completed: " + quest);
    }

    public void AddQuest(PassiveQuest quest) {
        PassiveQuest instanceQuest = Instantiate(quest);
        instanceQuest.StartQuest(QuestComplete);
        CurrentPassiveQuests.Add(instanceQuest);
    }
}
