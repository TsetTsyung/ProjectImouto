using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitScript : MonoBehaviour {

    [SerializeField]
    private PlayerAttackScript attackScript;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogWarning("We hit something, it's tag is " + other.tag);
        if (other.CompareTag("Monster"))
            attackScript.HitMonster(other.gameObject);
    }
}
