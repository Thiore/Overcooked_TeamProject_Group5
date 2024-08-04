using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCounter
{
    counter = 0,
    GasRange,
    GasStation,
    TrashCan,
    Pass,
    ChoppingBoard,
    Crate,
    Sink,
    Plate_Return
}

public class CounterController : MonoBehaviour
{
    //아무거나 올라가있으면 True, 아니면 false
    [SerializeField] private bool isPutOn;
    public bool IsPutOn { get => isPutOn; set => isPutOn = value; }

    [SerializeField] private GameObject putonOb = null;
    public GameObject PutOnOb { get => putonOb; set => putonOb = value; }

    [SerializeField] private GameObject choppingBoard = null;
    public GameObject ChoppingBoard { get => choppingBoard; }

    [SerializeField] private eCounter counter;
    public eCounter Counter { get; private set; }

    

    public void ChangePuton()
    {
        isPutOn = !isPutOn;
        
    }

  

}
