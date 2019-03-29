using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour {

    public Rigidbody rb;
    public bool playerControl;
    [SerializeField]
    protected float motorTorque;
    [SerializeField]
    protected float steeringAngle;
    [SerializeField]
    protected float brakeTorque;
    [SerializeField]
    protected float maxSpeed;

    public abstract void Accelerate();
    public abstract void Turn();
    public abstract void Brake();
}
