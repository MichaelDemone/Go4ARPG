using System;
using G4AW2.Data.DropSystem;
using UnityEngine;

[Serializable]
public struct WeaponInstance : IItem {

    [NonSerialized] public Weapon Data;

    // Instance Data
    public int DataID;
    public bool MarkedAsTrash;
    public int Level;
    public int Random;
    public EnchanterInstance Enchantment { get; private set; }

    // Properties
    public int RawDamage => Mathf.RoundToInt(Data.DamageAtLevel0 * MasteryDamageMod * (1 + Level / 10f) * Mod);
    public int Mastery => 0;
    public float RawMastery => 0;
    private float MasteryDamageMod => Mastery == 99 ? 2.15f : 1 + Mastery / 100f;
    public bool IsEnchanted => Enchantment != null;
    private float Mod => ModRoll.GetMod(Random);
    private string NameMod => ModRoll.GetName(Random);

    public WeaponInstance(Weapon data, int level, EnchanterInstance enchantment) {
        DataID = data.ID;
        Data = data;
        Level = level;
        Random = UnityEngine.Random.Range(0, 101);
        Enchantment = enchantment;
        MarkedAsTrash = false;
    }

    public string GetDescription() {
        if(IsEnchanted) {
            return $"Level: {Level}\nMastery: {Mastery}\nDamage: {RawDamage}\n{Enchantment.Data.Type.name} Damage: {GetEnchantDamage()}\nValue: {GetValue()}\n{Data.Description}";
        }
        return $"Level: {Level}\nMastery: {Mastery}\nDamage: {RawDamage}\nValue: {GetValue()}\n{Data.Description}";
    }

    public int GetValue() {
        return Mathf.RoundToInt(Data.Value * (1 + Level / 10f) * (1 + Random / 100f)) + (IsEnchanted ? Enchantment.GetValue() : 0);
    }

    public Item GetItem() {
        return Data;
    }

    public bool IsTrash() {
        return MarkedAsTrash;
    }

    public void SetTrash(bool isTrash) {
        MarkedAsTrash = isTrash;
    }

    public void Enchant(EnchanterInstance e) {
        Enchantment = e;
    }

    public int GetEnchantDamage() {
        return Enchantment == null ? 0 : Mathf.RoundToInt(Enchantment.GetAdditiveDamage(this));
    }

    public string GetName() {
        return GetName(IsEnchanted, true);
    }

    public string GetName(bool enchantInclude, bool includeNameMod) {
        if(enchantInclude && includeNameMod) {
            return $"{Enchantment.GetPrefix()} {NameMod} {Data.Name}";
        }
        if(enchantInclude) {
            return $"{Enchantment.GetPrefix()} {Data.Name}";
        }
        if(includeNameMod) {
            return $"{NameMod} {Data.Name}";
        }

        return Data.Name;
    }
}
