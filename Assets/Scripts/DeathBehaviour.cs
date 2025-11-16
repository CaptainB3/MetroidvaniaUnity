using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBehaviour : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Can update behaviour to reopen the menu or something
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
