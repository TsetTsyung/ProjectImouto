using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGearController : MonoBehaviour
{

    [SerializeField]
    private int smallHealthBrewHealAmount;
    [SerializeField]
    private int largeHealthBrewHealAmount;
    [SerializeField]
    private GameObject[] shieldPrefabs;
    [SerializeField]
    private GameObject[] swordPrefabs;
    [SerializeField]
    private Transform shieldSpawnLatchPoint;
    [SerializeField]
    private Transform swordSpawnLatchPoint;

    private PlayerStatsController playerStatsController;
    private PlayerHealthController playerHealthController;
    private OverlayController overlayController;

    private GameObject sword;
    private GameObject shield;

    private int swordLevel;
    private int shieldLevel;

    private int smallHealthBrewAmount;
    private int largeHealthBrewAmount;

    private void Awake()
    {
        GameObjectDirectory.PlayerGearController = this;
    }


    // Use this for initialization
    void Start()
    {
        playerHealthController = GameObjectDirectory.PlayerHealthController;
        playerStatsController = GameObjectDirectory.PlayerStatsController;
        overlayController = GameObjectDirectory.OverlayController;
    }


    public void SetupCharacter(int newShieldLevel, int newSwordLevel, int newSmallBrew, int newLargeBrew)
    {
        shieldLevel = newShieldLevel;
        swordLevel = newSwordLevel;
        smallHealthBrewAmount = newSmallBrew;
        largeHealthBrewAmount = newLargeBrew;

        overlayController.UpdateSmallHealthBrewAmount(smallHealthBrewAmount);
        overlayController.UpdateLargeHealthBrewAmount(largeHealthBrewAmount);

        SetupSword();
        SetupShield();
    }

    private void SetupSword()
    {
        if (sword != null)
        {
            sword.transform.parent = null;
            Destroy(sword.gameObject);
        }

        sword = Instantiate(swordPrefabs[swordLevel], swordSpawnLatchPoint.position, swordSpawnLatchPoint.rotation, swordSpawnLatchPoint.transform);
    }

    private void SetupShield()
    {
        if (shield != null)
        {
            shield.transform.parent = null;
            Destroy(shield.gameObject);
        }

        if (shieldLevel > 0)
            shield = Instantiate(shieldPrefabs[shieldLevel - 1], shieldSpawnLatchPoint.position, shieldSpawnLatchPoint.rotation, shieldSpawnLatchPoint.transform);
    }

    public void UpdatePlayerSwordLevel(int newSwordLevel)
    {
        swordLevel = newSwordLevel;
        SetupSword();
    }

    public void UpdatePlayerShieldLevel(int newShieldLevel)
    {
        shieldLevel = newShieldLevel;
        SetupShield();
    }

    public void AddItem(StoreItemType thisItemsType)
    {
        switch (thisItemsType)
        {
            case StoreItemType.SmallHealthBrew:
                smallHealthBrewAmount++;
                overlayController.UpdateSmallHealthBrewAmount(smallHealthBrewAmount);
                break;
            case StoreItemType.LargeHealthBrew:
                largeHealthBrewAmount++;
                overlayController.UpdateLargeHealthBrewAmount(largeHealthBrewAmount);
                break;
            default:
                break;
        }
    }

    public void UseSmallHealthBrew()
    {
        if(smallHealthBrewAmount > 0)
        {
            smallHealthBrewAmount--;
            playerHealthController.PlayerHasIncreasedHealth(smallHealthBrewHealAmount);
            playerStatsController.UpdateSmallHealthBrewAmount(smallHealthBrewAmount);
            overlayController.UpdateSmallHealthBrewAmount(smallHealthBrewAmount);
        }
    }

    public void UseLargeHealthBrew()
    {
        if (largeHealthBrewAmount > 0)
        {
            largeHealthBrewAmount--;
            playerHealthController.PlayerHasIncreasedHealth(largeHealthBrewHealAmount);
            playerStatsController.UpdateLargeHealthBrewAmount(largeHealthBrewAmount);
            overlayController.UpdateLargeHealthBrewAmount(largeHealthBrewAmount);
        }
    }
}
