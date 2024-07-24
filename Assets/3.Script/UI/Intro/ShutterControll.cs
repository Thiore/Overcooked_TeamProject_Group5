using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShutterControll : MonoBehaviour
{
    private Animator camera_ani;
    private Animator shutter_ani;
    private Animator notice_ani;
    private Animator chalkboard_ani;
    private Text Shutter_Text;

    private void Awake()
    {
        shutter_ani = GetComponent<Animator>();
        notice_ani = GameObject.Find("Main_Canvas").GetComponent<Animator>();
        chalkboard_ani = GameObject.Find("ChalkBoard").GetComponent<Animator>();
        camera_ani = GameObject.Find("Cameraman").GetComponent<Animator>();
        Shutter_Text = GetComponentInChildren<Text>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("눌림");
            shutter_ani.SetTrigger("Open");
            camera_ani.SetTrigger("GameStart");
            GameManager.Instance.isInputEnabled += 1;
        }
        else
        {
            return;
        }
    }
    private void Change_Text()
    {
        Shutter_Text.text = "플레이해주셔서 감사합니다.";
        notice_ani.SetTrigger("Close");
        chalkboard_ani.SetTrigger("Close");
    }
    private void CameraMoveForward()
    {
        camera_ani.SetTrigger("Zoom");
    }
    private void Active_Etc()
    {
        notice_ani.SetTrigger("Open");
        chalkboard_ani.SetTrigger("Open");
    }
    private void DestoryMySelf()
    {
        this.gameObject.SetActive(false);
    }
    private void Shut_Down()
    {
        Application.Quit();
    }
}