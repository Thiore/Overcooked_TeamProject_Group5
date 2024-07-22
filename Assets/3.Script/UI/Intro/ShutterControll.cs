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
        camera_ani = GameObject.Find("Cameraman").GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("����");
            shutter_ani.SetTrigger("Open");
            camera_ani.SetTrigger("GameStart");
            GameManager.Instance.isInputEnabled +=1;
        }
        else
        {
            return;
        }
    }
    private void DestoryMySelf()
    {
        Destroy(gameObject);
    }
}