using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_WolfriderAnimationController : AnimationBaseClass {

    [SerializeField]
    private Animator orcAnimator;

    public override void StartAttackAnimation()
    {
        orcAnimator.SetTrigger("Attacking");
    }

    public override void StartDeathAnimation()
    {
    }

    public override void StartIdlingAnimation()
    {
        orcAnimator.SetTrigger("Idling");
    }

    public override void StartRunAnimation()
    {
        orcAnimator.SetTrigger("Running");
    }

    public override void StartWalkAnimation()
    {
        orcAnimator.SetTrigger("Walking");
    }

    public override float GetAttackAnimationClipTime()
    {
        return 1.33f;
    }

    public override float GetTimeIntoAnimationForAttack()
    {
        return 0.33f;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
