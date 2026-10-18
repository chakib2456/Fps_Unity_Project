using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void StartGame()
    {
        Debug.Log("START!");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}
