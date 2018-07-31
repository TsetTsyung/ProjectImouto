using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreWeaponScript : InteractableObjectBaseClass
{

    [SerializeField]
    private Transform swordSpawnPoint;
    [SerializeField]
    private SelectedTransformRotator itemRotator;
    [SerializeField]
    private GameObject[] swordPrefabs;
    [SerializeField]
    private int[] swordCosts;
    [SerializeField]
    private string textToDisplay;

    private OverlayController overlayController;
    private PlayerStatsController playerStatsController;
    private PlayerTreasuryController playerTreasuryController;
    private GameObject sword;
    private int swordLevel;
    private int maxSwordLevel;

    // Use this for initialization
    void Start()
    {
        overlayController = GameObjectDirectory.OverlayController;
        playerStatsController = GameObjectDirectory.PlayerStatsController;
        playerTreasuryController = GameObjectDirectory.PlayerTreasuryController;
        allowedToDisplay = true;
        SetupStoreSwordItem();
    }

    private void SetupStoreSwordItem()
    {
        swordLevel = playerStatsController.GetPlayerSwordLevel();
        maxSwordLevel = playerStatsController.GetMaxSwordLevel();

        if (sword != null)
        {
            sword.transform.parent = null;
            Destroy(sword.gameObject);
        }

        if (swordLevel >= maxSwordLevel)
            this.gameObject.SetActive(false);

        sword = Instantiate(swordPrefabs[swordLevel], swordSpawnPoint.position, swordSpawnPoint.rotation, this.transform);

        itemRotator.AssignTransformToRotate(sword.transform);
    }

    public override void DisplayText()
    {
        if (!allowedToDisplay)
            return;
        overlayController.DisplayInteractionText(this.gameObject, textToDisplay);
    }

    public override void HideText()
    {
        overlayController.HideInteractionText(this.gameObject);
    }

    public override void Interact()
    {
        if (playerTreasuryController.SpendCoin(swordCosts[swordLevel]))
        {
            playerStatsController.UpdateSwordDamage(swordLevel + 1); // This is plus 1 to move the progress along
            SetupStoreSwordItem();
        }
    }

    public override void DisableText()
    {
        allowedToDisplay = false;
    }
}
