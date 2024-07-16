using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusController : MonoBehaviour
{
    private FlagUIController flag_ui;
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name.Contains("Flag"))
        {
            flag_ui = col.gameObject.GetComponent<FlagUIController>();
            flag_ui.ActiveUI();
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
        }
    }
}
