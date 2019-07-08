using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickPopUp : MonoBehaviour {

    public BoolVariable QuickPopUpAllowed;
    public RobustLerper PositionLerper;
    public RobustLerper AlphaLerper;
    public RobustLerperSerialized ShakeLerper;

    public Image Image;
    public TextMeshProUGUI Text;

    private bool alreadyActive = false;
    private Queue<PopUpData> PopUpsToShow = new Queue<PopUpData>();
    private bool alphaLerpRunning = false;

    public bool IsMainInstance = false;

    private static QuickPopUp instance;

    private class PopUpData {
        public Sprite Icon;
        public string Text;
    }

    void Awake() {
        if(IsMainInstance) {
            instance = this;
        }
        QuickPopUpAllowed.OnChange.AddListener(QuickPopUpAllowedChanged);
    }

    void QuickPopUpAllowedChanged(bool val) {
        if (QuickPopUpAllowed) {
            StartPopUp();
        }
        else {
            // Do nothing, user has to dismiss current one
        }
    }

    public void ShowSprite(Sprite icon, string text) {
        if(alreadyActive) {
            Shake();
        }
        AddToQueue(icon, text);
        StartPopUp();
    }

    public static void Show(Sprite icon, string text) {
        if (instance.alreadyActive) {
            instance.Shake();
        }
        instance.AddToQueue(icon, text);
        instance.StartPopUp();
    }

    public void Shake() {
        ShakeLerper.StartLerping();
    }

    private void AddToQueue(Sprite icon, string text) {
        PopUpsToShow.Enqueue(new PopUpData() { Icon = icon, Text = text });
    }

    private void StartPopUp() {

        if (!QuickPopUpAllowed || PopUpsToShow.Count == 0) return;

        // Note: Text can be rich text
        if (!alreadyActive) {
            PositionLerper.StartLerping();
            AlphaLerper.StartReverseLerp();

            alreadyActive = true;

            var data = PopUpsToShow.Dequeue();
            Image.sprite = data.Icon;
            Text.text = data.Text;
        }
    }

    private void ProcessNext() {

        if (!QuickPopUpAllowed && alreadyActive) {
            PositionLerper.StartReverseLerp();
            AlphaLerper.StartLerping();
            alreadyActive = false;
            alphaLerpRunning = false;
            return;
        }

        if(!QuickPopUpAllowed)
            return;

        if(alphaLerpRunning) {
            PopUpData data = PopUpsToShow.Dequeue();
            Image.sprite = data.Icon;
            Text.text = data.Text;
        }

        if (PopUpsToShow.Count == 0) {
            PositionLerper.StartReverseLerp();
            AlphaLerper.StartLerping();
            alreadyActive = false;
            alphaLerpRunning = false;
            return;
        }

        alphaLerpRunning = true;
        AlphaLerper.StartLerping(() => {
            alphaLerpRunning = false;
            PopUpData data = PopUpsToShow.Dequeue();
            Image.sprite = data.Icon;
            Text.text = data.Text;

            AlphaLerper.StartReverseLerp();
        });
    }


    public void PopUpClicked() {
        ProcessNext();
    }
}
