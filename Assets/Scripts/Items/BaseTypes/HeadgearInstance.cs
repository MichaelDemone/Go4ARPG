using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class HeadgearInstance : IItem, ITrashable {
    [NonSerialized] public Headgear Data;

    public int ExtraHealth => Formulas.GetValue(Data.HealthGainedAtLevel0, Level, Mod);

    public int Level { get; set; }
    public float Mod => ModRoll.GetMod(RandomRoll);
    public string NameMod => ModRoll.GetName(RandomRoll);
    public int RandomRoll { get; set; }

    private bool isTrash = false;

    public HeadgearInstance(Headgear data, int level) {
        Data = data;
        Level = level;
        RandomRoll = Random.Range(0, 101);
    }

    public bool IsTrash() {
        return isTrash;
    }

    public void SetTrash(bool isTrash) {
        this.isTrash = isTrash;
    }

    public string GetDescription() {
        return $"Level: {Level}\nHealth Add: {ExtraHealth}\nValue: {GetValue()}\n{Data.Description}";
    }

    public string GetName() {
        return $"{NameMod} {Data.Name}";
    }

    public int GetValue() {
        return Mathf.RoundToInt(Data.Value * (1 + Level / 10f) * (1 + RandomRoll / 100f));
    }

    public Item GetItem() {
        return Data;
    }
}
