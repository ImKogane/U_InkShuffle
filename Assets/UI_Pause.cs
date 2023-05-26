using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Pause : MonoBehaviour
{
    private bool isPause = false;
    [SerializeField] private GameObject pauseCanvas;

    public void SetPause()
    {
        if(isPause && pauseCanvas != null)
        {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            isPause = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            isPause = true;
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuScene");
    }
}
