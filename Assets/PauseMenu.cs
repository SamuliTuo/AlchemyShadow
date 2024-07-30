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

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            GameManager.Instance.PausedTheGame();
            Pause();
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
        GameManager.Instance.UnpausedTheGame();
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
