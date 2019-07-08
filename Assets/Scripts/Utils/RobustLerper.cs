using CustomEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RobustLerper : MonoBehaviour {

    public enum LoopType {
        CallOnEnd,
        Clamp,
        Restart,
        Reverse
    }

    public UnityEvent OnStart;
    public UnityEvent OnReverseStart;
    public UnityEvent OnUpdate;
    public UnityEvent OnEnd;
    public UnityEvent OnReverseEnd;

    public LoopType EndBehaviour;

    public bool AutoStart = false;
    public float LerpDuration = 1f;

    public List<LerpObject> Lerpers;

    private bool playing = false;
    private bool reverse = false;
    private float duration;

    private Action currentLerpingAction;

    public void StartLerping(Action a) {
        currentLerpingAction = a;
        StartLerp();
    }

    [ContextMenu("Play")]
    public void StartLerping() {
        currentLerpingAction = null;
        StartLerp();
    }

    private void StartLerp() {
        duration = 0;
        reverse = false;
        playing = true;

        foreach(var lerper in Lerpers) {
            lerper.Update(0);
        }

        OnStart.Invoke();
    }

    public void StartReverseLerp() {
        currentLerpingAction = null;
        ReverseLerp();
    }

    public void StartReverseLerp(Action a) {
        currentLerpingAction = a;
        ReverseLerp();
    }

    private void ReverseLerp() {
        duration = 0;
        reverse = true;
        playing = true;

        foreach(var lerper in Lerpers) {
            lerper.Update(LerpDuration);
        }

        OnReverseStart.Invoke();
    }

    [ContextMenu("Pause")]
    public void PauseLerp() {
        playing = true;
    }

    [ContextMenu("Stop")]
    public void EndLerping() {
        playing = false;

        foreach(LerpObject lerper in Lerpers) {
            lerper.Update(LerpDuration);
        }

        OnEnd.Invoke();
        currentLerpingAction?.Invoke();
    }

    public void EndReverseLerping() {
        playing = false;

        foreach(LerpObject lerper in Lerpers) {
            lerper.Update(0);
        }

        OnReverseEnd.Invoke();
        currentLerpingAction?.Invoke();
    }

    private void Start() {
        if(AutoStart) {
            StartLerping();
        }
    }

    // Update is called once per frame
    void Update () {
        if(!playing)
            return;

        duration += Time.deltaTime;
        float tempDuration = duration;

        if(reverse)
            tempDuration = LerpDuration - duration;

        if(EndBehaviour == LoopType.Clamp) {
            tempDuration = Mathf.Min(tempDuration, LerpDuration);
        } else if(EndBehaviour == LoopType.Restart) {
            tempDuration = tempDuration % LerpDuration;
        } else if(EndBehaviour == LoopType.Reverse) {
            tempDuration = tempDuration % (LerpDuration * 2);
            if(tempDuration > LerpDuration) {
                tempDuration = 2 * LerpDuration - tempDuration;
            }
        } else if (EndBehaviour == LoopType.CallOnEnd) {

            if(duration > LerpDuration) {
                if(reverse)
                    EndReverseLerping();
                else
                    EndLerping();
                return;
            }
        }

        foreach(LerpObject lerper in Lerpers) {
            lerper.Update(tempDuration);
        }
    }

    [System.Serializable]
    public class LerpObject {
        public AnimationCurve Values;
        public UnityEventFloat OnUpdate;

        public float Update(float duration) {
            OnUpdate.Invoke(Values.Evaluate(duration));
            return Values.Evaluate(duration);
        }
    }
}
