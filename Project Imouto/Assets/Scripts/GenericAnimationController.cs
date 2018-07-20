using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAnimationController : AnimationBaseClass
{

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float attackAnimationTime;
    [SerializeField]
    private float timeIntoAnimationForAttack;

    private bool animatorEnabled = true;

    public override void StartAttackAnimation()
    {
        if (animatorEnabled)
            animator.SetTrigger("Attacking");
    }

    public override void StartDeathAnimation()
    {
    }

    public override void StartIdlingAnimation()
    {
        if (animatorEnabled)
            animator.SetTrigger("Idling");
    }

    public override void StartRunAnimation()
    {
        if (animatorEnabled)
            animator.SetTrigger("Running");
    }

    public override void StartWalkAnimation()
    {
        if (animatorEnabled)
            animator.SetTrigger("Walking");
    }

    public override float GetAttackAnimationClipTime()
    {
        return attackAnimationTime;
    }

    public override float GetTimeIntoAnimationForAttack()
    {
        return timeIntoAnimationForAttack;
    }

    public void DeactivateAnimtor()
    {
        animator.enabled = false;
    }

    public void ActivateAnimator()
    {
        animator.enabled = true;
    }
}
