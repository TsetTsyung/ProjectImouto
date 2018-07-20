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
    [SerializeField]
    private Text maxHealthText;
    [SerializeField]
    private Button IncreaseMaxHealthButton;
    [SerializeField]
    private Text maxStaminaText;
    [SerializeField]
    private Button IncreaseMaxStaminButton;
    [SerializeField]
    private Button UnlockHeavyAttack1Button;
    [SerializeField]
    private Button UnlockHeavyAttack2Button;
    [SerializeField]
    private Button UnlockSuperHeavyButton;
    [SerializeField]
    private Text SkillPointsAvailableText;

    private PlayerXPController playerXPController;
    private PlayerProfileController playerProfileController;
    private GameControllerScript gameController;
    private OverlayController overlayController;
    private string currentLevelHeader = "Level: ";
    private string xpDivider = "/";

	// Use this for initialization
	void Start () {
        playerXPController = GameObjectDirectory.PlayerXPController;
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        gameController = GameObjectDirectory.GameController;
        overlayController = GameObjectDirectory.OverlayController;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateLevelUpScreen()
    {
        currentLevelText.text = currentLevelHeader + playerXPController.GetCurrentLevel();
        currentXP.text = playerXPController.GetNextLevelXP() + xpDivider + playerXPController.GetCurrentXP();
        maxHealthText.text = playerProfileController.GetPlayerMaxHealth().ToString();
        //IncreaseMaxHealthButton.interactable = gameController.MoreHealthUpgradesLeft();
        maxStaminaText.text = playerProfileController.GetPlayerMaxStamina().ToString();
        //IncreaseMaxHealthButton.interactable = gameController.MoreStaminaUpgradesLeft();
        UnlockHeavyAttack1Button.interactable = !playerProfileController.GetHeavyAttack1Unlock();
        UnlockHeavyAttack2Button.interactable = !playerProfileController.GetHeavyAttack2Unlock();
        UnlockSuperHeavyButton.interactable = !playerProfileController.GetSuperHeavyAttackUnlock();
        SkillPointsAvailableText.text = playerProfileController.GetSkillPointsToAssign().ToString();
    }

    public void IncreaseMaxHealth()
    { }

    public void IncreaseMaxStamina()
    { }

    public void UnlockHeavyAttack1()
    { }

    public void UnlockHeavyAttack2()
    { }

    public void UnlockSuperHeavyAttack()
    { }

    public void Cancel()
    {
        overlayController.HideLevelUpPanel();
    }
}
