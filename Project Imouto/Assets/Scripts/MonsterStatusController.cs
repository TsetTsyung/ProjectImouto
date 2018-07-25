using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatusController : MonoBehaviour
{
    [SerializeField]
    private int XPValue;
    [SerializeField]
    private Collider mainCollider;

    private MonsterHealthScript monsterHealth;
    private GenericAnimationController animationScript;
    private PatrolingMobScript patrolScript;

    private void Awake()
    {
        monsterHealth = GetComponent<MonsterHealthScript>();
        animationScript = GetComponent<GenericAnimationController>();
        patrolScript = GetComponent<PatrolingMobScript>();
        mainCollider = GetComponent<Collider>();
    }

    public void EnableCreature()
    {
        monsterHealth.enabled = true;
        animationScript.enabled = true;
        patrolScript.enabled = true;

        if (mainCollider != null)
            mainCollider.enabled = true;
    }

    public void ActivateCreature()
    {
        monsterHealth.ActivateCreature();
        patrolScript.ActivateCreature();
    }

    public void DisableCreature()
    {
        animationScript.enabled = false;
        patrolScript.enabled = false;
    }

    public void DeactivateCreature()
    {
        monsterHealth.enabled = false;
        if (mainCollider != null)
            mainCollider.enabled = true;
        GameObjectDirectory.PlayerXPController.AddXP(XPValue);
        patrolScript.DeactivateCreature();
    }
}
