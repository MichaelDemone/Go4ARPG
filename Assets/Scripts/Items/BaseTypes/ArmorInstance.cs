using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

[Serializable]
public class ArmorInstance : IItem, ITrashable {
    [NonSerialized] public Armor Data;

    public int DataId;
    public bool IsMarkedTrash;
    public int Level;
    public int Random;

    private float NoBlockModifierWithMod => Mathf.Max(1 - ARMValue / 100, 0);
    private float PerfectBlockModifierWithMod => Mathf.Max(0.25f - ARMValue / 400, 0); // at arm = 0, damage reduction is 75%
    private float MistimedBlockModifierWithMod => Mathf.Max(0.5f - ARMValue / 200, 0); // at arm = 0, damage reduction is 50%
    public float ARMValue => Mathf.RoundToInt(Data.ArmorAtLevel0 * Mod * (1 + Level / 100f));
    public float Mod => ModRoll.GetMod(Random);
    public string NameMod => ModRoll.GetName(Random);

    public enum BlockState { None, Blocking, PerfectlyBlocking, BadParry }

    public ArmorInstance(Armor data, int level) {
        Data = data;
        DataId = data.ID;
        Level = level;
        IsMarkedTrash = false;
        Random = UnityEngine.Random.Range(0, 101);
    }

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

    public int GetValue() {
        return Mathf.RoundToInt(Data.Value * (1 + Level / 10f) * (1 + Random / 100f));
    }

    public Item GetItem() {
        return Data;
    }

    public string GetName() {
        return $"{NameMod} {Data.Name}";
    }

    public string GetDescription() {
        return $"ARM Value: {ARMValue}\n" +
               $"{Data.Description}";
    }

    public bool IsTrash() {
        return IsMarkedTrash;
    }

    public void SetTrash(bool isTrash) {
        IsMarkedTrash = isTrash;
    }
}
