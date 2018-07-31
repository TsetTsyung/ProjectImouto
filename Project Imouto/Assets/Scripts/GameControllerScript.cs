using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    [SerializeField]
    private Vector3 startingPosition;

    private bool isPaused = false;
    private bool playerIsAlive = true;

    public bool PlayerIsAlive { get { return playerIsAlive; } }

    private void Awake()
    {
        GameObjectDirectory.GameController = this;
    }

    // Use this for initialization
    void Start () {
        // Load the game state from the save profile and set everything up.
        Invoke("SetupGame", 0.1f);
	}

    private void SetupGame()
    {
        Load();
        /*
        Debug.Log("Setting up game");
        GameObjectDirectory.PlayerProfileController.Load();

        if (!GameObjectDirectory.PlayerProfileController.NewGame())
        {
            Debug.Log("This is not a new game.");
            GameObjectDirectory.PlayerHealthController.transform.position = GameObjectDirectory.PlayerProfileController.GetPlayerSpawnLocation();
        }
        */
        ResumeGame();
    }

    // Update is called once per frame
    void Update () {

	}

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        ShowAndUnlockCursor();

    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        HideAndLockCursor();
    }

    public bool GetPausedState()
    {
        return isPaused;
    }

    public void HideAndLockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowAndUnlockCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;  // TODO: select the player's chosen lockmode
    }

    public void PlayerDied()
    {
        playerIsAlive = false;
    }

    internal void Save()
    {
        GameObjectDirectory.PlayerProfileController.SetPlayerSpawnLocation(GameObjectDirectory.PlayerHealthController.transform.position, GameObjectDirectory.PlayerHealthController.transform.rotation.eulerAngles.y);
        GameObjectDirectory.MissionController.SaveAllMissionStatus();
        GameObjectDirectory.PlayerProfileController.Save();
    }

    internal void Load()
    {
        GameObjectDirectory.PlayerProfileController.Load();
        // Sort out all of the position info

        if (!GameObjectDirectory.PlayerProfileController.NewGame())
        {
            Debug.Log("This is not a new game.");
            GameObjectDirectory.PlayerHealthController.transform.position = GameObjectDirectory.PlayerProfileController.GetPlayerSpawnLocation();
            GameObjectDirectory.PlayerHealthController.transform.Rotate(Vector3.up, GameObjectDirectory.PlayerProfileController.GetSpawnRotation());
        }
        else
        {
            Debug.Log("This is a new game");
            //GameObjectDirectory.PlayerHealthController.transform.position = startingPosition;
        } 

        GameObjectDirectory.MonsterSpawner.RedoAllMonsters();
        GameObjectDirectory.MissionController.ReloadMissions();
        GameObjectDirectory.PlayerStatsController.GetStats();
    }
}
