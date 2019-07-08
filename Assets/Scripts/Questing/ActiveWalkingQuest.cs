using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Dialogue;
using G4AW2.Questing;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quests/Active/Walking")]
public class ActiveWalkingQuest : ActiveQuest<float, FloatVariable, UnityEventFloat> {

    protected override void OnTotalChanged(float totalAmount) {
        AmountSoFar.Value = totalAmount - amountWhenStarted;
        if (IsFinished()) {
            finished(this);
        }
    }
    
    protected override void UpdateAmountOnStart() {
        amountWhenStarted = TotalAmount - AmountSoFar;
    }

    public override bool IsFinished() {
        return AmountSoFar.Value >= AmountToReach && AmountToReach > 0;
    }

#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public new void PickID() {
        ID = IDUtils.PickID<Quest>();
    }
#endif
}
