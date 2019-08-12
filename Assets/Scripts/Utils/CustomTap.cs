using CustomEvents;
using UnityEngine;
using UnityEngine.EventSystems;



public class CustomTap : MonoBehaviour, IPointerClickHandler {

    public GameEvent clickEvent;
    public GameEventItem itemEvent;


    public void OnPointerClick(PointerEventData pointerEventData) {
        if(clickEvent != null) {
            clickEvent.Raise();
        }
    }
}
