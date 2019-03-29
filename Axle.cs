using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Axle : MonoBehaviour
{
    public Rigidbody rb;
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    private float travelL = 1.0f;
    private float travelR = 1.0f;
    private WheelHit hit;
    private bool groundedL;
    private bool groundedR;
    [SerializeField]
    private float antiRoll;
    private float antiRollForce;

    private void FixedUpdate()
    {
        groundedL = leftWheel.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;

        groundedR = rightWheel.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;

        antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
            rb.AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);
        if (groundedR)
            rb.AddForceAtPosition(rightWheel.transform.up * antiRollForce, rightWheel.transform.position);
    }
}

