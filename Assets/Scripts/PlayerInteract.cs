using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerControls controls;
    private InteractButton currentButton;

    void Awake()
    {
        controls = GetComponent<PlayerControls>();
    }

    void Update()
    {
        if (currentButton != null && controls.interactPressed)
        {
            currentButton.Interact();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        InteractButton button = collision.GetComponent<InteractButton>();
        if (button != null)
        {
            currentButton = button;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        InteractButton button = collision.GetComponent<InteractButton>();
        if (button == currentButton)
        {
            currentButton = null;
        }
    }
}