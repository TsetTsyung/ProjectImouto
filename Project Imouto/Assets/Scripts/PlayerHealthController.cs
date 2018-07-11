using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour {

    [SerializeField]
    private int startingHealth;
    [SerializeField]
    private int startingMaxHealth;

    [SerializeField]
    private int startingStamina;
    [SerializeField]
    private int startingMaxStamina;


    private int health;
    private int maxHealth;
    private int stamina;
    private int maxStamina;

    private OverlayController overlayController;

    private void Awake()
    {
        GameObjectDirectory.PlayerHealthController = this;
    }

    // Use this for initialization
    void Start () {
        overlayController = GameObjectDirectory.OverlayController;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateUIBars();        
	}

    private void UpdateUIBars()
    {
        overlayController.UpdateHealthBar(health);
        overlayController.UpdateStaminaBar(stamina);
    }   

    public void StartNewGame()
    {
        maxHealth = startingMaxHealth;
        health = startingHealth;
        maxStamina = startingMaxStamina;
        stamina = startingStamina;

        UpdateUIBars();
    }

    public void InitialiseHealth(int newStartingHealth, int newStartingMaxHealth)
    {
        health = newStartingHealth;
        maxHealth = newStartingMaxHealth;
    }

    public void InitialiseStamina(int newStartingStamina, int newStartingMaxStamina)
    {
        stamina = newStartingStamina;
        maxStamina = newStartingStamina;
    }

    public void PlayerHasTakenDamage(int damageTaken)
    {
        health -= damageTaken;
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (health <= 0)
        {
            health = 0;
            // DIE!
        }
    }
}
