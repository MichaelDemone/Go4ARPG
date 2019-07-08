using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

[System.Serializable]
public class MiningPoint {
    public float MinDistanceBetween = 54;
    public float MaxDistanceBetween = 1080;
    public Sprite Image;
    public ItemDropper Drops;
}
