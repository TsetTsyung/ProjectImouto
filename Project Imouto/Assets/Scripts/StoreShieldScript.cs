using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreShieldScript : InteractableObjectBaseClass
{

    [SerializeField]
    private Transform shieldSpawnPoint;
    [SerializeField]
    private SelectedTransformRotator itemRotator;
    [SerializeField]
    private GameObject[] shieldPrefabs;
    [SerializeField]
    private int[] shieldCosts;
    [SerializeField]
    private string textToDisplay;

    private OverlayController overlayController;
    private PlayerStatsController playerStatsController;
    private PlayerTreasuryController playerTreasuryController;
    private GameObject shield;
    private int shieldLevel;
    private int maxShieldLevel;

    // Use this for initialization
    void Start()
    {
        overlayController = GameObjectDirectory.OverlayController;
        playerStatsController = GameObjectDirectory.PlayerStatsController;
        playerTreasuryController = GameObjectDirectory.PlayerTreasuryController;
        SetupStoreShieldItem();
    }

    private void SetupStoreShieldItem()
    {
        shieldLevel = playerStatsController.GetPlayerShieldLevel();
        maxShieldLevel = playerStatsController.GetMaxShieldLevel();

        if (shield != null)
        {
            shield.transform.parent = null;
            Destroy(shield.gameObject);
        }

        if (shieldLevel >= maxShieldLevel)
            this.gameObject.SetActive(false);

        shield = Instantiate(shieldPrefabs[shieldLevel], shieldSpawnPoint.position, shieldSpawnPoint.rotation, this.transform);

        itemRotator.AssignTransformToRotate(shield.transform);
    }

    public override void DisplayText()
    {
        overlayController.DisplayInteractionText(this.gameObject, textToDisplay);
    }

    public override void HideText()
    {
        overlayController.HideInteractionText(this.gameObject);
    }

    public override void Interact()
    {
        if (playerTreasuryController.SpendCoin(shieldCosts[shieldLevel]))
        {
            playerStatsController.UpdateShieldProtection(shieldLevel + 1); // This is plus 1 to move the progress along
        SetupStoreShieldItem();
        }
    }
}
