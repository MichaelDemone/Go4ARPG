using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Headgear")]
public class Headgear : Item, ITrashable, ISaveable {

    public int HealthGainedAtLevel0;

    public int ExtraHealth => Formulas.GetValue(HealthGainedAtLevel0, Level, Mod);

    public int Level { get; set; }
    public float Mod { get; set; }
    public string NameMod { get; set; }
    public int RandomRoll { get; set; }

    private bool isTrash = false;

    public void RollMod() {
        RandomRoll = Random.Range(0, 101);
        SetValuesBasedOnRandom();
    }

    public void SetValuesBasedOnRandom() {
        NameMod = ModRoll.GetName(RandomRoll);
        Mod = ModRoll.GetMod(RandomRoll);
    }

    public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
        return true;
    }

    public override string GetDescription() {
        return $"Level: {Level}\nHealth Add: {ExtraHealth}\nValue: {GetValue()}\n{Description}";
    }

    public override string GetName() {
        return $"{NameMod} {base.GetName()}";
    }

    public override int GetValue() {
        return Mathf.RoundToInt(Value * (1 + Level / 10f) * (1 + RandomRoll / 100f));
    }

    public override void OnAfterObtained() {
        RollMod();
    }

    public bool IsTrash() {
        return isTrash;
    }

    public void SetTrash(bool isTrash) {
        this.isTrash = isTrash;
        DataChanged?.Invoke();
    }

    private class DummySave {
        public int ID;
        public int Level = -1;
        public int RandomRoll = -1;
    }

    public string GetSaveString() {
        return JsonUtility.ToJson(new DummySave() { ID = ID, Level = Level, RandomRoll = RandomRoll });
    }

    public void SetData(string saveString, params object[] otherData) {

        DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

        ID = ds.ID;
        Level = ds.Level;
        RandomRoll = ds.RandomRoll;

        if(Level == -1) {
            Level = 1;
        }
        if(RandomRoll == -1) {
            RollMod();
        }

        SetValuesBasedOnRandom();

        if(CreatedFromOriginal)
            return;

        Headgear original;

        if(otherData[0] is PersistentSetItem) {
            PersistentSetItem allItems = (PersistentSetItem) otherData[0];
            original = allItems.First(it => it.ID == ID) as Headgear;
        } else {
            original = otherData[0] as Headgear;
        }

        HealthGainedAtLevel0 = original.HealthGainedAtLevel0;

        // Copy Original Values
        base.CopyValues(original);
    }
}
