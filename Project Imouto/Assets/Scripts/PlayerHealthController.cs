using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {

    [SerializeField]
    private PlayerAnimationController playerAnimationController;
    [SerializeField]
    private int startingHealth;
    [SerializeField]
    private int startingMaxHealth;
    [SerializeField]
    private int startingStamina;
    [SerializeField]
    private int startingMaxStamina;
    [SerializeField]
    private float timeToNextStaminaRecharge;
    [SerializeField]
    private int staminaRechargeAmount;

    private float staminaRechargeTimer;

    private int health;
    private int maxHealth;
    private int stamina;
    private int maxStamina;
    private int shieldProtection;

    private OverlayController overlayController;
    private PlayerProfileController playerProfileController;
    private PlayerStatsController playerStatsController;

    private void Awake()
    {
        GameObjectDirectory.PlayerHealthController = this;
    }

    // Use this for initialization
    void Start () {
        overlayController = GameObjectDirectory.OverlayController;
        playerProfileController = GameObjectDirectory.PlayerProfileController;
        playerStatsController = GameObjectDirectory.PlayerStatsController;
        StartNewGame();
    }
	
	// Update is called once per frame
	void Update () {
        if(stamina < maxStamina)
        {
            staminaRechargeTimer -= Time.deltaTime;

            if (staminaRechargeTimer < 0f)
            {
                staminaRechargeTimer = timeToNextStaminaRecharge;
                PlayerHasIncreasedStamina(staminaRechargeAmount);
            }
        }
	}


    private void UpdateUIBars()
    {
        overlayController.UpdateHealthBar(health);
        overlayController.UpdateStaminaBar(stamina);
    }   

    public void StartNewGame()
    {
        maxHealth = playerStatsController.GetPlayerMaxHealth();
        health = startingHealth;
        maxStamina = playerStatsController.GetPlayerMaxStamina();
        stamina = startingStamina;

        UpdateUIBars();
    }

    public void InitialiseHealth(int newStartingHealth, int newStartingMaxHealth)
    {
        health = newStartingHealth;
        maxHealth = newStartingMaxHealth;
        UpdateUIBars();
    }

    public void InitialiseStamina(int newStartingStamina, int newStartingMaxStamina)
    {
        stamina = newStartingStamina;
        maxStamina = newStartingStamina;
        UpdateUIBars();
    }

    public void UpdateSwordProtection(int newShieldProtection)
    {
        shieldProtection = newShieldProtection;
    }

    public void PlayerHasTakenDamage(int damageTaken)
    {
        damageTaken -= shieldProtection;
        health -= damageTaken;
        UpdateUIBars();
        CheckForDeath();
    }

    public bool PlayerHasUsedStamina(int staminaUsed)
    {
        if (stamina >= staminaUsed)
        {
            stamina -= staminaUsed;
            UpdateUIBars();
            staminaRechargeTimer = timeToNextStaminaRecharge;
            return true;
        }

        return false;
    }

    public void PlayerHasIncreasedHealth(int healthIncrease)
    {
        health += healthIncrease;
        if (health > maxHealth)
            health = maxHealth;
        UpdateUIBars();
    }

    public void PlayerHasIncreasedStamina(int staminaIncrease)
    {
        stamina += staminaIncrease;
        UpdateUIBars();
    }

    public void PlayerHasIncreasedMaxHealth(int maxHealthIncrease)
    {
        maxHealth += maxHealthIncrease;
        playerProfileController.SetPlayerMaxHealthLevel(maxHealth);
        UpdateUIBars();
    }

    public void PlayerHasIncreasedMaxStamina(int maxStaminaIncrease)
    {
        maxStamina += maxStaminaIncrease;
        playerProfileController.SetPlayerMaxStaminaLevel(maxStamina);
        UpdateUIBars();
    }

    private void CheckForDeath()
    {
        if (health <= 0)
        {
            health = 0;
            // DIE!
            playerAnimationController.PlayerDied();
        }
    }
}
