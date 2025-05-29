using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneUnit : MonoBehaviour
{
    void OnEnable()
    {
        DroneManager.RegisterDrone(transform);
    }

    void OnDisable()
    {
        DroneManager.UnregisterDrone(transform);
    }

    void OnDestroy()
    {
        DroneManager.UnregisterDrone(transform);
    }
}