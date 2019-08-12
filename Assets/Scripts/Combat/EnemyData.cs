using System;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data.Combat {
	[CreateAssetMenu(menuName = "Data/Follower/Enemy")]
    public class EnemyData : ScriptableObject {

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
	    public float Speed = 0.75f;

        [Header("Elemental")]
	    public bool HasElementalDamage;
	    public float ElementalDamageAtLevel0;
	    public ElementalType ElementalDamageType;
	    public ElementalWeaknessReference ElementalWeaknesses;

        [Header("Misc")]
		public ItemDropper Drops;
	    public bool OneAndDoneAttacker = false;

        public float GetElementalWeakness(ElementalType type) {
	        return ElementalWeaknesses.Value?[type] ?? 1;
	    }


        private class SaveObject {
	        public int ID;
	        public int Level;
	    }
	}
}


