using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject pauseScreen;

    private void Update()
    {
        if(GameManager.Instance.isPause==true && GameManager.Instance.isPlaying == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.isPause = false;
                GameManager.Instance.isPlaying = true;
                pauseScreen.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.isPause = true;
                GameManager.Instance.isPlaying = false;
                pauseScreen.SetActive(true);
            }
        }
        
    }
}
