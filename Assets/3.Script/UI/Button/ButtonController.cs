using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject pauseScreen;

    public void Keep_Click()
    {
        ResumeGame();
    }

    public void Restart_Click()
    {
        GameManager.Instance.LoadScene(0);
        pauseScreen.SetActive(false);
    }

    public void Quit_Click()
    {
        GameManager.Instance.LoadScene(1);
        pauseScreen.SetActive(false);
    }

    private void ResumeGame()
    {
        GameManager.Instance.isPause = false;
        GameManager.Instance.isPlaying = true;
        pauseScreen.SetActive(false);
    }
}
