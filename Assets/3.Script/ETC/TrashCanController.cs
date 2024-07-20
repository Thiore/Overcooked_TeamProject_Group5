using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : CounterController
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ingredients"))
        {
           
        }   
    }



}
