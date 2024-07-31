using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    //아무거나 올라가있으면 True, 아니면 false
    [SerializeField] private bool isPutOn = false;
    public bool IsPutOn { get => isPutOn; set => isPutOn = value; }

    [SerializeField] private GameObject putonOb = null;
    public GameObject PutOnOb { get => putonOb; set => putonOb = value; }

    [SerializeField] private GameObject choppingBoard = null;
    public GameObject ChoppingBoard { get => choppingBoard; }

    private void Awake()
    {
        if (transform.childCount.Equals(1) && !transform.CompareTag("Crate")
            && !transform.CompareTag("GasRange"))
        {
            choppingBoard = transform.GetChild(0).gameObject;
        }
    }

    public void ChangePuton()
    {
        isPutOn = !isPutOn;
        
    }

  

}
