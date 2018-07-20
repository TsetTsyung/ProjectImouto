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
        Debug.Log("Adding XP");
        playerXP += newXP;

        if (playerXP >= currentMaxXP)
            UpdatePlayersLevel();

        playerProfileController.SetPlayerXP(playerXP);
        overlayController.UpdateXPBar(newXP);
    }

    private void UpdatePlayersLevel()
    {
        playerLevel++;
        if (playerLevel > levels.Length)
        {
            playerLevel = levels.Length;
        }

        playerProfileController.SetPlayerLevel(playerLevel);
        overlayController.SetNewMaxXP(levels[playerLevel - 1]);

        // Allow unlock of new ability
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
