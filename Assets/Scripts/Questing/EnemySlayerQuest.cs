using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Quests/Passive/Slaying")]
    public class EnemySlayerQuest : PassiveQuest {
       
        public EnemyData Enemy;
        public int TotalToKill;
        public IntVariable KilledCount;

        private int startAmount = -1;

        public override void StartQuest(Action<PassiveQuest> onComplete) {
            base.StartQuest(onComplete);
            startAmount = KilledCount;
            KilledCount.OnChange.AddListener(EnemyKillCountChange);
        }

        public override void ResumeQuest(Action<PassiveQuest> onComplete) {
            base.ResumeQuest(onComplete);
            KilledCount.OnChange.AddListener(EnemyKillCountChange);
        }

        private void EnemyKillCountChange(int killed) {
            if(killed >= TotalToKill + startAmount) {
                FinishQuest();
            }
        }

        public override void FinishQuest() {
            base.FinishQuest();
            KilledCount.OnChange.RemoveListener(EnemyKillCountChange);
            Debug.Log("Killed " + TotalToKill + " " + Enemy.name + "s.");
        }

        [Serializable]
        private class DummySave {
            public int ID;
            public int StartAmount;
        }

        public override string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() {ID = ID, StartAmount = startAmount});
        }

        public override void SetData(string saveString, params object[] otherData) {

            PersistentSetPassiveQuest quests = otherData[0] as PersistentSetPassiveQuest;
            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            EnemySlayerQuest original = quests.First(q => q.ID == ds.ID) as EnemySlayerQuest;

            startAmount = ds.StartAmount;

            ID = original.ID;
            Enemy = original.Enemy;
            TotalToKill = original.TotalToKill;
            startAmount = original.startAmount;
            KilledCount = original.KilledCount;
        }
    }
}


