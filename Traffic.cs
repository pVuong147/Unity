using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : MonoBehaviour {

    private List<IAutoPilot> rightLaneVehicles;
    private List<IAutoPilot> leftLaneVehicles;
    float randomMaxSpeed;

    void Start () {

        rightLaneVehicles = new List<IAutoPilot>();
        leftLaneVehicles = new List<IAutoPilot>();

        LoadLane(transform.Find("Left Lane"), leftLaneVehicles);
        LoadLane(transform.Find("Right Lane"), rightLaneVehicles);
    }
	
    private void FixedUpdate()
    {
        foreach (IAutoPilot pilot in rightLaneVehicles)
            pilot.AutoAccelerate(randomMaxSpeed);
        foreach (IAutoPilot pilot in leftLaneVehicles)
            pilot.AutoAccelerate(randomMaxSpeed);
    }
    private void LoadLane(Transform lane, List<IAutoPilot> laneVehicles)
    {
        for (int i = 0; i < lane.childCount; i++)
            laneVehicles.Add(lane.GetChild(i).GetComponent<IAutoPilot>());
    }
}
