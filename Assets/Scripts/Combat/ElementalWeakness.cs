using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ElementalWeakness {

    [Serializable]
    public struct Weakness {
        public ElementalType ElementalType;
        public float DamageMultiplier;
    }

    public Weakness[] Weaknesses;

    public float this[ElementalType i]
    {
        get {
            Weakness w = Weaknesses.FirstOrDefault(weakness => weakness.ElementalType == i);
            if (default(Weakness).Equals(w)) return 1;
            return w.DamageMultiplier;
        }
    } 
}
