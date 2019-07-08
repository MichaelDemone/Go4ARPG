using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SmoothPopUpManager : MonoBehaviour {

    public GameObject PopUpPrefab;

    private ObjectPrefabPool popUpPool;
    private static SmoothPopUpManager instance;

    public AnimationCurve HeightCurve;
    public AnimationCurve OpacityCurve;

    private List<LerpData> currentLerpers = new List<LerpData>();

    private class LerpData {
        public TextMeshProUGUI PopUpText;
        public RectTransform PopUpPosition;
        public float StartY;
        public float Time;
    }

    void Awake() {
        popUpPool = new ObjectPrefabPool(PopUpPrefab, transform, 2);
        instance = this;
    }

    void Update() {

        foreach (var data in currentLerpers.ToList()) {
            data.Time += Time.deltaTime;

            if (OpacityCurve.keys[OpacityCurve.length-1].time <= data.Time) {
                currentLerpers.Remove(data);
                popUpPool.Return(data.PopUpText.gameObject);
                continue;
            }

            data.PopUpText.alpha = OpacityCurve.Evaluate(data.Time);
            Vector3 pos = data.PopUpPosition.anchoredPosition;
            pos.y = data.StartY + HeightCurve.Evaluate(data.Time);
            data.PopUpPosition.anchoredPosition = pos;
        }
    }

    private void ShowPopUpPrivate(Vector2 canvasSpaceStartPosition, string text, Color color, bool worldCoords) {
        var go = popUpPool.GetObject();

        TextMeshProUGUI t = go.GetComponent<TextMeshProUGUI>();
        t.text = text;
        t.color = color;
        RectTransform pos = go.GetComponent<RectTransform>();
        if (!worldCoords) pos.anchoredPosition = canvasSpaceStartPosition;
        else pos.position = canvasSpaceStartPosition;

        LerpData data = new LerpData();
        data.Time = 0;
        data.PopUpText = t;
        data.PopUpPosition = pos;
        data.StartY = pos.anchoredPosition.y;

        currentLerpers.Add(data);
    }

    public static void ShowPopUp(Vector2 canvasSpaceStartPosition, string text, Color color, bool worldCoords = false) {
        instance.ShowPopUpPrivate(canvasSpaceStartPosition, text, color, worldCoords);
    }
}
