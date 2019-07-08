using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Questing {
    public class PassiveQuest : Quest {

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public new void PickID() {
            ID = IDUtils.PickID<Quest>();
        }
#endif

        public Action<PassiveQuest> OnComplete;

        public virtual void StartQuest(Action<PassiveQuest> onComplete) {
            OnComplete = onComplete;
        }

        public virtual void ResumeQuest(Action<PassiveQuest> onComplete) {
            OnComplete = onComplete;
        }

        public virtual void FinishQuest() {
            OnComplete?.Invoke(this);
        }
    }
}


