using System;
using CustomEvents;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data.DropSystem {
    [CreateAssetMenu(menuName = "Data/Items/Armor")]
    public class Armor : Item {

        [Range(0, 50)]
        public float ArmorAtLevel0;
        public ElementalWeaknessReference ElementalWeakness;
    }
}


