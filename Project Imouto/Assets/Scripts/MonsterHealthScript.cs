using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealthScript : MonoBehaviour
{

    [SerializeField]
    private int startingHealth;

    private MonsterStatusController monsterStatusController;
    private GenericAnimationController animationControllerScript;

    private int currentHealth;

    private void Awake()
    {
        monsterStatusController = GetComponent<MonsterStatusController>();
        animationControllerScript = GetComponent<GenericAnimationController>();
    }

    // Use this for initialization
    void Start()
    {
        ActivateCreature();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateCreature()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damageTaken)
    {
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            CreatureDied();
        }
    }

    private void CreatureDied()
    {
        gameObject.SetActive(false);
        animationControllerScript.StartDeathAnimation();

        monsterStatusController.DeactivateCreature();
    }
}
