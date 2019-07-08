using CustomEvents;
using G4AW2.Questing;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Quests/Active/Reach Value")]
public class ReachValueQuest : ActiveQuestBase {

    public IntVariable TotalAmount;
    public int AmountToReach;

    public override void StartQuest(Action<ActiveQuestBase> onFinish) {
        base.StartQuest(onFinish);
        TotalAmount.OnChange.AddListener(OnTotalChange);
    }

    public override void ResumeQuest(Action<ActiveQuestBase> onFinish) {
        base.ResumeQuest(onFinish);
        TotalAmount.OnChange.AddListener(OnTotalChange);
    }

    public override void CleanUp() {
        base.CleanUp();
        TotalAmount.OnChange.RemoveListener(OnTotalChange);
    }

    public override bool IsFinished() {
        return TotalAmount >= AmountToReach;
    }

    private void OnTotalChange(int val) {
        if (val >= AmountToReach) {
            finished.Invoke(this);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public new void PickID() {
        ID = IDUtils.PickID<Quest>();
    }
#endif
}
