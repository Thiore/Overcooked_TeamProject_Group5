using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_Button_Controll : MonoBehaviour
{
    public void Correct()
    {
        GameManager.Instance.AllCorrect_Recipe();
    }
    public void Incorrect()
    {
        GameManager.Instance.Incorrect_Recipe();
    }
    public void Wrong()
    {
        GameManager.Instance.Wrong_Recipe();
    }
}
