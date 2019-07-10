using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarControllerFloat : MonoBehaviour {

	[Header("Values")]
	public ObservableFloat Max;
	public ObservableFloat Current;

	[Header("UI")]
	public Image ProgressBarFill;

    public TextMeshProUGUI Text;

	void Start() {
		UpdateUI();
	}

    public void SetData(ObservableFloat current, ObservableFloat max) {
        Max = max;
        Current = current;

        Max.OnValueChange += UpdateUI;
        Current.OnValueChange += UpdateUI;

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
			return;
		}
		scale.x = Mathf.Clamp01(Current / Max);
		ProgressBarFill.rectTransform.localScale = scale;
	    Text.text = $"{Current.Value:n2} / {Max.Value:n2}";

	}
}
