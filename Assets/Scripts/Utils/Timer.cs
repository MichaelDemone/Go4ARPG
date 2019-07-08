using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    private static Timer instance;

    private void Awake() {
        instance = this;
    }

    public static void StartTimer(float time, Action onFinish, MonoBehaviour actor = null) {
        (actor ?? instance).StartCoroutine(_StartTimer(time, onFinish));
    }

    private static IEnumerator _StartTimer(float time, Action OnFinish)
    {
        yield return new WaitForSeconds(time);
        OnFinish();
    }


    public static void StartTimer(float time, Action OnFinish, Action<float> OnUpdate, MonoBehaviour actor = null) {
        (actor ?? instance).StartCoroutine(_StartTimer(time, OnFinish, OnUpdate));
    }

    private static IEnumerator _StartTimer(float time, Action OnFinish, Action<float> OnUpdate) {
        float timeElapsed = 0;
        while (true) {
            timeElapsed += Time.deltaTime;
            if (time - timeElapsed <= 0) break;
            OnUpdate(timeElapsed);
            yield return null;
        }

        OnFinish();
    }
}
