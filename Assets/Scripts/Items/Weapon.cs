using System;
using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Utils;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    [CreateAssetMenu(menuName = "Data/Items/Weapon")]
    public class Weapon : Item, ISaveable, ITrashable {

        public PersistentSetItem AllItems;

        public int RawDamage => Mathf.RoundToInt(DamageAtLevel0 * MasteryDamageMod * (1 + Level / 10f) * mod);
        public int Mastery => 0;
        public float RawMastery => 0;
        private float MasteryDamageMod => Mastery == 99 ? 2.15f : 1 + Mastery / 100f;

        public float DamageAtLevel0;

        public bool IsEnchanted { get { return Enchantment != null; } }
        public Enchanter Enchantment { get; private set; }

        public GameEventWeapon LevelUp;

        [NonSerialized]
        public bool MarkedAsTrash = false;
        [NonSerialized]
        public ObservableInt TapsWithWeapon = new ObservableInt(1);
        [NonSerialized]
        public int Level = 1;

        [NonSerialized]
        public int Random = -1;
        private float mod;
        private string nameMod;


        void OnEnable() {
            if(Name == "")
                Name = name;
        }

        public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return true;
        }

        public override void OnAfterObtained() {

            Random = UnityEngine.Random.Range(0, 101);
            SetValuesBasedOnRandom();
            TapsWithWeapon.Value = 0;
        }

        public override string GetName() {
            return GetName(IsEnchanted, true);
        }

        public object GetSaveObject() {
            throw new NotImplementedException();
        }

        public Type GetSaveType() {
            throw new NotImplementedException();
        }

        public void SetData(object saveObject) {
            throw new NotImplementedException();
        }

        public string GetName(bool enchantInclude, bool includeNameMod) {
            if (enchantInclude && includeNameMod) {
                return $"{Enchantment.GetPrefix()} {nameMod} {base.GetName()}";
            }
            if (enchantInclude) {
                return $"{Enchantment.GetPrefix()} {base.GetName()}";
            }
            if (includeNameMod) {
                return $"{nameMod} {base.GetName()}";
            }

            return base.GetName();
            
        }

        public override string GetDescription() {
            if (IsEnchanted) {
                return $"Level: {Level}\nMastery: {Mastery}\nDamage: {RawDamage}\n{Enchantment.Type.name} Damage: {GetEnchantDamage()}\nValue: {GetValue()}\n{Description}";
            }
            return $"Level: {Level}\nMastery: {Mastery}\nDamage: {RawDamage}\nValue: {GetValue()}\n{Description}";
        }

        public override int GetValue() {
            return Mathf.RoundToInt(Value * (1 + Level / 10f) * (1 + Random / 100f)) + (IsEnchanted ? Enchantment.Value : 0);
        }

        public void SetValuesBasedOnRandom() {
            mod = ModRoll.GetMod(Random);
            nameMod = ModRoll.GetName(Random);
        }


        private class DummySave {
            public int ID;
            public int Random;
            public int Taps = 0;
            public bool Trash = false;
            public int Level = 1;
            public int EnchantID = -1;
            public string EnchantSave = "";
        }

        public string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() {ID = ID, Random = Random, Taps = TapsWithWeapon, Trash = MarkedAsTrash, Level = Level, EnchantID = Enchantment == null ? -1 : Enchantment.ID, EnchantSave = Enchantment == null ? "" : Enchantment.GetSaveString()});
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            Random = ds.Random;
            TapsWithWeapon.Value = ds.Taps;
            MarkedAsTrash = ds.Trash;
            Level = ds.Level;

            if(ds.EnchantID != -1 && CreatedFromOriginal) {
                Enchanter og = AllItems.First(it => it.ID == ds.EnchantID) as Enchanter;
                Enchantment = Instantiate(og);
                Enchantment.CreatedFromOriginal = true;
                Enchantment.SetData(ds.EnchantSave, og);
            }

            SetValuesBasedOnRandom();

            if(CreatedFromOriginal)
                return;

            Weapon original;

            if (otherData[0] is PersistentSetItem) {
                PersistentSetItem allItems = (PersistentSetItem) otherData[0];
                original = allItems.First(it => it.ID == ID) as Weapon;
            }
            else {
                original = otherData[0] as Weapon;
            }

            // Copy Original Values
            base.CopyValues(original);
            DamageAtLevel0 = original.DamageAtLevel0;
            LevelUp = original.LevelUp;
            AllItems = original.AllItems;

            if(ds.EnchantID != -1) {
                Enchanter og = AllItems.First(it => it.ID == ds.EnchantID) as Enchanter;
                Enchantment = Instantiate(og);
                Enchantment.CreatedFromOriginal = true;
                Enchantment.SetData(ds.EnchantSave, og);
            }
        }

        public bool IsTrash() {
            return MarkedAsTrash;
        }

        public void SetTrash(bool isTrash) {
            MarkedAsTrash = isTrash;
            DataChanged?.Invoke();
        }

        public void Enchant(Enchanter e) {
            Enchantment = e;
        }

        public int GetEnchantDamage() {
            return Enchantment == null ? 0 : Mathf.RoundToInt(Enchantment.GetAdditiveDamage(this));
        }
    }
}


