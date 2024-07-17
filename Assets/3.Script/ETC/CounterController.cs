using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    //아무거나 올라가있으면 True, 아니면 false
    private bool isPutOn = false;
    public bool IsPutOn { get => isPutOn; set => isPutOn = value; }

}
