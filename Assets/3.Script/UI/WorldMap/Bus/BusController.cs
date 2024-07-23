using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusController : MonoBehaviour
{
    private int stage_index;
    private FlagUIController flag_ui;
    private WorldState worldState;

    private void OnCollisionStay(Collision col)
    {
        if (col.gameObject.name.Contains("Flag"))
        {
            flag_ui = col.gameObject.GetComponent<FlagUIController>();
            flag_ui.ActiveUI();
            stage_index = col.gameObject.GetComponent<Flag>().stage_index;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.stage_index = stage_index;
                worldState.ResetState(stage_index);
                GameManager.Instance.LoadGame(stage_index);
                
                
            }
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
