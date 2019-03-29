using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class Vehicle4andMoreWheels : Vehicle, IVisualWheel, IAutoPilot
{
    public List<Axle> axles;
    private float autoAcceleration = 1.0f;

    public void FixedUpdate()
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
        {
            foreach (Axle axle in axles)
            {
                if (axle.motor)
                {
                    axle.leftWheel.motorTorque = motor;
                    axle.rightWheel.motorTorque = motor;
                    ApplyVisualWheel(axle.leftWheel);
                    ApplyVisualWheel(axle.rightWheel);
                }
            }
        }
        else
        {
            foreach (Axle axle in axles)
            {
                if (axle.motor)
                {
                    axle.leftWheel.motorTorque = 0;
                    axle.rightWheel.motorTorque = 0;
                    ApplyVisualWheel(axle.leftWheel);
                    ApplyVisualWheel(axle.rightWheel);
                }
            }
        }
    }
    public override void Turn()
    {
        float steering = steeringAngle * Input.GetAxis("Horizontal");
        
        foreach (Axle axle in axles)
        {
            if (axle.steering)
            {
                axle.leftWheel.steerAngle = steering;
                axle.rightWheel.steerAngle = steering;
                ApplyVisualWheel(axle.leftWheel);
                ApplyVisualWheel(axle.rightWheel);
            }
        }
    }
    public override void Brake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (Axle axle in axles)
            {
                axle.leftWheel.brakeTorque = brakeTorque;
                axle.rightWheel.brakeTorque = brakeTorque;
                ApplyVisualWheel(axle.leftWheel);
                ApplyVisualWheel(axle.rightWheel);
            }
        }
        else
        {
            foreach (Axle axle in axles)
            {
                axle.leftWheel.brakeTorque = 0;
                axle.rightWheel.brakeTorque = 0;
                ApplyVisualWheel(axle.leftWheel);
                ApplyVisualWheel(axle.rightWheel);
            }
        }
    }
    public void ApplyVisualWheel(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
            return;

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
    public void AutoAccelerate(float autoMaxSpeed)
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