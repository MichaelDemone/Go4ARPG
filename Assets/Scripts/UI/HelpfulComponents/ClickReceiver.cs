using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickReceiver : MonoBehaviour, IPointerClickHandler {

	public UnityEventVector3 MouseClick3D;
	public UnityEventVector2 MouseClick2D;

	public void OnPointerClick(PointerEventData eventData) {
		Vector3 vec = Camera.main.ScreenToWorldPoint(eventData.position);
		vec.z = 0;
		MouseClick2D.Invoke(vec);
		MouseClick3D.Invoke(vec);
	}
}
