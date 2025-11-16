using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen = false;
    public Sprite openSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        Debug.Log("Door opened!");

        if (spriteRenderer != null && openSprite != null)
        {
            spriteRenderer.sprite = openSprite;
        }

        GetComponent<Collider2D>().enabled = false;
    }
}