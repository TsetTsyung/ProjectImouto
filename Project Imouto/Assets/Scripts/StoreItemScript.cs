using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoreItemType { SmallHealthBrew, LargeHealthBrew}
public class StoreItemScript : InteractableObjectBaseClass{

    [SerializeField]
    private string textToDisplay;
    [SerializeField]
    private StoreItemType thisItemsType;

    private OverlayController overlayController;

    // Use this for initialization
    void Start()
    {
        overlayController = GameObjectDirectory.OverlayController;
    }

    public override void DisplayText()
    {
        overlayController.DisplayInteractionText(this.gameObject, textToDisplay);
    }

    public override void HideText()
    {
        overlayController.HideInteractionText(this.gameObject);
    }

    public override void Interact()
    {
        // Player has pressed the button, so do stuff.
    }
}
