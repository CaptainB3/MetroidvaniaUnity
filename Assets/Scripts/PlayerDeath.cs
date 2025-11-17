using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerSpawnManager spawnManager = collision.gameObject.GetComponent<PlayerSpawnManager>();

            if (spawnManager != null)
            {
                if (spawnManager.playerID == PlayerSpawnManager.PlayerID.Player1)
                {
                    if (ScenePosition.hasSavedPlayerPosition1)
                    {
                        collision.gameObject.transform.position = ScenePosition.savedPlayerPosition1;
                    }
                    else
                    {
                        ReloadScene();
                    }
                }
                else
                {
                    if (ScenePosition.hasSavedPlayerPosition2)
                    {
                        collision.gameObject.transform.position = ScenePosition.savedPlayerPosition2;
                    }
                    else
                    {
                        ReloadScene();
                    }
                }
            }
            else
            {
                ReloadScene();
            }
        }
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}