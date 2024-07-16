using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagUIController : MonoBehaviour
{
    private Flag flag;
    private GameObject ui;
    private Canvas flag_canvas;
    private void Awake()
    {
        TryGetComponent(out flag);
        ui=GameObject.Find("UI");



        ui.SetActive(false);
    }
    public void ActiveUI()
    {
        ui.SetActive(true);
    }
    public void DeActiveUI()
    {
        ui.SetActive(false);
    }
    private void SetUI()
    {

    }
}
