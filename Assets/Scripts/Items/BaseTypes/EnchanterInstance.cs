using G4AW2.Data.DropSystem;
using UnityEngine;

public class EnchanterInstance : IItem {
    public Enchanter Data;

    public int DataId;
    public int RandomlyGeneratedValue;

    public EnchanterInstance(Enchanter data) {
        Data = data;
        DataId = data.ID;
        RandomlyGeneratedValue = UnityEngine.Random.Range(0, 101) + (int) Data.GemTypeType;
    }

    private const float DAMAGE_SCALING = 2.5F;
    public float GetAdditiveDamage(WeaponInstance w) {
        return DAMAGE_SCALING * (1 + Data.Type.GetDamage(RandomlyGeneratedValue) / 10f) * (1 + w.Level / 10);
    }

    public int GetValue() {
        return Mathf.RoundToInt(Data.Value * (1 + RandomlyGeneratedValue / 100f));
    }

    public Item GetItem() {
        return Data;
    }

    public string GetName() {
        return Data.Type.GetPrefix(RandomlyGeneratedValue) + " " + Data.Name;
    }

    public string GetPrefix() {
        return Data.Type.GetPrefix(RandomlyGeneratedValue);
    }

    public string GetDescription() {
        return $"Type: {Data.Type.name}\nValue: {GetValue()}";
    }

}
