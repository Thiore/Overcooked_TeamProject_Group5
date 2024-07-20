using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : CounterController
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ingredeint"))
        {
            if(this.IsPutOn == true || this.transform.GetChild(0).CompareTag("Ingredeint"))
            {

            }
        }
    }


    private IEnumerator DropTrash_co()
    {
        float totaltime = 5f;
        float elapsedtime = 0f;

        while (elapsedtime < totaltime)
        {


            yield return null;
        }

    }



}
