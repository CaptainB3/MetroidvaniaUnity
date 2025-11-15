using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public DoorSensor sensorA;
    public DoorSensor sensorB;
    public DoorController door;

    private bool hasOpened = false;

    void Update()
    {
        if (hasOpened) return;

        if (sensorA.isActivated && sensorB.isActivated)
        {
            door.OpenDoor();
            hasOpened = true;
        }
    }
}
