using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollDeathScript : MonoBehaviour {

    private Rigidbody[] bodyParts;

    public void Initialise()
    {
        bodyParts = GetComponentsInChildren<Rigidbody>();
    }

    public void EnableRagDoll()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].isKinematic = false;
        }
    }

    public void DisableRagdoll()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].isKinematic = true;
        }
    }
}
