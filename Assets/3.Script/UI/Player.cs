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
            GameManager.Instance.isPause = true;
            GameManager.Instance.isPlaying = false;
            pauseScreen.SetActive(true);
        }
    }
}
