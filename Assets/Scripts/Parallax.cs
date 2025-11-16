using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;

    [SerializeField]
    private float parallaxEffectMultiplier = 0.5f;

    private Vector3 startBackgroundPosition;

    void Start()
    {
        startBackgroundPosition = transform.position;
    }

    void LateUpdate()
    {
        float distanceX = cameraTransform.position.x * parallaxEffectMultiplier;

        transform.position = new Vector3(
            startBackgroundPosition.x + distanceX,
            startBackgroundPosition.y,
            startBackgroundPosition.z
        );
    }
}