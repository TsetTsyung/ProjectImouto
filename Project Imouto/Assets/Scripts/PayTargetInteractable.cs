using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayTargetInteractable : InteractableObjectBaseClass {

    [SerializeField]
    private string header = "Press 'use' to pay ";
    [SerializeField]
    private string footer = " to the mission target.";

    private string missionName;
    private string textToDisplay;
    private OverlayController overlayController;
    private MissionController missionController;
    private PlayerGearController playerGearController;
    private PlayerTreasuryController playerTreasuryController;

    private int amountToPay;

    // Use this for initialization
    void Start () {
        overlayController = GameObjectDirectory.OverlayController;
        missionController = GameObjectDirectory.MissionController;
        playerTreasuryController = GameObjectDirectory.PlayerTreasuryController;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMissionParameters(string _missionName, int _amountToPay)
    {
        Debug.Log("Setting up the parameters");
        amountToPay = _amountToPay;
        SetText(header + amountToPay.ToString() + footer, _missionName);
    }

    public void SetText(string _textToDisplay, string _missionName)
    {
        missionName = _missionName;
        textToDisplay = _textToDisplay;
    }

    public override void DisplayText()
    {
        Debug.Log("Displaying the text");
        overlayController.DisplayInteractionText(this.gameObject, textToDisplay);
    }

    public override void HideText()
    {
        overlayController.HideInteractionText(this.gameObject);
    }

    public override void Interact()
    {
        Debug.Log("Interacting");
        // Show the full mission display
        if (playerTreasuryController.SpendCoin(amountToPay))
            missionController.PaidTarget(amountToPay);

        HideText();
    }
}
