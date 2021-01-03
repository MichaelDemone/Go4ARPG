using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPickUp : MonoBehaviour
{
    public CircleCollider2D frontCollider;
    public GameObject currentlyColliding;
    public bool holdingObject;
    public PlayerMovement player;

    public SimpleControls Inputs;
    public float carryingMoveSpeed = 0.5f;
  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentlyColliding == null)
        {
            currentlyColliding = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (holdingObject == false)
        {
            currentlyColliding = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    private void Update()
    {
        if (holdingObject == true && currentlyColliding != null)
        {
            currentlyColliding.transform.position = frontCollider.transform.position;
        }
    }

    public void PickUp()
    {
        
        if (currentlyColliding == null)
        {
            return;
        }
        
        if(currentlyColliding.GetComponentInChildren<InteractableObject>() == null)
        {
            return;
        }

        int weight = currentlyColliding.GetComponentInChildren<InteractableObject>().weight; //?
;
        if (holdingObject == false && weight <= player.PlayerLiftWeight)
        {
            //pick up object
            holdingObject = true;
            Debug.Log("picked up");
            currentlyColliding.transform.position = frontCollider.transform.position;
            player.MovementForceStrength = player.MovementForceStrength * carryingMoveSpeed;
        }
        else if (holdingObject == true)
        {
            //drop object
            holdingObject = false;
            Debug.Log("dropped");
            player.MovementForceStrength = player.MovementForceStrength / carryingMoveSpeed;
        }

        

      
    }

}

  



