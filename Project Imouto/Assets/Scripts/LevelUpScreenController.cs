using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScreenController : MonoBehaviour {

    [SerializeField]
    private Text currentLevelText;
    [SerializeField]
    private Text currentXP;

    private PlayerXPController playerXPController;
    private string currentLevelHeader = "Level: ";
    private string xpDivider = "/";

	// Use this for initialization
	void Start () {
        playerXPController = GameObjectDirectory.PlayerXPController;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenLevelUpScreen()
    {
        currentLevelText.text = currentLevelHeader + playerXPController.GetCurrentLevel();
        currentXP.text = playerXPController.GetNextLevelXP() + xpDivider + playerXPController.GetCurrentXP();
    }
}
