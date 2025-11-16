using UnityEngine;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour
{
    [Header("Assign Sensors OR Buttons (or both)")]
    public List<DoorSensor> sensors = new List<DoorSensor>();
    public List<InteractButton> buttons = new List<InteractButton>();

    [SerializeField] private int requiredActivations = 2;
    public DoorController door;

    private bool hasOpened = false;

    void Update()
    {
        if (hasOpened) return;

        int activatedCount = 0;

        // Check sensors
        foreach (DoorSensor sensor in sensors)
        {
            if (sensor != null && sensor.isActivated)
                activatedCount++;
        }

        // Check buttons
        foreach (InteractButton button in buttons)
        {
            if (button != null && button.IsActivated())
                activatedCount++;
        }

        if (activatedCount >= requiredActivations)
        {
            door.OpenDoor();
            hasOpened = true;
        }
    }
}