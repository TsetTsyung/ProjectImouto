using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScript : MonoBehaviour
{

    [SerializeField]
    private float checkDistance;

    private RaycastHit hitInfo;
    private OverlayController overlayController;
    private InteractableObjectBaseClass interactableObject;
    private bool attemptInteraction = false;

    private void Awake()
    {
        GameObjectDirectory.InteractionSystem = this;
    }

    // Use this for initialization
    void Start()
    {
        overlayController = GameObjectDirectory.OverlayController;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, checkDistance))
        {
            if (hitInfo.transform.CompareTag("Interactable"))
            {
                interactableObject = hitInfo.transform.GetComponent<InteractableObjectBaseClass>();
                interactableObject.DisplayText();
            }

            // See if the player is pressing 'use'
            if (interactableObject != null)
            {
                if (attemptInteraction)
                {
                    // Interact with the interactable object
                    interactableObject.Interact();

                    interactableObject.HideText();
                    interactableObject = null;
                    attemptInteraction = false;
                }
            }
        }
        // If we're no longer looking at an interactable object, hide the text
        else if (interactableObject != null)
        {
            interactableObject.HideText();
            interactableObject = null;
        }

        attemptInteraction = false; // reset the interaction flag no matter the result
    }

    public void AttemptInteraction()
    {
        attemptInteraction = true;
    }

    public void StopAttemptingInteraction()
    {
        attemptInteraction = false;
    }
}
