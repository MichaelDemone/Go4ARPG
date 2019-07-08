using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour {
    [NonSerialized] [AutoSet] public Transform t;
    public Transform transformToWatch;
    public Vector3 offset;

    void Awake() {
        AutoSet.Init(this);
    }

    // Update is called once per frame
    void FixedUpdate() {
        t.position = Vector3.Lerp(t.position, transformToWatch.position + offset, 0.5f);
    }
}
