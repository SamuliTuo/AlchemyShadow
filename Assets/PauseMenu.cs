using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    private void Update()
    {
        if (!GameManager.Instance.playerIsAlive)
        {
            return;
        }

        if (GameManager.Instance.paused == false && Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.PauseTheGame();
            Pause();
        }
        else if (GameManager.Instance.paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Unpause();
        }
    }
    
    private void Start()
    {
        pausePanel.SetActive(false);
    }


    public void Pause()
    {
        pausePanel.SetActive(true);
    }
    public void Unpause()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.UnpauseTheGame();
    }

    // Pause buttons:
    public void Continue()
    {
        Unpause();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
