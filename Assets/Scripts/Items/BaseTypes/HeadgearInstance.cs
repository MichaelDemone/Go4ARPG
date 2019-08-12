using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class HeadgearInstance : IItem, ITrashable {
    [NonSerialized] public Headgear Data;

    public int DataId;
    public bool IsMarkedAsTrash;
    public int Random;
    public int Level;

    public int ExtraHealth => Formulas.GetValue(Data.HealthGainedAtLevel0, Level, Mod);
    public float Mod => ModRoll.GetMod(Random);
    public string NameMod => ModRoll.GetName(Random);

    public HeadgearInstance(Headgear data, int level) {
        Data = data;
        DataId = data.ID;
        Level = level;
        Random = UnityEngine.Random.Range(0, 101);
    }

    public bool IsTrash() {
        return IsMarkedAsTrash;
    }

    public void SetTrash(bool isTrash) {
        this.IsMarkedAsTrash = isTrash;
    }

    public string GetDescription() {
        return $"Level: {Level}\nHealth Add: {ExtraHealth}\nValue: {GetValue()}\n{Data.Description}";
    }

    public string GetName() {
        return $"{NameMod} {Data.Name}";
    }

    public int GetValue() {
        return Mathf.RoundToInt(Data.Value * (1 + Level / 10f) * (1 + Random / 100f));
    }

    public Item GetItem() {
        return Data;
    }
}
