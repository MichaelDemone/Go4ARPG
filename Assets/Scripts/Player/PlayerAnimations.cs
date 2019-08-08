using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour {

    public Animator animator;
    public Animator armAnimator;
    public Animator armourAnimator;
    public Animator weaponAnimator;

    public UnityEvent StoppedWalking;
    public UnityEvent StartedWalking;
    public UnityEvent Spun;

    private Action celebrationFinished;

    // Use this for initialization
    void Awake () {
        animator = GetComponent<Animator>();
	}
	
    [ContextMenu("StartWalking")]
	public void StartWalking()
    {
        animator?.SetBool("Walking", true);
        //armAnimator?.SetBool("Walking", true);
        //armourAnimator?.SetBool("Walking", true);
        //weaponAnimator?.SetBool("Walking", true);
        StartedWalking.Invoke();

    }

    [ContextMenu("StopWalking")]
    public void StopWalking()
    {
        animator?.SetBool("Walking", false);
        //armAnimator?.SetBool("Walking", false);
        //armourAnimator?.SetBool("Walking", false);
        //weaponAnimator?.SetBool("Walking", false);
        StoppedWalking.Invoke();
    }

    [ContextMenu("Spin")]
    public void Spin() {
        spinDone = null;
        animator?.SetTrigger("Spin");
        //armAnimator?.SetTrigger("Spin");
        //armourAnimator?.SetTrigger("Spin");
        //weaponAnimator?.SetTrigger("Spin");
    }

    private Action spinDone;
    public void Spin(Action spinDone) {
        this.spinDone = spinDone;
        animator?.SetTrigger("Spin");
        //armAnimator?.SetTrigger("Spin");
        //armourAnimator?.SetTrigger("Spin");
        //weaponAnimator?.SetTrigger("Spin");
    }

    public void SpinDone()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        Spun.Invoke();
        spinDone?.Invoke();
    }

    [ContextMenu("Celebrate")]
    public void Celebrate()
    {
        celebrationFinished = null;
        animator?.SetTrigger("Celebrate");
        //armAnimator?.SetTrigger("Celebrate");
        //armourAnimator?.SetTrigger("Celebrate");
        //weaponAnimator?.SetTrigger("Celebrate");
    }

    public void Celebrate(Action onFinish) {
        animator?.SetTrigger("Celebrate");
        ////rmAnimator?.SetTrigger("Celebrate");
        //armourAnimator?.SetTrigger("Celebrate");
        //weaponAnimator?.SetTrigger("Celebrate");
        celebrationFinished = onFinish;
    }

    public void CelebrationDone() {
        celebrationFinished?.Invoke();
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
        animator?.SetTrigger("Attack");
        //armAnimator?.SetTrigger("Attack");
        //armourAnimator?.SetTrigger("Attack");
        weaponAnimator?.SetTrigger("Attack");
    }

    [ContextMenu("ResetAttack")]
    public void ResetAttack()
    {
        animator?.ResetTrigger("Attack");
        //armAnimator?.ResetTrigger("Attack");
        //armourAnimator?.ResetTrigger("Attack");
        weaponAnimator?.ResetTrigger("Attack");
    }
}
