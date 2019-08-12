using G4AW2.Data.DropSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Enchanter")]
public class Enchanter : Item {

    public enum GemType {
        Gem = 0,
        Jewel = 15,
        Crystal = 30,
    }

    public ElementalType Type;
    [Tooltip("Gem/Crystal/Other")]
    public GemType GemTypeType;
}
