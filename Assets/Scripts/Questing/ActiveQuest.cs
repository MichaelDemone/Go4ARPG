using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Dialogue;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif


namespace G4AW2.Questing {

    public class ActiveQuest<T, TVar, TEvent> : ActiveQuestBase, ISaveable
        where T : IComparable
        where TEvent : UnityEvent<T>, ISerializationCallbackReceiver, new()
        where TVar : Variable<T, TEvent> {

        [Header("Objective")]
        public TVar TotalAmount;

        public T AmountToReach;
        public TVar AmountSoFar;

        protected T amountWhenStarted;

        public override bool IsFinished() {
            throw new NotImplementedException();
        }

        public override void StartQuest(Action<ActiveQuestBase> onFinish) {
            finished = onFinish;
            AmountSoFar.Value = default(T);
            amountWhenStarted = TotalAmount;
            TotalAmount.OnChange.AddListener(OnTotalChanged);
        }

        public override void ResumeQuest(Action<ActiveQuestBase> onFinish) {
            finished = onFinish;
            UpdateAmountOnStart();
            TotalAmount.OnChange.AddListener(OnTotalChanged);
        }

        protected virtual void UpdateAmountOnStart() {
        }

        protected virtual void OnTotalChanged(T totalAmount) {
            if(IsFinished()) {
                finished.Invoke(this);
            }
        }

        public override void CleanUp() {
            TotalAmount.OnChange.RemoveListener(OnTotalChanged);
        }

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public new void PickID() {
            ID = IDUtils.PickID<Quest>();
        }
#endif


        [Serializable]
        private class DummySave {
            public int ID;
        }

        public override string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() { ID = ID });
        }

        public override void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ActiveQuest<T, TVar, TEvent> original;

            if (otherData[0] is PersistentSetQuest) {
                PersistentSetQuest quests = otherData[0] as PersistentSetQuest;
                original = quests.First(q => q.ID == ds.ID) as ActiveQuest<T, TVar, TEvent>;
            } else if (otherData[0] is ActiveQuest<T, TVar, TEvent>) {
                original = (ActiveQuest < T, TVar, TEvent > ) otherData[0];
            }
            else {
                throw new Exception("Wtf");
            }

            ID = original.ID;
            TotalAmount = original.TotalAmount;
            NextQuest = original.NextQuest;
            Area = original.Area;
            StartConversation = original.StartConversation;
            EndConversation = original.EndConversation;
            AmountToReach = original.AmountToReach;
        }
    }
}


