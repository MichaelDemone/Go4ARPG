using System;
using CustomEvents;
using UnityEngine;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;

namespace G4AW2.Combat {

	[CreateAssetMenu(menuName = "Data/Player")]
	public class Player : ScriptableObject {

		public IntReference MaxHealth;

		public IntReference Health;
	    public IntReference Gold;

        public IntReference Level;
        public IntReference Experience;

		public FloatReference PowerPerBlock;
		public GameEvent OnPowerMax;

	    public GameEvent PlayerDeath;

        public WeaponReference Weapon;
        public ArmorReference Armor;
        public HeadgearReference Headgear;

		public void OnEnable() {
            Headgear.Variable.BeforeChange += UnequipHeadgear;
            Headgear.Variable.OnChange.AddListener(EquipHeadgear);

        }

		public void OnDisable() {
            Headgear.Variable.BeforeChange -= UnequipHeadgear;
            Headgear.Variable.OnChange.RemoveListener(EquipHeadgear);
        }

        public void UnequipHeadgear() {
            if(Headgear.Value != null) 
                MaxHealth.Value -= Headgear.Value.ExtraHealth;
        }

        public void EquipHeadgear(Headgear hg) {
            MaxHealth.Value += Headgear.Value.ExtraHealth;
        }

        public void DamagePlayer(int damage)
        {
            if (damage >= Health) {
                PlayerDeath.Raise();
                Health.Value = 0;
            }
            else {
                Health.Value -= damage;
            }
        }

	    public void OnDeathFinished() {
	        int oldAmount = Gold;
	        int newAmount = oldAmount - Mathf.RoundToInt(oldAmount * 0.2f);
	        newAmount = Mathf.Max(newAmount, 0);
	        Gold.Value = newAmount;

            Health.Value = MaxHealth;
	        PopUp.SetPopUp($"You died! You lost: {oldAmount - newAmount} gold.", new string[] {"Ok"}, new Action[] {() => { }});
	    }

		public int GetLightDamage() {
            return Weapon.Value.RawDamage;
		}

	    public int GetElementalDamage() {
	        return Weapon.Value.GetEnchantDamage();
	    }

#if UNITY_EDITOR
		[ContextMenu("Restore Health")]
		private void ResetHealth() {
			Health.Value = MaxHealth;
		}
#endif
	}

}
