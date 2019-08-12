using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using G4AW2.Utils;
using UnityEngine;

[Serializable]
public class EnemyDataInstance {
    public EnemyData Data;
    public Vector3 Position;
    public ObservableFloat CurrentHealth = new ObservableFloat(20);
    public int Health;
    public int Level;
    public int MaxHealth => Mathf.RoundToInt(Data.HealthAtLevel0 * (1 + Level / 10f));
    public int Damage => Mathf.RoundToInt(Data.DamageAtLevel0 * (1 + Level / 10f));
    public int ElementalDamage => Mathf.RoundToInt(Data.ElementalDamageAtLevel0 * (1 + Level / 10f));
}
