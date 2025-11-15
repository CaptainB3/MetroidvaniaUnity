using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "OpeningRoom":
                    switch (gameObject.tag)
                    {
                        case "Door1":
                            LoadScene("SecondRoom");
                            break;
                        case "Door2":
                            LoadScene("ThirdRoom");
                            break;
                    }
                    break;
                case "SecondRoom":
                    LoadScene("OpeningRoom");
                    break;
                case "ThirdRoom":
                    LoadScene("OpeningRoom");
                    break;
                default:
                    break;
            }
        }
    }

    private async void LoadScene(string name)
    {
        await SceneManager.LoadSceneAsync(name);
        Execute(name);
    }
    private void Execute(string name)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }
}