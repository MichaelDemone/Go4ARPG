using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Dialogue;
using G4AW2.Questing;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quests/Active/Collecting")]
public class ActiveItemCollectQuest : ActiveQuest<int, IntVariable, UnityEventInt> {

    protected override void OnTotalChanged(int totalAmount) {
        AmountSoFar.Value = totalAmount - amountWhenStarted;
        if(IsFinished()) {
            finished.Invoke(this);
        }
    }

    protected override void UpdateAmountOnStart() {
        amountWhenStarted = TotalAmount - AmountSoFar;
    }

    public override bool IsFinished() {
        return AmountSoFar.Value >= AmountToReach;
    }

#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public new void PickID() {
        ID = IDUtils.PickID<Quest>();
    }
#endif
}
