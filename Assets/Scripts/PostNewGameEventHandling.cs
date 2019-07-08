using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class PostNewGameEventHandling : MonoBehaviour {

    public WeaponVariable PlayerWeapon;
    public ArmorVariable PlayerArmor;
    //public HeadgearVariable PlayerHeadgear;

    public void OnNewGame() {
        Weapon original = PlayerWeapon.Value;
        PlayerWeapon.Value = Instantiate(original);
        PlayerWeapon.Value.CreatedFromOriginal = true;
        PlayerWeapon.Value.Level = 1;
        PlayerWeapon.Value.TapsWithWeapon.Value = 0;
        PlayerWeapon.Value.Random = 30;
        PlayerWeapon.Value.SetValuesBasedOnRandom();

        Armor originalArmor = PlayerArmor;
        PlayerArmor.Value = Instantiate(originalArmor);
        PlayerArmor.Value.CreatedFromOriginal = true;
        PlayerArmor.Value.Level = 1;
        PlayerArmor.Value.Random = 50;
        PlayerArmor.Value.SetValuesBasedOnRandom();
    }
}
