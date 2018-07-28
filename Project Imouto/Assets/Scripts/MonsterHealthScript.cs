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
    private RagDollDeathScript ragDollDeathScript;

    private int currentHealth;
    private bool isTargetCreature = false;
    private bool isDead = false;

    private void Awake()
    {
        monsterStatusController = GetComponent<MonsterStatusController>();
        animationControllerScript = GetComponent<GenericAnimationController>();
        ragDollDeathScript = GetComponent<RagDollDeathScript>();
    }

    // Use this for initialization
    void Start()
    {
        ragDollDeathScript.Initialise();
        ActivateCreature();
    }

    public void SetAsTargetCreature()
    {
        isTargetCreature = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateCreature()
    {
        currentHealth = startingHealth;
        animationControllerScript.ActivateAnimator();
        ragDollDeathScript.DisableRagdoll();
    }

    public void TakeDamage(int damageTaken)
    {
        if (isDead)
            return; 

        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            CreatureDied();
        }
    }

    private void CreatureDied()
    {
        isDead = true;
        animationControllerScript.DeactivateAnimtor();
        ragDollDeathScript.EnableRagDoll();

        if (isTargetCreature)
            GameObjectDirectory.MissionController.TargetCreatureDied(this);

        monsterStatusController.DeactivateCreature();
        Destroy(this.gameObject, 10f);
        GameObjectDirectory.MonsterSpawner.ThisCreatureDied(this.gameObject);
    }
}
