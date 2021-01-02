using System;
using System.Collections;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement Instance;


    [Header("Autoset References")] 
    [AutoSet(CheckChildrenForComponents = true)] public Animator animator;
    [AutoSet] public Rigidbody2D body;
    [AutoSet(SetByNameInChildren = true)] public SpriteRenderer View;
    [AutoSet] public Transform t;
    [AutoSet(CheckChildrenForComponents = true, SetByNameInChildren = true)] public Transform Weapon;

    [Header("Movement")]
    public float MovementForceStrength = 1f;

    public SimpleControls Inputs;

#if UNITY_EDITOR
    void Reset() {
        AutoSet.Init(this);
    }


#endif

    void OnEnable()
    {
        Inputs.Enable();
#if UNITY_EDITOR
        AutoSet.Init(this);
#endif
    }

    private void OnDisable()
    {
        Inputs.Disable();
    }

    void Awake() {
        Instance = this;
        Inputs = new SimpleControls();

        Inputs.gameplay.interact.performed += Interact_performed;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("interacted");
    }

    // Update is called once per frame
    void Update() {
        var move = Inputs.gameplay.move.ReadValue<Vector2>();
        transform.position += ((Vector3)move) * MovementForceStrength * Time.deltaTime;
    }
}
