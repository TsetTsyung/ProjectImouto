using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileController : MonoBehaviour {

    private PlayerAttackScript playerAttackScript;
    private PlayerSaveProfile ourSaveProfile;
    private FileHandler fileHandler;


    private void Awake(){
        GameObjectDirectory.PlayerProfileController = this;
    }

    // Use this for initialization
    void Start () {
        fileHandler = GameObjectDirectory.FileHandler;
        playerAttackScript = GameObjectDirectory.PlayerAttackScript;

        ourSaveProfile = fileHandler.GetPlayerProfileFromFile();

        if (ourSaveProfile == null)
            ourSaveProfile = new PlayerSaveProfile();

        playerAttackScript.SubmitUnlockedMoves(ourSaveProfile.UnlockedHeavyAttack1, ourSaveProfile.UnlockedHeavyAttack2, ourSaveProfile.UnlockedSuperHeavyAttack);
	}

	// Update is called once per frame
	void Update () {
		
	}

    public int GetPlayerLevel()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.PlayerLevel;
        }
        return 0;
    }

    public int GetPlayerXP()
    {
        if(ourSaveProfile != null)
        {
            return ourSaveProfile.PlayerXP;
        }
        return 0;
    }

    public Vector3 GetPlayerSpawnLocation()
    {
        if(ourSaveProfile != null)
        {
            return ourSaveProfile.RespawnLocation;
        }
        return Vector3.zero;
    }

    public int GetPlayerMaxHealthLevel()
    {
        if(ourSaveProfile != null)
        {
            return ourSaveProfile.MaxHealth;
        }
        return 0;
    }

    public int GetPlayerMaxStaminaLevel()
    {
        if(ourSaveProfile != null)
        {
            return ourSaveProfile.MaxStamina;
        }
        return 0;
    }

    public int GetPlayerBonusDamageLevel()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.BonusDamage;
        }
        return 0;
    }

    public int GetPlayerSwordLevel()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.SwordLevel;
        }
        return 0;
    }

    public int GetPlayerShieldLevel()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.ShieldLevel;
        }
        return 0;
    }

    public int GetSkillPointsToAssign()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.SkillPointsToAssign;
        }
        return 0;
    }

    public bool GetHeavyAttack1Unlock()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.UnlockedHeavyAttack1;
        }
        return false;
    }

    public bool GetHeavyAttack2Unlock()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.UnlockedHeavyAttack2;
        }
        return false;
    }

    public bool GetSuperHeavyAttackUnlock()
    {
        if (ourSaveProfile != null)
        {
            return ourSaveProfile.UnlockedSuperHeavyAttack;
        }
        return false;
    }


    public void SetPlayerLevel(int newLevel)
    {
        if(ourSaveProfile != null)
        {
            ourSaveProfile.PlayerLevel = newLevel;
        }
    }//

    public void SetPlayerXP(int newXP)
    {
        if (ourSaveProfile != null)
        {
            ourSaveProfile.PlayerXP = newXP;
        }
    }//

    public void SetPlayerSpawnLocation(Vector3 newLocation)
    {
        if (ourSaveProfile != null)
        {
            ourSaveProfile.RespawnLocation = newLocation;
        }
    }

    public void SetPlayerMaxHealthLevel(int newMaxHealth)
    {
        if (ourSaveProfile != null && newMaxHealth > ourSaveProfile.MaxHealth)
        {
            ourSaveProfile.MaxHealth = newMaxHealth;
        }
    }

    public void SetPlayerMaxStaminaLevel(int newMaxStamina)
    {
        if (ourSaveProfile != null && newMaxStamina > ourSaveProfile.MaxStamina)
        {
            ourSaveProfile.MaxStamina= newMaxStamina;
        }
    }

    public void SetPlayerBonusDamageLevel(int newBonusDamage)
    {
        if (ourSaveProfile != null && newBonusDamage > ourSaveProfile.BonusDamage)
        {
            ourSaveProfile.BonusDamage = newBonusDamage;
        }
    }

    public void SetPlayerSwordLevel(int newSwordLevel)
    {
        if (ourSaveProfile != null && newSwordLevel > ourSaveProfile.SwordLevel)
        {
            ourSaveProfile.SwordLevel = newSwordLevel;
        }
    }

    public void SetPlayerShieldLevel(int newShieldLevel)
    {
        if (ourSaveProfile != null && newShieldLevel > ourSaveProfile.ShieldLevel)
        {
            ourSaveProfile.ShieldLevel = newShieldLevel;
        }
    }

    public void SetSkillPointsToAssign(int newSkillPointsToAssign)
    {
        if (ourSaveProfile != null)
        {
            ourSaveProfile.SkillPointsToAssign = newSkillPointsToAssign;
        }
    }

    public void SetHeavyAttack1Unlock(bool newUnlockState)
    {
        if(ourSaveProfile != null)
        {
            ourSaveProfile.UnlockedHeavyAttack1 = newUnlockState;
            playerAttackScript.SubmitUnlockedMoves(ourSaveProfile.UnlockedHeavyAttack1, ourSaveProfile.UnlockedHeavyAttack2, ourSaveProfile.UnlockedSuperHeavyAttack);
        }
    }

    public void SetHeavyAttack2Unlock(bool newUnlockState)
    {
        if (ourSaveProfile != null)
        {
            ourSaveProfile.UnlockedHeavyAttack2 = newUnlockState;
            playerAttackScript.SubmitUnlockedMoves(ourSaveProfile.UnlockedHeavyAttack1, ourSaveProfile.UnlockedHeavyAttack2, ourSaveProfile.UnlockedSuperHeavyAttack);
        }
    }

    public void SetSuperHeavyAttackUnlock(bool newUnlockState)
    {
        if (ourSaveProfile != null)
        {
            ourSaveProfile.UnlockedSuperHeavyAttack = newUnlockState;
            playerAttackScript.SubmitUnlockedMoves(ourSaveProfile.UnlockedHeavyAttack1, ourSaveProfile.UnlockedHeavyAttack2, ourSaveProfile.UnlockedSuperHeavyAttack);
        }
    }

    public void SkillPointSpent()
    {
       if(ourSaveProfile != null && ourSaveProfile.SkillPointsToAssign > 0)
        {
            ourSaveProfile.SkillPointsToAssign--;
        }
    }
}

// serializable class for saving the player's data
[Serializable]
public class PlayerSaveProfile {

    public int PlayerLevel { get; set; }
    public int PlayerXP { get; set; }
    public Vector3 RespawnLocation { get; set; }
    public int MaxHealth { get; set; }
    public int MaxStamina { get; set; }
    public int BonusDamage { get; set; }
    public int SwordLevel { get; set; }
    public int ShieldLevel { get; set; }
    public int SkillPointsToAssign { get; set; }
    public bool UnlockedHeavyAttack1 { get; set; }
    public bool UnlockedHeavyAttack2 { get; set; }
    public bool UnlockedSuperHeavyAttack { get; set; }

    public PlayerSaveProfile()
    {
        PlayerLevel = 0;
        PlayerXP = 0;
        //RespawnLocation = default start location
        MaxHealth = 50;
        MaxStamina = 50;
        BonusDamage = 0;
        SwordLevel = 1;
        ShieldLevel = 1;
        SkillPointsToAssign = 0;
        UnlockedHeavyAttack1 = false;
        UnlockedHeavyAttack2 = false;
        UnlockedSuperHeavyAttack = false;
    }
}