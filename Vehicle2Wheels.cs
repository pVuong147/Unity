using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle2Wheels : Vehicle, IVisualWheel, IAutoPilot
{
    public WheelCollider frontWheel;
    public WheelCollider rearWheel;
    public Transform frontTyre;
    public Transform rearTyre;
    private float centerOfMassHeight = .1f;
    private float tiltAngle = 20.0f;
    private Vector3 tiltAngleZ;
    private Quaternion deltaRotation;

    private void Start()
    {
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, centerOfMassHeight, rb.centerOfMass.z);
    }

    private void FixedUpdate()
    {
        if (playerControl)
        {
            Accelerate();
            Turn();
            Brake();
        }
    }
    public override void Accelerate()
    {
        float motor = motorTorque * Input.GetAxis("Vertical");
        if (rb.velocity.z < maxSpeed)
            rearWheel.motorTorque = motor;
        else
            rearWheel.motorTorque = 0;
        ApplyVisualWheel(rearWheel);
    }
    public override void Turn()
    {
        float steering = steeringAngle * Input.GetAxis("Horizontal");
        frontWheel.steerAngle = steering;
        tiltAngleZ = new Vector3(0, 0, Input.GetAxis("Horizontal") * tiltAngle);
        deltaRotation = Quaternion.Euler(tiltAngleZ * Time.deltaTime);
        if (steering > 0)
        {
            tiltAngleZ.Set(0, 0, -tiltAngle);
            deltaRotation = Quaternion.Euler(tiltAngleZ * Time.deltaTime);
            if (rb.rotation.eulerAngles.z >= 350f || rb.rotation.eulerAngles.z <= 20)
                rb.MoveRotation(rb.rotation * deltaRotation);
        }
        else if (steering < 0)
        {
            tiltAngleZ.Set(0, 0, tiltAngle);
            deltaRotation = Quaternion.Euler(tiltAngleZ * Time.deltaTime);
            if (rb.rotation.eulerAngles.z <= 10f || rb.rotation.eulerAngles.z >= 340)
                rb.MoveRotation(rb.rotation * deltaRotation);
        }
        else
            rb.angularVelocity = Vector3.zero;
        ApplyVisualWheel(frontWheel);
    }
    public override void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            frontWheel.brakeTorque = brakeTorque;
            rearWheel.brakeTorque = brakeTorque;
            ApplyVisualWheel(frontWheel);
            ApplyVisualWheel(rearWheel);
        }
        else
        {
            frontWheel.brakeTorque = 0;
            rearWheel.brakeTorque = 0;
            ApplyVisualWheel(frontWheel);
            ApplyVisualWheel(rearWheel);
        }
    }
    public void ApplyVisualWheel(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
            return;

        Transform visualWheel = null;
        if (collider.tag == "FrontRim")
            visualWheel = frontTyre;
        else if (collider.tag == "BackRim")
            visualWheel = rearTyre;

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        if (visualWheel != null)
        {
            visualWheel.position = position;
            visualWheel.rotation = rotation;
        }
    }
    public void AutoAccelerate(float maxSpeed)
    {
        Debug.Log("AutoAccelerate() from " + gameObject.name);
    }
    public void AutoTurn(float steerAngle)
    {
        Debug.Log("AutoTurn() from " + gameObject.name);
    }
    public void AutoBrake(float maxBrakeTorque)
    {
        Debug.Log("AutoBrake() from " + gameObject.name);
    }
}
