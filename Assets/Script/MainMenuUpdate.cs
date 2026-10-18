using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUpdate : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;

    public Image guideImage;
    public Button guide_btn;

    public Button Back;

    void Awake()
    {
         startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        guide_btn.gameObject.SetActive(true);
        guideImage.gameObject.SetActive(false);
    }
    public void Guide()
    {
        startButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        guide_btn.gameObject.SetActive(false);
        guideImage.gameObject.SetActive(true);

    }
    public void BackToMenu()
    {
        startButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        guide_btn.gameObject.SetActive(true);
        guideImage.gameObject.SetActive(false);
    }
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
