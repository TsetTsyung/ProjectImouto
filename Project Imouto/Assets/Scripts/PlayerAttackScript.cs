using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour {

    private PlayerAnimationController playerAnimationController;
    private Animator animator;

    private AnimatorClipInfo[] clipInfo;

    private void Awake()
    {
        GameObjectDirectory.PlayerAttackScript = this;
    }

    // Use this for initialization
    void Start () {
        playerAnimationController = GameObjectDirectory.PlayerAnimationController;
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerHasPressedAttack()
    {
        clipInfo = animator.GetCurrentAnimatorClipInfo(0);

    }
}
