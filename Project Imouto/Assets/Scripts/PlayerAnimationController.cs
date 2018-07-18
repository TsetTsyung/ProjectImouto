using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    [SerializeField]
    private Animator playerAnimator;

    private GameControllerScript gameController;
    private AnimatorClipInfo[] clipInfo;
    private Vector2 mouseInput;

    private bool isMovingOnX = false;
    private bool isMovingOnY = false;

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
            }
        }

        // Take in the rotational input from the mouse
        if (!gameController.GetPausedState())
        {
            mouseInput = Input.mousePosition;
        }
    }

    public void PlayEmote1()
    {
        playerAnimator.SetTrigger("Emote1");
    }

    public void PlayEmote2()
    {
        playerAnimator.SetTrigger("Emote2");
    }

    public void PlayEmote3()
    {
        playerAnimator.SetTrigger("Emote3");
    }

    public void PlayEmote4()
    {
        playerAnimator.SetTrigger("Emote4");
    }

    public void PlayEmote5()
    {
        playerAnimator.SetTrigger("Emote5");
    }

    public void PlayEmote6()
    {
        playerAnimator.SetTrigger("Emote6");
    }

    public void PlayEmote7()
    {
        playerAnimator.SetTrigger("Emote7");
    }

    public void SubmitVerticalInput(float verticalInput, bool isWalking)
    {
        if (verticalInput != 0f)
        {
            if (!isWalking)
            {
                verticalInput *= 2f;
            }
            playerAnimator.SetFloat("ForwardSpeed", verticalInput);
            isMovingOnY = true;
        }
        else
        {
            playerAnimator.SetFloat("ForwardSpeed", 0f);
            isMovingOnY = false;
        }
    }

    public void SubmitHorizontalInput(float horizontalInput, bool isWalking)
    {
        if (horizontalInput != 0f)
        {
            if (!isWalking)
            {
                horizontalInput *= 2f;
            }

            playerAnimator.SetFloat("SidewaysSpeed", horizontalInput);
            isMovingOnX = true;
        }
        else
        {
            playerAnimator.SetFloat("SidewaysSpeed", 0f);
            isMovingOnX = false;
        }
    }

    public void SubmitMouseRotationInput(float mouseXInput, bool isGrounded)
    {
        if (isGrounded && mouseXInput != 0f && !isMovingOnX && !isMovingOnY)
        {
            if (mouseXInput < 0f)
            {
                // Turning Left
                playerAnimator.SetBool("StationaryTurningLeft", true);
                playerAnimator.SetBool("StationaryTurningRight", false);
            }
            else
            {
                // Turning Right
                playerAnimator.SetBool("StationaryTurningLeft", false);
                playerAnimator.SetBool("StationaryTurningRight", true);
            }
        }
        else
        {
            playerAnimator.SetBool("StationaryTurningLeft", false);
            playerAnimator.SetBool("StationaryTurningRight", false);
        }
    }

    public void PlayAttack1()
    {
        playerAnimator.SetTrigger("Attack");
    }

    public void PlayEmote8()
    {
        playerAnimator.SetTrigger("Emote8");
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
}
