using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAnimationController : AnimationBaseClass {

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float attackAnimationTime;
    [SerializeField]
    private float timeIntoAnimationForAttack;

    public override void StartAttackAnimation()
    {
        animator.SetTrigger("Attacking");
    }

    public override void StartDeathAnimation()
    {
    }

    public override void StartIdlingAnimation()
    {
        animator.SetTrigger("Idling");
    }

    public override void StartRunAnimation()
    {
        Debug.Log("Trying to start run Animation");
        animator.SetTrigger("Running");
    }

    public override void StartWalkAnimation()
    {
        Debug.Log("Trying to start Walking animation");
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

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
