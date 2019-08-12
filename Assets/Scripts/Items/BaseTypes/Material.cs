using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data.DropSystem {

    public enum MaterialType {
        Ore = 0,
        Gem = 1,
        Monster = 2,
    }

    [CreateAssetMenu(menuName = "Data/Items/Material")]
    public class Material : Item {
        public MaterialType Type;

    }
}
