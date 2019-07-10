using System;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Combat {
	[CreateAssetMenu(menuName = "Data/Follower/Enemy")]
    public class EnemyData : ScriptableObject, ISaveable {

	    public int ID;
	    public string DisplayName;
	    public Sprite Portrait;

		[Header("Animations")]
		public AnimationClip Idle;
		public AnimationClip Flinch;
        public AnimationClip BeforeAttack;
        public AnimationClip AttackExecute;
		public AnimationClip AfterAttack;
		public AnimationClip Death;
		public AnimationClip Dead;
        public AnimationClip Walking;

	    public Sprite DeadSprite;
	    
	    [Header("Stats")]
        public float HealthAtLevel0;
	    public float DamageAtLevel0;

	    public float TimeBetweenAttacks;
        public float AttackPrepTime;
        public float AttackExecuteTime;

        [Header("Elemental")]
	    public bool HasElementalDamage;
	    public float ElementalDamageAtLevel0;
	    public ElementalType ElementalDamageType;
	    public ElementalWeaknessReference ElementalWeaknesses;

        [Header("Misc")]
		public ItemDropper Drops;
	    public bool OneAndDoneAttacker = false;

	    [NonSerialized] public int Level;

	    public int MaxHealth => Mathf.RoundToInt(HealthAtLevel0 * (1 + Level / 10f));
	    public int Damage => Mathf.RoundToInt(DamageAtLevel0 * (1 + Level / 10f));
	    public int ElementalDamage => Mathf.RoundToInt(ElementalDamageAtLevel0 * (1 + Level / 10f));

#if UNITY_EDITOR
        [ContextMenu("Print Stats")]
        public void PrintStats() {
            foreach(int i in new[] { 1, 5, 10, 15, 20, 25, 50, 100}) {
                this.Level = i;
                Debug.Log($"Stats at level {i}:\nHealth: {MaxHealth}\nDamage: {Damage}\nElemental Damage: {ElementalDamage}");
            }
            this.Level = 0;
        }
#endif

        public float GetElementalWeakness(ElementalType type) {
	        return ElementalWeaknesses.Value?[type] ?? 1;
	    }


        private class SaveObject {
	        public int ID;
	        public int Level;
	    }

	    public string GetSaveString() {
	        return JsonUtility.ToJson(new SaveObject() { ID = ID, Level = Level});
        }


	    public void SetData(string saveString, params object[] otherData) {

	        SaveObject ds = JsonUtility.FromJson<SaveObject>(saveString);

	        ID = ds.ID;
	        Level = ds.Level;

	        EnemyData original;

	        //if(otherData[0] is PersistentSetEnemyData) {
	        //    PersistentSetEnemyData allFollowers = (PersistentSetEnemyData) otherData[0];
	        //    original = allFollowers.First(it => it.ID == ID) as EnemyData;
	        //} else {
	            original = otherData[0] as EnemyData;
	            if(Idle == original.Idle)
	                return; // This object may have been create based on the original. In which case, we don't need to do any copying
            //}

            // Copy Original Values
	        DisplayName = original.DisplayName;
	        Idle = original.Idle;
            Flinch = original.Flinch;
            BeforeAttack = original.BeforeAttack;
            AttackExecute = original.AttackExecute;
            AfterAttack = original.AfterAttack;
            Death = original.Death;
            Dead = original.Dead;
            Walking = original.Walking;
	        ElementalWeaknesses = original.ElementalWeaknesses;
	        ElementalDamageType = original.ElementalDamageType;
        }
	}
}


