using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{

    [SerializeField]
    private int[] healthLevels;
    [SerializeField]
    private int[] staminaLevels;
    [SerializeField]
    private int[] bonusDamageLevels;
    [SerializeField]
    private int[] swordDamageLevels;
    [SerializeField]
    private int[] shieldProtectionLevels;
    [SerializeField]
    private int maxSwordLevel;
    [SerializeField]
    private int maxShieldLevel;

    private PlayerProfileController playerProfileController;
    private OverlayController overlayController;
    private PlayerAttackScript playerAttackScript;
    private PlayerGearController playerGearController;
    private PlayerHealthController playerHealthController;

    private int maxHealthPointer;
    private int maxStaminaPointer;
    private int bonusDamagePointer;
    private int shieldLevel;
    private int swordLevel;

    private void Awake()
    {
        GameObjectDirectory.PlayerStatsController = this;
    }


    // Use this for initialization
    void Start()
    {
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        overlayController = GameObjectDirectory.OverlayController;
        playerAttackScript = GameObjectDirectory.PlayerAttackScript;
        playerGearController = GameObjectDirectory.PlayerGearController;
        playerHealthController = GameObjectDirectory.PlayerHealthController;

        GetStats();
    }

    public void GetStats()
    {
        maxHealthPointer = playerProfileController.GetPlayerMaxHealthLevel();
        maxStaminaPointer = playerProfileController.GetPlayerMaxStaminaLevel();
        bonusDamagePointer = playerProfileController.GetPlayerBonusDamageLevel();
        shieldLevel = playerProfileController.GetPlayerShieldLevel();
        swordLevel = playerProfileController.GetPlayerSwordLevel();

        // Setup the gear controller
        playerGearController.SetupCharacter(shieldLevel, swordLevel, playerProfileController.GetSmallHealthBrewAmount(), playerProfileController.GetLargeHealthBrewAmount());
    }

    public int GetMaxSwordLevel()
    {
        return maxSwordLevel;
    }

    public int GetMaxShieldLevel()
    {
        return maxShieldLevel;
    }

    // Update is called once per frame
    void Update()
    {

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


    public int GetPlayerShieldLevel()
    {
        return shieldLevel;
    }

    public int GetPlayerSwordLevel()
    {
        return swordLevel;
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

    public void UpdateSwordDamage(int newSwordLevel)
    {
        swordLevel = newSwordLevel;
        playerProfileController.SetPlayerSwordLevel(swordLevel);
        playerGearController.UpdatePlayerSwordLevel(swordLevel);
        playerAttackScript.UpdateSwordDamage(swordDamageLevels[swordLevel]);
    }

    public void UpdateShieldProtection(int newShieldLevel)
    {
        shieldLevel = newShieldLevel;
        playerProfileController.SetPlayerShieldLevel(shieldLevel);
        playerGearController.UpdatePlayerShieldLevel(shieldLevel);
        playerHealthController.UpdateSwordProtection(shieldProtectionLevels[shieldLevel]);
    }

    public void UpdateSmallHealthBrewAmount(int newAmount)
    {
        playerProfileController.UpdateSmallHealthBrewAmount(newAmount);
    }

    public void UpdateLargeHealthBrewAmount(int newAmount)
    {
        playerProfileController.UpdateLargeHealthBrewAmount(newAmount);
    }
}
