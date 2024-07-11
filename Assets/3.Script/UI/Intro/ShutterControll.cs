using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterControll : MonoBehaviour
{
    private Animator camera_ani;
    private GameObject shutter;
    private Animator shutter_ani;

    private void Awake()
    {
        shutter = GetComponent<GameObject>();
        shutter_ani = GetComponent<Animator>();
        camera_ani=GameObject.Find("Cameraman").GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("´­¸²");
            shutter_ani.SetTrigger("Open");
            camera_ani.SetTrigger("GameStart");
        }
        else 
        {
            return;
        }
    }
}
