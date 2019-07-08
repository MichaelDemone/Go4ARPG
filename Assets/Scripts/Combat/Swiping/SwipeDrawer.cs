using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;

public class SwipeDrawer : MonoBehaviour {

    public LineRenderer lr;
    public float DelayBeforeDeath;
    public FloatReference MinSwipingDistance;

    public Color BlockColor;
    public Color ParryColor;

    // Use this for initialization
    void Start () {
    }

    private bool firstSpotRecorded = false;
    private Vector3 firstPos;

    public void SwipeStart() {
        StopAllCoroutines();
        firstSpotRecorded = false;
    }

    public void DrawVector(Vector3[] arr) {
        if (!firstSpotRecorded) {
            firstPos = arr[0];
            firstSpotRecorded = true;
        }

        Vector3 start = firstPos;
        Vector3 end = arr[arr.Length - 1];

        float swipeLength = Mathf.Min(Mathf.Abs(end.x - start.x), MinSwipingDistance);
        float percentComplete = 1f - swipeLength / MinSwipingDistance;

        Color color;

        if(start.x > end.x) {
            color = ParryColor;
        } else {
            color = BlockColor;
        }

        Color c = color + percentComplete * (lr.startColor - color);
        c.a = lr.endColor.a;
        lr.endColor = c;
    }

    public void OnDone() {
    }
}
