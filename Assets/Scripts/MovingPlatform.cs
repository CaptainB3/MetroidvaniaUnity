using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;


    [SerializeField]
    private Vector3 endPositionOffset = new Vector3(10f, 0, 0);

    private Vector3 startPosition;
    private Vector3 targetPosition;

    void Start()
    {
        startPosition = transform.position;
        targetPosition = startPosition + endPositionOffset;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (targetPosition == startPosition)
            {
                targetPosition = startPosition + endPositionOffset;
            }
            else
            {
                targetPosition = startPosition;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 start = (Application.isPlaying) ? startPosition : transform.position;
        Vector3 end = start + endPositionOffset;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireCube(start, Vector3.one * 0.3f);
        Gizmos.DrawWireCube(end, Vector3.one * 0.3f);
    }
}