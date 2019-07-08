using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Items/Enchanting Type")]
public class ElementalType : ScriptableObject {

    [System.Serializable]
    public class NamePrefix {
        public string Name;
        public int RandomValueMin;
        public int BaseDamage;
    }

    public Color DamageColor;
    public List<NamePrefix> NamePrefixes;

    public string GetPrefix(int random) {
        for(int i=NamePrefixes.Count-1; i >= 0; i--) {
            if(random >= NamePrefixes[i].RandomValueMin) {
                return NamePrefixes[i].Name;
            }
        }

        return NamePrefixes[NamePrefixes.Count - 1].Name;
    }

    public int GetDamage(int random) {
        foreach(var prefix in NamePrefixes) {
            if(random >= prefix.RandomValueMin) {
                return prefix.BaseDamage;
            }
        }

        return NamePrefixes[NamePrefixes.Count - 1].BaseDamage;
    }
}
