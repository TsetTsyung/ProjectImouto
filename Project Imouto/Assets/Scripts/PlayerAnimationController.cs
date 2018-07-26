using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator playerAnimator;

    private GameControllerScript gameController;
    private AnimatorClipInfo[] clipInfo;
    private Vector2 mouseInput;

    private bool isMovingOnX = false;
    private bool isMovingOnY = false;
    private bool stationary = false;
    private AnimatorStateInfo stateInfo;

    private void Awake()
    {
        GameObjectDirectory.PlayerAnimationController = this;
    }

    // Use this for initialization
    void Start()
    {
        gameController = GameObjectDirectory.GameController;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo[0].clip.name.Contains("Emote"))
            {
                playerAnimator.SetTrigger("StopEmoting");
                DeactivateRootMotion();
            }
        }

        // Take in the rotational input from the mouse
        if (!gameController.GetPausedState())
        {
            mouseInput = Input.mousePosition;
        }
    }

    public AnimatorStateInfo GetState()
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0);
    }

    public void PlayEmote1()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote1");
    }

    public void PlayEmote2()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote2");
    }

    public void PlayEmote3()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote3");
    }


    public void PlayEmote4()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote4");
    }

    public void PlayEmote5()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote5");
    }

    public void PlayEmote6()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote6");
    }

    public void PlayEmote7()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote7");
    }

    public void PlayEmote8()
    {
        ActivateRootMotion();
        playerAnimator.SetTrigger("Emote8");
    }

    public void SubmitInput(float verticalInput, float horizonatlInput, float mouseInput, bool isWalking)
    {
        //stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

        stationary = true;
        if (!isWalking)
        {
            verticalInput *= 2f;
            horizonatlInput *= 2f;
        }

        if (verticalInput != 0f || horizonatlInput != 0f)
            stationary = false;

        playerAnimator.SetFloat("ForwardSpeed", verticalInput);
        playerAnimator.SetFloat("SidewaysSpeed", horizonatlInput);
        playerAnimator.SetFloat("TurnInput", mouseInput);

        playerAnimator.SetBool("Stationary", stationary);
    }

    public void PlayerAttack(ComboMoves newMove)
    {
        playerAnimator.SetBool("Attacking", true);
        ActivateRootMotion();
        switch (newMove)
        {
            case ComboMoves.Unset:
                break;
            case ComboMoves.LightAttack:
                playerAnimator.SetTrigger("Attack");
                break;
            case ComboMoves.HeavyAttack1:
                playerAnimator.SetTrigger("HeavyAttack1");
                break;
            case ComboMoves.HeavyAttack2:
                playerAnimator.SetTrigger("HeavyAttack2");
                break;
            case ComboMoves.SuperHeavyAttack:
                playerAnimator.SetTrigger("SuperHeavyAttack");
                break;
            default:
                break;
        }
    }

    private void ActivateRootMotion()
    {
        Debug.Log("enabling root motion");
        playerAnimator.applyRootMotion = true;
    }

    public  void DeactivateRootMotion()
    {
        Debug.Log("deactivating root motion");
        playerAnimator.applyRootMotion = false;
    }

    public void PlayerDied()
    {
        Debug.LogWarning("Player Has Died");
        if (gameController.PlayerIsAlive)
        {
            playerAnimator.SetTrigger("Death" + UnityEngine.Random.Range(0, 2).ToString());
            gameController.PlayerDied();
        }
    }

    public void PlayStationaryJump()
    {

        playerAnimator.SetTrigger("StationaryJump");
    }


    public void PlayMovingJump()
    {
        playerAnimator.SetTrigger("MovingJump");
    }

    public void ClearAllAttackAnimations()
    {
        //Debug.Log("Clearing all animations");
        playerAnimator.SetBool("Attacking", false);
        playerAnimator.ResetTrigger("Attack");
        playerAnimator.ResetTrigger("HeavyAttack1");
        playerAnimator.ResetTrigger("HeavyAttack2");
        playerAnimator.ResetTrigger("SuperHeavyAttack");
        DeactivateRootMotion();
    }
}