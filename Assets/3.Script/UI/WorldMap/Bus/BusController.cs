using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BusController : MonoBehaviour
{
    private int stage_index;
    private FlagUIController flag_ui;
    private WorldState worldState;

    private void Start()
    {
        worldState = FindObjectOfType<WorldState>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.Contains("Flag"))
        {
            flag_ui = other.gameObject.GetComponent<FlagUIController>();
            flag_ui.ActiveUI();
            stage_index = other.gameObject.GetComponent<Flag>().stage_index;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.Instance.AfterResult = "BSJScene";
                //Debug.Log("여기서 ResetState");
                GameManager.Instance.stage_index = stage_index;
                GameManager.Instance.LoadGame(stage_index);
                worldState.ResetState(1);
                //현재 Van의 position
                worldState.SaveVanPosition(transform.position);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Flag"))
        {
            flag_ui = other.gameObject.GetComponent<FlagUIController>();
            flag_ui.DeActiveUI();
            stage_index = 0;
        }
    }

}
