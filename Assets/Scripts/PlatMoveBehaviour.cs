using System.Runtime.InteropServices;
using UnityEngine;

public class PlatMoveBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    private Vector3 target;
    void Start()
    {
        target = GameObject.Find("PointB").transform.position;
    }
    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);

        if (transform.position == GameObject.Find("PointB").transform.position)
        {
            target = GameObject.Find("PointA").transform.position;
        } 
        else if (transform.position == GameObject.Find("PointA").transform.position)
        {
            target = GameObject.Find("PointB").transform.position;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Player").transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Player").transform.parent = null;
        }
    }
}
