using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarControllerFloat : MonoBehaviour {

	[Header("Values")]
	public FloatReference Max;
	public FloatReference Current;

	[Header("UI")]
	public Image ProgressBarFill;

	void OnEnable() {
		if (!Max.UseConstant)
			Max.Variable.OnChange.AddListener(UpdateUI);
		if (!Current.UseConstant)
			Current.Variable.OnChange.AddListener(UpdateUI);
	}

	void OnDisable() {
		if (!Max.UseConstant)
			Max.Variable.OnChange.RemoveListener(UpdateUI);
		if (!Current.UseConstant)
			Current.Variable.OnChange.RemoveListener(UpdateUI);
	}

	void Start() {
		UpdateUI();
	}

	private void UpdateUI( float i ) { UpdateUI(); }

	public void SetMax( float Max ) {
		this.Max.Value = Max;
		UpdateUI();
	}

	public void SetCurrent( float Current ) {
		this.Current.Value = Current;
		UpdateUI();
	}

	[ContextMenu("Update")]
	public void UpdateUI() {
		Vector3 scale = ProgressBarFill.rectTransform.localScale;
		if (Max == 0) {
			Debug.LogWarning("Max - min is zero. Object: " + name);
			return;
		}
		scale.x = Mathf.Clamp01(Current / Max);
		ProgressBarFill.rectTransform.localScale = scale;
	}
}
