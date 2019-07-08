using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class LerpToPosition : MonoBehaviour {

    public AnimationCurve XCurve;
    public AnimationCurve YCurve;

	public float TimeToLerp;
	public UnityEvent OnLerpingDone;

    public RectTransform RectTransformToLerp;

    private Vector2 steps;
	private float duration;
    private bool lerping = false;

    private void Awake() {
        if(RectTransformToLerp == null) 
            RectTransformToLerp = GetComponent<RectTransform>();
    }

    private Action currentLerpAction;
    public void StartLerping(Action a) {
        currentLerpAction = a;
        StartLerp();
    }

    // Use this for initialization
    public void StartLerping () {
        currentLerpAction = null;
        StartLerp();
	}

    private void StartLerp() {
        Keyframe frame = XCurve.keys[0];
        frame.value = RectTransformToLerp.anchoredPosition.x;
        XCurve.MoveKey(0, frame);

        frame = YCurve.keys[0];
        frame.value = RectTransformToLerp.anchoredPosition.y;
        YCurve.MoveKey(0, frame);

        lerping = true;
        duration = 0;
    }

	public void StopLerping() {
        Vector2 pos;
        pos.x = XCurve.keys[1].value;
        pos.y = YCurve.keys[1].value;
        RectTransformToLerp.anchoredPosition = pos;

        lerping = false;
        OnLerpingDone.Invoke();
        currentLerpAction?.Invoke();
    }

	// Update is called once per frame
	void Update () {
		if(lerping) {
            duration += Time.deltaTime;

            if(duration > TimeToLerp) {
                StopLerping();
                return;
            }

            Vector2 pos = new Vector2();
            pos.x = XCurve.Evaluate(duration);
            pos.y = YCurve.Evaluate(duration);
            RectTransformToLerp.anchoredPosition = pos;
        }
    }
}
