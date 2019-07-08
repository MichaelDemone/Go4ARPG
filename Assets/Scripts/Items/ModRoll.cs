using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ModRoll {

	public static string GetName(int roll) {
        string nameMod;
        if(roll == 0) {
            nameMod = "Broken";
        } else if(roll >= 1 && roll <= 10) {
            nameMod = "Damaged";
        } else if(roll >= 11 && roll <= 30) {
            nameMod = "Inferior";
        } else if(roll >= 31 && roll <= 70) {
            nameMod = "Normal";
        } else if(roll >= 71 && roll <= 90) {
            nameMod = "Fine";
        } else if(roll >= 91 && roll <= 99) {
            nameMod = "Exquisite";
        } else if(roll == 100) {
            nameMod = "Masterwork";
        } else {
            nameMod = "WTF";
        }

        return nameMod;
    }

    public static float GetMod(int roll) {
        float mod;
        if(roll == 0) {
            mod = 0.5f;
        } else if(roll >= 1 && roll <= 10) {
            mod = 0.7f;
        } else if(roll >= 11 && roll <= 30) {
            mod = 0.85f;
        } else if(roll >= 31 && roll <= 70) {
            mod = 1;
        } else if(roll >= 71 && roll <= 90) {
            mod = 1.15f;
        } else if(roll >= 91 && roll <= 99) {
            mod = 1.3f;
        } else if(roll == 100) {
            mod = 1.5f;
        } else {
            mod = -1;
        }

        return mod;
    }
}
