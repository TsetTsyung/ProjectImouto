using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AxisToRotateOn{ YAxis, XAxis, ZAxis}
public class SelectedTransformRotator : MonoBehaviour {

    [SerializeField]
    private Transform transformToRotate;
    [SerializeField]
    private AxisToRotateOn rotationAxis;
    [SerializeField]
    private float speed;

    private Vector3 axis;

    private void Start()
    {
        if (rotationAxis == AxisToRotateOn.XAxis)
        {
            axis = transform.right;
        }
        else if (rotationAxis == AxisToRotateOn.YAxis)
        {
            axis = transform.up;
        }
        else if (rotationAxis == AxisToRotateOn.ZAxis)
        {
            axis = transform.forward;
        }
    }

    // Update is called once per frame
    void Update () {
        transformToRotate.Rotate(axis, speed * Time.deltaTime, Space.World);
	}
}
