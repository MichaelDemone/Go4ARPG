using CustomEvents;
using G4AW2.Data.Combat;
using System;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Achievement")]
    public class Achievement : ScriptableObject, IID {

        public int ID;
        public Sprite AchievementIcon;
        [Multiline]
        public string AchievementCompletedText;
        public int NumberToReach;
        public IntVariable Number;
        public Action<Achievement> OnComplete;

        public void AchievementInit() {
            Number.OnChange.AddListener(CountChange);
        }

        private void CountChange(int killed) {
            if(killed >= NumberToReach) {
                FinishQuest();
            }
        }

        public void FinishQuest() {
            OnComplete?.Invoke(this);
            Number.OnChange.RemoveListener(CountChange);
            Debug.Log(AchievementCompletedText);
        }

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public void PickID() {
            ID = IDUtils.PickID<Achievement>();
        }
#endif
        public int GetID() {
            return ID;
        }
    }
}


