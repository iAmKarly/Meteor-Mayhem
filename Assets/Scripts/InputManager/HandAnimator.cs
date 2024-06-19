using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimator : MonoBehaviour
{
    [Tooltip("The animator for left hand.")]
    [SerializeField] private Animator leftAnimator;
    [Tooltip("The animator for right hand.")]
    [SerializeField] private Animator rightAnimator;

    /// <summary>
    /// Show the shoot left hand animation.
    /// </summary>
    public void shootLeft(){
        leftAnimator.CrossFade("Shoot", 0.1f);

        leftAnimator.SetBool("isShoot", true);
        leftAnimator.SetBool("isIdle", false);
    }

    /// <summary>
    /// Show the shoot right hand animation.
    /// </summary>
    public void shootRight(){
        rightAnimator.CrossFade("Shoot", 0.1f);

        rightAnimator.SetBool("isShoot", true);
        rightAnimator.SetBool("isIdle", false);
    }

    /// <summary>
    /// Show the idle left hand animation.
    /// </summary>
    public void idleLeft(){
        leftAnimator.SetBool("isShoot", false);
        leftAnimator.SetBool("isIdle", true);
    }

    /// <summary>
    /// Show the idle right hand animation.
    /// </summary>
    public void idleRight(){
        rightAnimator.SetBool("isShoot", false);
        rightAnimator.SetBool("isIdle", true);
    }
}
