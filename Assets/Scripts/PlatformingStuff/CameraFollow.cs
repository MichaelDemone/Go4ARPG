using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    [AutoSet] public Transform Camera;
    public Transform TransformToWatch;
    public Vector3 Offset;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }
#endif

    // Update is called once per frame
    void FixedUpdate() {
        Camera.position = Vector3.Lerp(Camera.position, TransformToWatch.position + Offset, 0.5f);
    }
}
