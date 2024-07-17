using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubPanelButton : MonoBehaviour
{
    public void WorldMapLoad()
    {
        SceneManager.LoadScene("Bus_Flag");
    }
    public void MainGameLoad()
    {
        SceneManager.LoadScene("MangoScene");
    }
}
