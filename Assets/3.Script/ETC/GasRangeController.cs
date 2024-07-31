using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasRangeController : CounterController
{

    [SerializeField] private GameObject fire;
    private bool isfire = false;

    public void ChangeFire()
    {
        isfire = !isfire;

        fire.SetActive(isfire);
    }




}
