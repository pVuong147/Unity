using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAutoPilot {

    void AutoAccelerate(float maxMotorTorque);
    void AutoTurn(float steerAngle);
    void AutoBrake(float maxBrakeTorque);
}
