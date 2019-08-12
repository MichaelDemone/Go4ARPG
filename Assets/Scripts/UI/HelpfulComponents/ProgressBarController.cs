using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour {

    [Header("Values")]
    public IntReference Max;
    public IntReference Current;

    [Header("UI")]
    public Image ProgressBarFill;

    void Awake() {
        if(!Max.UseConstant) {
            Max.Variable.OnChange.AddListener(UpdateUI);
        }
        if(!Current.UseConstant) {
            Current.Variable.OnChange.AddListener(UpdateUI);
        }
    }

    void Start() {
        UpdateUI(false);
	}

	private void UpdateUI(int i ) { UpdateUI(true);}

    public void SetMax( int Max ) {
        this.Max.Value = Max;
        UpdateUI(true);
    }

    public void SetCurrent( int Current ) {
        this.Current.Value = Current;
        UpdateUI(true);
    }

    [ContextMenu("Update")]
    public void UpdateUI(bool lerp) {

        if (Max == 0) {
            return;
        }

        StopAllCoroutines();

        if (gameObject.activeInHierarchy && lerp)
            StartCoroutine(Lerp(Mathf.Clamp01((float) Current / Max)));
        else {
            Vector3 scale = ProgressBarFill.rectTransform.localScale;
            scale.x = Mathf.Clamp01((float) Current / Max);
            ProgressBarFill.rectTransform.localScale = scale;
        }
    }

    public float TimeToCompleteLerp = 0.2f;

    IEnumerator Lerp(float end) {
        Vector3 scale = ProgressBarFill.rectTransform.localScale;
        float start = scale.x;
        float cur = start;

        float sign = end > start ? 1 : -1;
        float timeToLerp = Mathf.Abs(cur - end) / TimeToCompleteLerp;

        while ((sign > 0 && cur < end) || (sign < 0 && end < cur)) {
            cur += sign * Time.deltaTime * timeToLerp;
            scale.x = cur;
            ProgressBarFill.rectTransform.localScale = scale;
            yield return null;
        }

        scale.x = end;
        ProgressBarFill.rectTransform.localScale = scale;
    }
}
