using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatusController : MonoBehaviour {

    private MonsterHealthScript monsterHealth;
    private GenericAnimationController animationScript;
    private PatrolingMobScript patrolScript;
    private Collider mainCollider;

    private void Awake()
    {
        monsterHealth = GetComponent<MonsterHealthScript>();
        animationScript = GetComponent<GenericAnimationController>();
        patrolScript = GetComponent<PatrolingMobScript>();
        mainCollider = GetComponent<Collider>();
            }

    public void ActivateCreature()
    {
        monsterHealth.enabled = true;
        animationScript.enabled = true;
        patrolScript.enabled = true;

        if (mainCollider != null)
            mainCollider.enabled = true;

        monsterHealth.ActivateCreature();
        patrolScript.ActivateCreature();
    }

    public void DeactivateCreature()
    {
        monsterHealth.enabled = false;
        animationScript.enabled = false;
        patrolScript.enabled = false;
     

        if (mainCollider != null)
            mainCollider.enabled = true;
    }
}
