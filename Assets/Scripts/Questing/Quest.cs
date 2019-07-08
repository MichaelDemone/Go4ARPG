using UnityEngine;

namespace G4AW2.Questing {
    public class Quest : ScriptableObject, IID, ISaveable {

        public int ID;
        public string DisplayName;
        public int Level;

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public void PickID() {
            ID = IDUtils.PickID<Quest>();
        }
#endif
        public int GetID() {
            return ID;
        }
        public virtual string GetSaveString() {
            return "";
        }

        public virtual void SetData(string saveString, params object[] otherData) {
            
        }
    }
}


