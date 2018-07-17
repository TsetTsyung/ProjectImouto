using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BodyPartOrigins
{
    public Transform parent;
    public Vector3 originalPosition;
    public Quaternion originalRotation;
}

public class SkeletonDeathScript : MonoBehaviour {

    [SerializeField]
    private Transform[] bodyParts;

    BodyPartOrigins[] bodyPartOrigins;

	// Use this for initialization
	void Start () {
        bodyPartOrigins = new BodyPartOrigins[bodyParts.Length];

        for (int i = 0; i < bodyPartOrigins.Length; i++)
        {
            bodyPartOrigins[i].parent = bodyParts[i].parent;
            bodyPartOrigins[i].originalPosition = bodyParts[i].position;
            bodyPartOrigins[i].originalRotation = bodyParts[i].rotation;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Disassemble()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].parent = null;
        }
    }

    public void Reassemble()
    {
        for (int i = 0; i < bodyPartOrigins.Length; i++)
        {
            bodyParts[i].parent = bodyPartOrigins[i].parent;
            bodyParts[i].position = bodyPartOrigins[i].originalPosition;
            bodyParts[i].rotation = bodyPartOrigins[i].originalRotation;
        }
    }
}
