using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    //�ƹ��ų� �ö������� True, �ƴϸ� false
    [SerializeField] private bool isPutOn = false;
    public bool IsPutOn { get => isPutOn; set => isPutOn = value; }

    [SerializeField] private GameObject putonOb;
    public GameObject PutOnOb { get => putonOb; set => putonOb = value; }

    [SerializeField] private GameObject choppingBoard;
    public GameObject ChoppingBoard { get => choppingBoard; }

    private void Awake()
    {
        if (transform.childCount.Equals(1))
        {
            choppingBoard = transform.GetChild(0).gameObject;
        }
    }

    public void ChangePuton()
    {
        isPutOn = !isPutOn;
        
    }

  

}
