using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Data.DropSystem;



public class CustomTap : MonoBehaviour, IPointerClickHandler {

    public GameEvent clickEvent;
    public GameEventItem itemEvent;
    public Player player;


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (clickEvent != null)
            clickEvent.Raise();
    }
}
