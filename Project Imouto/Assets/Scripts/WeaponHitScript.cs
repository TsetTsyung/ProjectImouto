using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitScript : MonoBehaviour {

    [SerializeField]
    private PlayerAttackScript attackScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
            attackScript.HitMonster(other.gameObject);
    }
}
