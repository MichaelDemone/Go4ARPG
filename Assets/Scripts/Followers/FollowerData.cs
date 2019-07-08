using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace G4AW2.Data {

	public abstract class FollowerData : ScriptableObject, IID, ISaveable {

		public int ID;
	    public string DisplayName;
	    public Sprite Portrait;

        public Vector2 SizeOfSprite = new Vector2(32,32);
        public int SpaceBetweenEnemies = 32;
		public AnimationClip SideIdleAnimation;
		public AnimationClip RandomAnimation;
		public bool HasRandomAnimation { get { return RandomAnimation != null; } }

		public float MinTimeBetweenRandomAnims = 30;
		public float MaxTimeBetweenRandomAnims = 180;

#if UNITY_EDITOR
		[ContextMenu("Pick ID")]
		public void PickID() {
			ID = IDUtils.PickID<FollowerData>();
		}
#endif

	    public virtual void AfterCreated() {
	        
	    }

		public int GetID() {
			return ID;
		}

	    private class SaveObject {
	        public int ID;
	    }

	    public virtual string GetSaveString() {
	        return JsonUtility.ToJson(new SaveObject() { ID = ID });
	    }

	    public virtual void SetData(string saveString, params object[] otherData) {
	        SaveObject ds = JsonUtility.FromJson<SaveObject>(saveString);

	        ID = ds.ID;

	        FollowerData original;

	        if(otherData[0] is PersistentSetFollowerData) {
	            PersistentSetFollowerData allFollowers = (PersistentSetFollowerData) otherData[0];
	            original = allFollowers.First(it => it.ID == ID);
	        } else {
	            original = otherData[0] as FollowerData;
	            if (SizeOfSprite == original.SizeOfSprite) return; // This object may have been create based on the original. In which case, we don't need to do any copying
	        }

	        ID = original.ID;
	        SizeOfSprite = original.SizeOfSprite;
	        SideIdleAnimation = original.SideIdleAnimation;
	        RandomAnimation = original.RandomAnimation;
	        MinTimeBetweenRandomAnims = original.MinTimeBetweenRandomAnims;
	        MaxTimeBetweenRandomAnims = original.MaxTimeBetweenRandomAnims;
	    }
	}

}
