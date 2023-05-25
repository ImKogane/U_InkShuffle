using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_EndScreen : MonoBehaviour
{

    [SerializeField] private Image gameOverImage;
    [SerializeField] private Image winImage;

    [SerializeField] string mainMenuSceneName;

    public void ShowGameOver()
    {
        gameOverImage.gameObject.SetActive(true);
        winImage.gameObject.SetActive(false);
    }

    public void ShowWin()
    {
        gameOverImage.gameObject.SetActive(false);
        winImage.gameObject.SetActive(true);
    }

    public void ButtonReplay()
    {
        Time.timeScale = 1;

        var scene = SceneManager.GetActiveScene();
        string sceneName = scene.name;
        SceneManager.LoadScene(sceneName);   
    }

    public void ButtonMenu()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
