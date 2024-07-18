using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    //�ƹ��ų� �ö������� True, �ƴϸ� false
    private bool isPutOn = false;
    public bool IsPutOn { get => isPutOn; set => isPutOn = value; }

    [SerializeField] private GameObject putonOb;
    public GameObject PutOnOb { get => putonOb; set => putonOb = value; }

    public void ChangePuton()
    {
        isPutOn = !isPutOn;
    }

    public void PutOnObject(GameObject obj)
    {
        putonOb = obj;
    }

}
