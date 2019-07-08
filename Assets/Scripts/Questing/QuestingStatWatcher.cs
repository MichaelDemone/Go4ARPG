using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using TMPro;
using UnityEngine;

public class QuestingStatWatcher : MonoBehaviour {

    public TextMeshProUGUI QuestTitle;
    public TextMeshProUGUI MaxText;
    public TextMeshProUGUI CurrentText;


    private ActiveQuestBase previous;

    public void SetQuest(ActiveQuestBase currentQuest) {

        RemoveListeners(previous);

        QuestTitle.text = currentQuest.DisplayName;
        if (currentQuest is ActiveWalkingQuest) {
            ActiveWalkingQuest awq = currentQuest as ActiveWalkingQuest;
            MaxText.text = "" + awq.AmountToReach;
            CurrentText.text = "" + awq.AmountSoFar.Value;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuest is ActiveQuest<int, IntVariable, UnityEventInt>) {
            ActiveQuest < int, IntVariable, UnityEventInt> awq = currentQuest as ActiveQuest<int, IntVariable, UnityEventInt>;
            MaxText.text = "" + awq.AmountToReach;
            CurrentText.text = "" + awq.AmountSoFar.Value;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuest is ReachValueQuest) {
            var q = currentQuest as ReachValueQuest;
            MaxText.text = "" + q.AmountToReach;
            CurrentText.text = "" + q.TotalAmount.Value;
            q.TotalAmount.OnChange.AddListener(OnChange);
        } 
        else if (currentQuest is BossQuest) {
            MaxText.text = "1";
            CurrentText.text = "0";
        }

        previous = currentQuest;
    }

    void RemoveListeners(ActiveQuestBase previousQuest) {
        if (previousQuest == null) return;

        if(previousQuest is ActiveWalkingQuest) {
            ActiveWalkingQuest awq = previousQuest as ActiveWalkingQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ActiveEnemySlayerQuest) {
            ActiveEnemySlayerQuest awq = previousQuest as ActiveEnemySlayerQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ActiveItemCollectQuest) {
            ActiveItemCollectQuest awq = previousQuest as ActiveItemCollectQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        }

        if(previousQuest is ActiveWalkingQuest) {
            ActiveWalkingQuest awq = previousQuest as ActiveWalkingQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ActiveQuest<int, IntVariable, UnityEventInt>) {
            ActiveQuest<int, IntVariable, UnityEventInt> awq = previousQuest as ActiveQuest<int, IntVariable, UnityEventInt>;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ReachValueQuest) {
            var q = previousQuest as ReachValueQuest;
            q.TotalAmount.OnChange.RemoveListener(OnChange);
        }
    }

    void OnChange(float val) {
        //SmoothPopUpManager.ShowPopUp(((RectTransform)transform).anchoredPosition, "+1", Color.green);
        CurrentText.text = "" + val;
    }

    public Transform SpawnPointOfNumberIncreasePopUp;

    private int prevVal = -1;
    void OnChange(int val) {
        if (prevVal != -1 && val > prevVal) {
            SmoothPopUpManager.ShowPopUp(SpawnPointOfNumberIncreasePopUp.position, "+" + (val - prevVal), Color.green, true);
        }
        prevVal = val;
        CurrentText.text = "" + val;
    }
}
