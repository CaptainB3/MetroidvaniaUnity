using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    public bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = true;
        }

        else isActivated = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isActivated = false;
            return;
        }
    }
}
