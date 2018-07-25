using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour {

    [SerializeField]
    private int[] healthLevels;
    [SerializeField]
    private int[] staminaLevels;
    [SerializeField]
    private int[] bonusDamageLevels;

    private PlayerProfileController playerProfileController;
    private OverlayController overlayController;
    private PlayerAttackScript playerAttackScript;

    private int maxHealthPointer;
    private int maxStaminaPointer;
    private int bonusDamagePointer;

    private void Awake()
    {
        GameObjectDirectory.PlayerStatsController = this;
    }

    // Use this for initialization
    void Start () {
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        overlayController = GameObjectDirectory.OverlayController;
        playerAttackScript = GameObjectDirectory.PlayerAttackScript;

        maxHealthPointer = playerProfileController.GetPlayerMaxHealthLevel();
        maxStaminaPointer = playerProfileController.GetPlayerMaxStaminaLevel();
        bonusDamagePointer = playerProfileController.GetPlayerBonusDamageLevel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetPlayerMaxHealth()
    {
        return healthLevels[maxHealthPointer];
    }

    public int GetPlayerMaxStamina()
    {
        return staminaLevels[maxStaminaPointer];
    }

    public int GetPlayerBonusDamage()
    {
        return bonusDamageLevels[bonusDamagePointer];
    }

    public bool CheckIfMoreHealthUpgradesAvailable()
    {
        if (maxHealthPointer < (healthLevels.Length - 1))
            return true;
        else
            return false;
    }

    public bool CheckIfMoreStaminaUpgradesAvailable()
    {
        if (maxStaminaPointer < (staminaLevels.Length - 1))
            return true;
        else
            return false;
    }

    public bool CheckIfMoreDamageUpgradesAvailable()
    {
        if (bonusDamagePointer < (bonusDamageLevels.Length - 1))
            return true;
        else
            return false;
    }

    public bool UpgradeHealthLevels()
    {
        if (CheckIfMoreHealthUpgradesAvailable())
        {
            maxHealthPointer++;
            overlayController.SetNewMaxHealth(healthLevels[maxHealthPointer]);
            return true;
        }
        return false;
    }

    public bool UpgradeStaminaLevels()
    {
        if (CheckIfMoreStaminaUpgradesAvailable())
        {
            maxStaminaPointer++;
            overlayController.SetNewMaxStamina(staminaLevels[maxStaminaPointer]);
            return true;
        }
        return false;
    }

    public bool UpgradeBonusDamage()
    {
        if (CheckIfMoreDamageUpgradesAvailable())
        {
            bonusDamagePointer++;
            playerAttackScript.SubmitBonusDamage(bonusDamageLevels[bonusDamagePointer]);
            return true;
        }
        return false;
    }
}
