using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private float movementThresholdForJump = 0.1f;
    [SerializeField]
    private float airControllerMultiplier = 0.2f;
    [SerializeField]
    private Transform groundCheckStartPoint;
    [SerializeField]
    private float groundCheckDistance;

    private GameControllerScript gameController;
    private OverlayController overlayController;
    private PlayerMovementController playerMovementController;
    private PlayerAnimationController playerAnimationController;
    private PlayerAttackScript playerAttackScript;
    private InteractionScript interactionScript;
    private PlayerGearController playerGearController;

    private float verticalInput;
    private float horizontalInput;
    private float mouseXInput;
    private bool isWalking;
    private bool isGrounded;
    private bool inputAllowed = true;
    private bool movementAllowed = true;

    private RaycastHit hitInfo;

    private void Awake()
    {
        GameObjectDirectory.PlayerInputController = this;
    }

    // Use this for initialization
    void Start()
    {
        gameController = GameObjectDirectory.GameController;
        overlayController = GameObjectDirectory.OverlayController;
        playerMovementController = GameObjectDirectory.PlayerMovementController;
        playerAnimationController = GameObjectDirectory.PlayerAnimationController;
        playerAttackScript = GameObjectDirectory.PlayerAttackScript;
        interactionScript = GameObjectDirectory.InteractionSystem;
        playerGearController = GameObjectDirectory.PlayerGearController;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.PlayerIsAlive || !inputAllowed || gameController.GetPausedState())
            return;


        // Movement Handling
        if (movementAllowed)
        {
            verticalInput = Input.GetAxis("Vertical");
            horizontalInput = Input.GetAxis("Horizontal");
        }
        else
        {
            verticalInput = 0f;
            horizontalInput = 0f;
        }

        // Check for walk/Run modifier
        isWalking = (Input.GetButton("Walk")) ? true : false;

        // Check to see if the player is grounded
        DoGroundCheck();
        if (!isGrounded)

        {
            verticalInput *= airControllerMultiplier;
            horizontalInput *= airControllerMultiplier;
        }

        // Inform the movement script of the Forward/Backward input
        playerMovementController.SubmitVerticalInput(verticalInput, isWalking);

        // Inform the movement script of the Left/Right input
        playerMovementController.SubmitHorizontalInput(horizontalInput, isWalking);

        // Rotational Handling
        if (movementAllowed)
        {
            mouseXInput = Input.GetAxis("Mouse X");
        }
        else
        {
            mouseXInput = 0f;
        }
        playerMovementController.SubmitMouseRotationInput(mouseXInput);

        // Inform the animation script of the input
        playerAnimationController.SubmitInput(verticalInput, horizontalInput, mouseXInput, isWalking);

        if (Input.GetButtonDown("Jump") && isGrounded && movementAllowed)
        {
            Jump(verticalInput, horizontalInput);
        }

        // Check to see if the player is trying to interact with stuff
        if (Input.GetButtonDown("Use") && movementAllowed)
        {
            // see if we can activate stuff
            interactionScript.AttemptInteraction();
        }

        if (Input.GetButtonUp("Use") && movementAllowed)
        {
            interactionScript.StopAttemptingInteraction();
        }
        
        // Check for Emote input
        if (Input.GetButtonDown("Emote") && movementAllowed)
        {
            // display the emote window
            overlayController.DisplayEmotePanel();
        }
        else if (Input.GetButtonUp("Emote"))
        {
            overlayController.HideEmotePanel();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            // Contact the attack script
            playerAttackScript.PlayerHasPressedAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            // Contact the attack script
            playerAttackScript.PlayerHasPressedHeavyAttack();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            // Contact the attack script
            playerAttackScript.PlayerHasPressedSuperHeavyAttack();
        }

        if(Input.GetButtonDown("LevelUp"))
        {
            overlayController.DisplayLevelUpPanel();
        }

        if(Input.GetButtonDown("SmallBrew"))
        {
            playerGearController.UseSmallHealthBrew();
        }

        if(Input.GetButtonDown("LargeBrew"))
        {
            playerGearController.UseLargeHealthBrew();
        }

        if (Input.GetButtonDown("Save"))
        {
            // Save the game
            Debug.Log("Player pressed 'Save'");
            gameController.Save();
        }

        if(Input.GetButtonDown("Load"))
        {
            // Reload the scene
            gameController.Load();
        }
    }

    private void DoGroundCheck()
    {
        if (Physics.Raycast(groundCheckStartPoint.position, -Vector3.up, out hitInfo, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump(float verticalInput, float horizontalInput)
    {
        if (Mathf.Abs(verticalInput) <= movementThresholdForJump && Mathf.Abs(horizontalInput) <= movementThresholdForJump)
        {
            // Perform standing jump
            playerAnimationController.PlayStationaryJump();

        }
        else
        {
            // Perform moving jump
            playerAnimationController.PlayMovingJump();
        }
    }

    public void EnableInput()
    {
        inputAllowed = true;
    }

    public void DisableInput()
    {
        inputAllowed = false;
    }

    public void EnableMovementInput()
    {
        movementAllowed = true;
    }

    internal void DisableMovementInput()
    {
        movementAllowed = false;
    }
}
