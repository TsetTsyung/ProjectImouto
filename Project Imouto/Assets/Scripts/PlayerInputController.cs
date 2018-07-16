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

    private float verticalInput;
    private float horizontalInput;
    private float mouseXInput;
    private bool isWalking;
    private bool isGrounded;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.PlayerIsAlive)
            return;

        // Rotational Handling
        mouseXInput = Input.GetAxis("Mouse X");
        playerMovementController.SubmitMouseRotationInput(mouseXInput);

        // Movement Handling
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Check for walk/Run modifier
        isWalking = (Input.GetButton("Walk")) ? true : false;

        // Check to see if the player is grounded
        DoGroundCheck();
        if (!isGrounded)

        {
            verticalInput *= airControllerMultiplier;
            horizontalInput *= airControllerMultiplier;
        }

        // Inform the movement and animation scripts of the Forward/Backward input
        playerMovementController.SubmitVerticalInput(verticalInput, isWalking);
        playerAnimationController.SubmitVerticalInput(verticalInput, isWalking);

        // Inform the movement script and animation scripts of the Left/Right input
        playerMovementController.SubmitHorizontalInput(horizontalInput, isWalking);
        playerAnimationController.SubmitHorizontalInput(horizontalInput, isWalking);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump(verticalInput, horizontalInput);
        }

        // Check to see if the player is trying to interact with stuff
        if (Input.GetButtonDown("Use"))
        {
            // see if we can activate stuff
        }

        // Check for Emote input
        if (Input.GetButtonDown("Emote"))
        {
            // display the emote window
            overlayController.DisplayEmotePanel();
        }
        else if (Input.GetButtonUp("Emote"))
        {
            overlayController.HideEmotePanel();
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
        Debug.Log("Jumping");
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
}
