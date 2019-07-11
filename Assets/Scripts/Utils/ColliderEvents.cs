using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvents : MonoBehaviour {

    public Action<Collider2D> TriggerEntered2D;
    public Action<Collider2D> TriggerStayed2D;
    public Action<Collider2D> TriggerExited2D;

    private void OnTriggerEnter2D(Collider2D collision) => TriggerEntered2D?.Invoke(collision);
    private void OnTriggerExit2D(Collider2D collision) => TriggerExited2D?.Invoke(collision);
    private void OnTriggerStay2D(Collider2D collision) => TriggerStayed2D?.Invoke(collision);
    

}
