using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{

    private GameControllerScript gameController;
    private OverlayController overlayController;
    private PlayerMovementController playerMovementController;
    private PlayerAnimationController playerAnimationController;

    private float verticalInput;
    private float horizontalInput;
    private float mouseXInput;
    private bool isWalking;

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

        // Inform the movement and animation scripts of the Forward/Backward input
        playerMovementController.SubmitVerticalInput(verticalInput, isWalking);
        playerAnimationController.SubmitVerticalInput(verticalInput, isWalking);

        // Inform the movement script and animation scripts of the Left/Right input
        playerMovementController.SubmitHorizontalInput(horizontalInput, isWalking);
        playerAnimationController.SubmitHorizontalInput(horizontalInput, isWalking);


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
}
