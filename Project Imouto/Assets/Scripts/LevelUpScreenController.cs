using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScreenController : MonoBehaviour
{

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
    private Button IncreaseMaxStaminaButton;
    [SerializeField]
    private Text bonusDamageText;
    [SerializeField]
    private Button IncreaseBonusDamageButton;
    [SerializeField]
    private Button UnlockHeavyAttack1Button;
    [SerializeField]
    private Button UnlockHeavyAttack2Button;
    [SerializeField]
    private Button UnlockSuperHeavyButton;
    [SerializeField]
    private Text skillPointsAvailableText;

    private PlayerXPController playerXPController;
    private PlayerProfileController playerProfileController;
    private PlayerStatsController playerStatsController;
    private GameControllerScript gameController;
    private OverlayController overlayController;
    private string currentLevelHeader = "Level: ";
    private string xpDivider = "/";

    // Use this for initialization
    void Start()
    {
        playerXPController = GameObjectDirectory.PlayerXPController;
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        playerStatsController = GameObjectDirectory.PlayerStatsController;
        gameController = GameObjectDirectory.GameController;
        overlayController = GameObjectDirectory.OverlayController;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateLevelUpScreen()
    {
        currentLevelText.text = currentLevelHeader + playerXPController.GetCurrentLevel();
        currentXP.text = playerXPController.GetNextLevelXP() + xpDivider + playerXPController.GetCurrentXP();
        maxHealthText.text = playerStatsController.GetPlayerMaxHealth().ToString();
        IncreaseMaxHealthButton.interactable = playerStatsController.CheckIfMoreHealthUpgradesAvailable();
        maxStaminaText.text = playerStatsController.GetPlayerMaxStamina().ToString();
        IncreaseMaxStaminaButton.interactable = playerStatsController.CheckIfMoreStaminaUpgradesAvailable();
        bonusDamageText.text = playerStatsController.GetPlayerBonusDamage().ToString();
        IncreaseBonusDamageButton.interactable = playerStatsController.CheckIfMoreDamageUpgradesAvailable();
        UnlockHeavyAttack1Button.interactable = !playerProfileController.GetHeavyAttack1Unlock();
        UnlockHeavyAttack2Button.interactable = !playerProfileController.GetHeavyAttack2Unlock();
        UnlockSuperHeavyButton.interactable = !playerProfileController.GetSuperHeavyAttackUnlock();
        skillPointsAvailableText.text = playerProfileController.GetSkillPointsToAssign().ToString();
    }

    public void IncreaseMaxHealth()
    {
        if (playerProfileController.GetSkillPointsToAssign() > 0)
        {
            if (playerStatsController.UpgradeHealthLevels())
            {
                playerProfileController.SkillPointSpent();
                skillPointsAvailableText.text = playerProfileController.GetSkillPointsToAssign().ToString();
                IncreaseMaxHealthButton.interactable = playerStatsController.CheckIfMoreHealthUpgradesAvailable();
                maxHealthText.text = playerStatsController.GetPlayerMaxHealth().ToString();
            }
        }
    }

    public void IncreaseMaxStamina()
    {
        if (playerProfileController.GetSkillPointsToAssign() > 0)
        {
            if (playerStatsController.UpgradeStaminaLevels())
            {
                playerProfileController.SkillPointSpent();
                skillPointsAvailableText.text = playerProfileController.GetSkillPointsToAssign().ToString();
                IncreaseMaxStaminaButton.interactable = playerStatsController.CheckIfMoreStaminaUpgradesAvailable();
                maxStaminaText.text = playerStatsController.GetPlayerMaxStamina().ToString();
            }
        }
    }

    public void IncreaseBonusDamage()
    {
        if (playerProfileController.GetSkillPointsToAssign() > 0)
        {
            if (playerStatsController.UpgradeBonusDamage())
            {
                playerProfileController.SkillPointSpent();
                skillPointsAvailableText.text = playerProfileController.GetSkillPointsToAssign().ToString();
                IncreaseBonusDamageButton.interactable = playerStatsController.CheckIfMoreDamageUpgradesAvailable();
                bonusDamageText.text = playerStatsController.GetPlayerBonusDamage().ToString();
            }
        }
    }

    public void UnlockHeavyAttack1()
    {
        if (playerProfileController.GetSkillPointsToAssign() > 0)
        {
            if (!playerProfileController.GetHeavyAttack1Unlock())
            {
                playerProfileController.SetHeavyAttack1Unlock(true);
                playerProfileController.SkillPointSpent();
                UnlockHeavyAttack1Button.interactable = !playerProfileController.GetHeavyAttack1Unlock();
            }
        }
    }

    public void UnlockHeavyAttack2()
    {
        if (playerProfileController.GetSkillPointsToAssign() > 0)
        {
            if (!playerProfileController.GetHeavyAttack2Unlock())
            {
                playerProfileController.SetHeavyAttack2Unlock(true);
                playerProfileController.SkillPointSpent();
                UnlockHeavyAttack2Button.interactable = !playerProfileController.GetHeavyAttack2Unlock();
            }
        }
    }

    public void UnlockSuperHeavyAttack()
    {
        if (playerProfileController.GetSkillPointsToAssign() > 0)
        {
            if (!playerProfileController.GetSuperHeavyAttackUnlock())
            {
                playerProfileController.SetSuperHeavyAttackUnlock(true);
                playerProfileController.SkillPointSpent();
                UnlockSuperHeavyButton.interactable = !playerProfileController.GetSuperHeavyAttackUnlock();
            }
        }
    }

    public void Cancel()
    {
        overlayController.HideLevelUpPanel();
    }
}
