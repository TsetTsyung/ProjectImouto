using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour {

    private OverlayController overlayController;

    // Use this for initialization
    void Start () {
        overlayController = GameObjectDirectory.OverlayController;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Use"))
        {
            // see if we can activate stuff
        }

        if (Input.GetButtonDown("Emote"))
        {
            // display the emote window
            overlayController.DisplayEmotePanel();
        }

        if(Input.GetButtonUp("Emote"))
        {
            overlayController.HideEmotePanel();
        }
	}
}
