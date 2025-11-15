using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        Debug.Log("Door opened!");

        GetComponent<Collider2D>().enabled = false;
    }
}
