using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTreasuryController : MonoBehaviour {

    [SerializeField]
    private int startingCoin;

    private PlayerProfileController playerProfileController;
    private OverlayController overlayController;

    private int currentCoin;

    private void Awake()
    {
        GameObjectDirectory.PlayerTreasuryController = this;
    }

    private void Start()
    {
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        overlayController = GameObjectDirectory.OverlayController;

        SetCoin(playerProfileController.GetCoinAmount());
        overlayController.UpdateCoinAmount(currentCoin);
    }

    public void SetCoin(int newCurrentCoin)
    {
        currentCoin = newCurrentCoin;
        overlayController.UpdateCoinAmount(currentCoin);
    }

    public void AddCoin(int coinToAdd)
    {
        currentCoin += coinToAdd;
        overlayController.UpdateCoinAmount(currentCoin);
    }

    public int GetCoinAmount()
    {
        return currentCoin;
    }

    public bool SpendCoin(int coinSpent)
    {
        if (currentCoin >= coinSpent)
        {
            currentCoin -= coinSpent;
            playerProfileController.SetCoinAmount(currentCoin);
            overlayController.UpdateCoinAmount(currentCoin);
            return true;
        }

        return false;
    }
}
