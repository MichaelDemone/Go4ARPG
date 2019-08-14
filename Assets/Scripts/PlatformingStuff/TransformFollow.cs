using System;
using G4AW2.Utils;
using UnityEngine;

public class TransformFollow : MonoBehaviour {
    [AutoSet] public Transform ThisTransform;
    public Transform TransformToWatch;
    public Vector3 Offset;

    public float tolerance = 0.1f;
    public float lerpMod = 10f;

    public Vector2 MaxPosition;
    public Vector2 MinPosition;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }

    void OnEnable() {
        AutoSet.Init(this);
    }
#endif

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 newPos = Vector3.Lerp(ThisTransform.position, TransformToWatch.position + Offset, Time.deltaTime * lerpMod);
        if((ThisTransform.position - newPos).magnitude > tolerance)
            ThisTransform.position = newPos.BoundVector3(MinPosition, MaxPosition);
    }


}
