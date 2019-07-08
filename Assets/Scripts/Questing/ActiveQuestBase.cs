using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using G4AW2.Data.Area;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using G4AW2.Followers;
using UnityEngine;

namespace G4AW2.Questing {
    public class ActiveQuestBase : Quest {

        [Header("Progress")]
        public ActiveQuestBase NextQuest;
        public Conversation StartConversation;
        public Conversation EndConversation;

        [Header("Area Definitions")]
        public float MinEnemyDropTime = 240;
        public float MaxEnemyDropTime = 480;
        public float MinEnemyDropDistance = 50;
        public float MaxEnemyDropDistance = 250;
        public Area Area;
        public List<MiningPoint> MiningPoints;
        public FollowerDropData Enemies;
        public List<Reward> QuestRewards;

        
        protected Action<ActiveQuestBase> finished;

        [Serializable]
        public class Reward {
            public Item it;
            public int Level = -1;
            [Tooltip("-1 means roll it on creation")]
            public int RandomRoll = -1; 
        }

        public virtual void StartQuest(Action<ActiveQuestBase> onFinish) {
            finished = onFinish;
        }

        public virtual void ResumeQuest(Action<ActiveQuestBase> onFinish) {
            finished = onFinish;
        }

        public virtual bool IsFinished() { throw new NotImplementedException();}

        public virtual void CleanUp() { /*Remove listeners*/ }
    }
}
