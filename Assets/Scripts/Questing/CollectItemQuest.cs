using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Quests/Passive/Collecting")]
    public class CollectItemQuest : PassiveQuest {

        public Item Item;
        public int AmountToCollect;
        public IntVariable TotalCollected;

        private int startAmount = -1;


        public override void StartQuest(Action<PassiveQuest> onComplete) {
            base.StartQuest(onComplete);
            TotalCollected.OnChange.AddListener(CountChanged);
        }

        public override void ResumeQuest(Action<PassiveQuest> onComplete) {
            base.ResumeQuest(onComplete);
            TotalCollected.OnChange.AddListener(CountChanged);
        }

        private void CountChanged(int collected) {
            if(collected >= AmountToCollect + startAmount) {
                FinishQuest();
            }
        }

        public override void FinishQuest() {
            base.FinishQuest();
            TotalCollected.OnChange.RemoveListener(CountChanged);
            Debug.Log("Collected " + AmountToCollect + " " + Item.name + "s.");
        }

        [Serializable]
        private class DummySave {
            public int ID;
            public int StartAmount;
        }

        public override string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() { ID = ID, StartAmount = startAmount });
        }

        public override void SetData(string saveString, params object[] otherData) {

            PersistentSetPassiveQuest quests = otherData[0] as PersistentSetPassiveQuest;
            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            CollectItemQuest original = quests.First(q => q.ID == ds.ID) as CollectItemQuest;

            startAmount = ds.StartAmount;

            ID = original.ID;
            Item = original.Item;
            AmountToCollect = original.AmountToCollect;
            startAmount = original.startAmount;
            TotalCollected = original.TotalCollected;
        }
    }
}


