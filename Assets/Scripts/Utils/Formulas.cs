using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Formulas {

    public static int GetValue(int start, int level, float mod = 1) {
        return Mathf.RoundToInt(start * (1 + level / 10f) * mod);
    }
}
