using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {

    [SerializeField]
    private Rigidbody ourRigidbody;
    [SerializeField]
    private float forwardMovementSpeed;
    [SerializeField]
    private float sidewaysMovementSpeed;
    [SerializeField]
    private float walkingSpeedModifier;
    [SerializeField]
    private float turnSpeed;

    private Vector3 moveDirection;
    private bool isWalking;
    private float mouseXInput;

    private void Awake()
    {
        GameObjectDirectory.PlayerMovementController = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SubmitVerticalInput(float verticalInput, bool walk)
    {
        moveDirection += transform.forward * verticalInput * forwardMovementSpeed;
        isWalking = walk;
    }

    public void SubmitHorizontalInput(float horizontalInput, bool walk)
    {
        moveDirection += transform.right * horizontalInput * sidewaysMovementSpeed;
        isWalking = walk;
    }

    internal void SubmitMouseRotationInput(float _mouseXInput)
    {
        mouseXInput = _mouseXInput;

        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * mouseXInput * turnSpeed);
        ourRigidbody.MoveRotation(ourRigidbody.rotation * deltaRotation);
    }

    private void LateUpdate()
    {
        if (isWalking)
            moveDirection *= walkingSpeedModifier;
        ourRigidbody.MovePosition(transform.position + moveDirection * Time.deltaTime);

        // reset the direction for the next frame
        moveDirection = Vector3.zero;
    }
}
