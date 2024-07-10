using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject pauseScreen;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void ResumeGame()
    {
        GameManager.Instance.isPause = false;
        GameManager.Instance.isPlaying = true;
        pauseScreen.SetActive(false);
    }

    private void PauseGame()
    {
        GameManager.Instance.isPause = true;
        GameManager.Instance.isPlaying = false;
        pauseScreen.SetActive(true);
        pauseScreen.GetComponent<PauseMenuController>().enabled = true;
    }
}
