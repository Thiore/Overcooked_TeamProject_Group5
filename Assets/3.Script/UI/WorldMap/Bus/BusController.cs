using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusController : MonoBehaviour
{
    private int stage_index;
    private FlagUIController flag_ui;
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains("Flag"))
        {
            flag_ui = col.gameObject.GetComponent<FlagUIController>();
            flag_ui.ActiveUI();
            stage_index = col.gameObject.GetComponent<Flag>().stage_index;
        }
        else
        {
            return;
        }
    }
    private void OnCollisionExit(Collision col)
    {
        if (col.gameObject.name.Contains("Flag"))
        {
            flag_ui = col.gameObject.GetComponent<FlagUIController>();
            flag_ui.DeActiveUI();
            stage_index = 0;
        }
    }
}
