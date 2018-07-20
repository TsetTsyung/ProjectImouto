using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

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
        SetupGame();

        ResumeGame();
	}

    private void SetupGame()
    {

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
}
