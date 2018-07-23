using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBoardInteractable : InteractableObjectBaseClass
{
    private string missionName;
    private string textToDisplay;

    private OverlayController overlayController;
    private MissionController missionController;
    
    // Use this for initialization
    void Start () {
        overlayController = GameObjectDirectory.OverlayController;
        missionController = GameObjectDirectory.MissionController;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(string _textToDisplay, string _missionName)
    {
        textToDisplay = _textToDisplay;
        missionName = _missionName;
    }

    public string GetMissionName()
    {
        return missionName;
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
        Debug.Log("Interacting, trying to display mission with name " + missionName);
        Debug.Log("MissionController is " + missionController);
        // Show the full mission display
        missionController.DisplayMissionScreen(missionName);
        HideText();
    }
}
