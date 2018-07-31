using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoreItemType { SmallHealthBrew, LargeHealthBrew}
public class StoreItemScript : InteractableObjectBaseClass{

    [SerializeField]
    private string textToDisplay;
    [SerializeField]
    private int cost;
    [SerializeField]
    private StoreItemType thisItemsType;

    private OverlayController overlayController;
    private PlayerTreasuryController playerTreasuryController;
    private PlayerGearController playerGearController;

    // Use this for initialization
    void Start()
    {
        overlayController = GameObjectDirectory.OverlayController;
        playerTreasuryController = GameObjectDirectory.PlayerTreasuryController;
        playerGearController = GameObjectDirectory.PlayerGearController;
        allowedToDisplay = true;
    }

    public override void DisplayText()
    {
        if (!allowedToDisplay)
            return;
        overlayController.DisplayInteractionText(this.gameObject, textToDisplay);
    }

    public override void HideText()
    {
        overlayController.HideInteractionText(this.gameObject);
    }

    public override void Interact()
    {
        // Player has pressed the button, so do stuff.
        if (playerTreasuryController.SpendCoin(cost))
        {
            playerGearController.AddItem(thisItemsType);
        }
    }

    public override void DisableText()
    {
        allowedToDisplay = false;
    }
}
