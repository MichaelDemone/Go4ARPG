using System;
using CustomEvents;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [CreateAssetMenu(menuName = "Data/Items/Armor")]
    public class Armor : Item, ISaveable, ITrashable {

        [Range(0, 50)]
        public float ArmorAtLevel0;
        public ElementalWeaknessReference ElementalWeakness;

        [NonSerialized]
        public bool IsMarkedTrash = false;

        [NonSerialized]
        public int Level = 1;

        [NonSerialized]
        public int Random = -1;
        private float mod = 1;
        private string nameMod;

        
        private float NoBlockModifierWithMod => Mathf.Max(1 - ARMValue / 100, 0);
        private float PerfectBlockModifierWithMod => Mathf.Max(0.25f - ARMValue / 400, 0); // at arm = 0, damage reduction is 75%
        private float MistimedBlockModifierWithMod => Mathf.Max(0.5f - ARMValue / 200, 0); // at arm = 0, damage reduction is 50%
        public float ARMValue => Mathf.RoundToInt(ArmorAtLevel0 * mod * (1 + Level / 100f));

        public enum BlockState { None, Blocking, PerfectlyBlocking, BadParry}

        public float GetDamage(int damage, BlockState state) {
            float fdamage = damage;
            fdamage = Mathf.Max(0, fdamage);

            if(state == BlockState.PerfectlyBlocking) {
                return fdamage * PerfectBlockModifierWithMod;
            }

            if(state == BlockState.Blocking) {
                return fdamage * MistimedBlockModifierWithMod;
            }

            if(state == BlockState.BadParry) {
                return fdamage;
            }

            return fdamage * NoBlockModifierWithMod;
        }

        public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return true;
        }

        public override void OnAfterObtained() {

            Random = UnityEngine.Random.Range(0, 101);
            SetValuesBasedOnRandom();
        }

        public override string GetName() {
            return $"{nameMod} {base.GetName()}";
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

        public override string GetDescription() {
            return $"ARM Value: {ARMValue}\n" +
                   $"{Description}";
        }

        public void SetValuesBasedOnRandom() {
            mod = ModRoll.GetMod(Random);
            nameMod = ModRoll.GetName(Random);
        }

        private class DummySave {
            public int ID;
            public int Random;
            public bool Trash;
            public int Level = 1;
        }

        public string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() { ID = ID, Random = Random, Trash = IsMarkedTrash, Level = Level});
        }

        public void SetData(string saveString, params object[] otherData) {

            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            ID = ds.ID;
            Random = ds.Random;
            IsMarkedTrash = ds.Trash;
            Level = ds.Level;

            SetValuesBasedOnRandom();

            if(CreatedFromOriginal)
                return;

            Armor original;

            if(otherData[0] is PersistentSetItem) {
                PersistentSetItem allItems = (PersistentSetItem) otherData[0];
                original = allItems.First(it => it.ID == ID) as Armor;
            } else {
                original = otherData[0] as Armor;
            }

            // Copy Original Values
            //OnAfterObtained(original);
            base.CopyValues(original);
            ArmorAtLevel0 = original.ArmorAtLevel0;
            ElementalWeakness = original.ElementalWeakness;
        }

        public bool IsTrash() {
            return IsMarkedTrash;
        }

        public void SetTrash(bool isTrash) {
            IsMarkedTrash = isTrash;
            DataChanged?.Invoke();
        }
    }
}


