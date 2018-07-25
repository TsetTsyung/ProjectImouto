using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXPController : MonoBehaviour {

    [SerializeField]
    private int[] levels;

    private OverlayController overlayController;
    private PlayerProfileController playerProfileController;
    private int playerXP;
    private int currentMaxXP;
    private int playerLevel;

    private void Awake()
    {
        GameObjectDirectory.PlayerXPController = this;
    }

    // Use this for initialization
    void Start () {
        overlayController = GameObjectDirectory.OverlayController;
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        StartNewGame();
	}

    private void StartNewGame()
    {
        playerXP = 0;
        playerLevel = 0;
        currentMaxXP = levels[playerLevel];
        UpdateXPBars();
    }

    private void UpdateXPBars()
    {
        overlayController.SetNewMaxXP(currentMaxXP);
        overlayController.UpdateXPBar(playerXP);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void AddXP(int newXP)
    {
        playerXP += newXP;

        while (playerXP >= currentMaxXP && playerLevel < levels.Length)
            UpdatePlayersLevel();

        playerProfileController.SetPlayerXP(playerXP);
        overlayController.UpdateXPBar(playerXP);
    }

    private void UpdatePlayersLevel()
    {
        playerLevel++;
        currentMaxXP = levels[playerLevel];
        if (playerLevel > levels.Length)
        {
            playerLevel = levels.Length;
        }

        playerProfileController.SetPlayerLevel(playerLevel);
        playerProfileController.SetSkillPointsToAssign(playerProfileController.GetSkillPointsToAssign()+1);
        overlayController.SetNewMaxXP(levels[playerLevel]);
    }

    public int GetCurrentXP()
    {
        return playerXP;
    }

    public int GetCurrentLevel()
    {
        return playerLevel;
    }

    public int GetNextLevelXP()
    {
        return levels[playerLevel];
    }
}
