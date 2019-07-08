using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonText : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {

    public enum State {
        Up,
        Down,
        Disabled
    }

    [SerializeField] private State _state = State.Up;

    public State CurrentState {
        get { return _state; }
        set {
            _state = value;
            UpdateState();
        }
    }

    public UnityEvent OnClick;
    public Sprite DefaultSprite;
    public Sprite ClickedSprite;
    public Sprite DisabledSprite;


    public Vector2 TextShiftOnClick;

    private Image im;
    private TextMeshProUGUI myText;
    private Vector2 originalTextPos;

    // Use this for initialization
    void Awake() {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        im = GetComponentInChildren<Image>();
        originalTextPos = myText.rectTransform.anchoredPosition;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (CurrentState != State.Up) {
            return;
        }

        CurrentState = State.Down;
        StartCoroutine(click());
    }

    IEnumerator click() {
        yield return null;
        CurrentState = State.Up;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(CurrentState != State.Up) {
            return;
        }

        CurrentState = State.Down;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (CurrentState != State.Down) return;

        CurrentState = State.Up;
        OnClick.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(CurrentState != State.Down)return;

        CurrentState = State.Up;
    }

    private void UpdateState() {
        if (CurrentState == State.Up) {
            im.sprite = DefaultSprite;
            myText.rectTransform.anchoredPosition = originalTextPos;
        }
        else if (CurrentState == State.Disabled) {
            im.sprite = DisabledSprite;
            myText.rectTransform.anchoredPosition = originalTextPos;
        } else if (CurrentState == State.Down) {
            im.sprite = ClickedSprite;
            myText.rectTransform.anchoredPosition = originalTextPos + TextShiftOnClick;
        }
    }
}
