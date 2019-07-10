using CustomEvents;
using G4AW2.Data.DropSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Enchanter")]
public class Enchanter : Item, ISaveable {

    public enum GemType {
        Gem = 0,
        Jewel = 15,
        Crystal = 30,
    }

    public ElementalType Type;
    [Tooltip("Gem/Crystal/Other")]
    public GemType GemTypeType;

    private int RandomlyGeneratedValue;
    private const float DAMAGE_SCALING = 2.5F;
    public float GetAdditiveDamage(Weapon w) {
        return DAMAGE_SCALING * (1 + Type.GetDamage(RandomlyGeneratedValue) / 10f) * (1 + w.Level / 10);
    }

    public override void OnAfterObtained() {
        RandomlyGeneratedValue = UnityEngine.Random.Range(0, 101) + (int)GemTypeType;
    }

    public override bool ShouldCreateNewInstanceWhenPlayerObtained() {
        return true;
    }

    public override int GetValue() {
        return Mathf.RoundToInt(Value * (1 + RandomlyGeneratedValue / 100f));
    }

    public override string GetName() {
        return Type.GetPrefix(RandomlyGeneratedValue) + " " + base.GetName();
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

    public string GetPrefix() {
        return Type.GetPrefix(RandomlyGeneratedValue);
    }

    public override string GetDescription() {
        return $"Type: {Type.name}\nValue: {GetValue()}";
    }

    private class SaveObject {
        public int ID;
        public int RandomlyGeneratedNumber;
    }

    public string GetSaveString() {
        return JsonUtility.ToJson(new SaveObject() { RandomlyGeneratedNumber = RandomlyGeneratedValue, ID = ID });
    }

    public void SetData(string saveString, params object[] otherData) {

        SaveObject so = JsonUtility.FromJson<SaveObject>(saveString);
        ID = so.ID;
        RandomlyGeneratedValue = so.RandomlyGeneratedNumber;

        Enchanter original;

        if(otherData[0] is PersistentSetItem) {
            original = (otherData[0] as PersistentSetItem).First(it => it.ID == ID) as Enchanter;
        } else if (otherData[0] is Enchanter) {
            original = otherData[0] as Enchanter;
        } else {
            throw new Exception("Other data was not persistent set or enchanter");
        }

        GemTypeType = original.GemTypeType;
        Type = original.Type;
        CopyValues(original);
    }
}
