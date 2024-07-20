using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : CounterController
{
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Ingredients"))
        {
            if (this.IsPutOn == true && this.PutOnOb.CompareTag("Ingredients"))
            {
                StartCoroutine(DropTrash_co());
            }
        }
    }


    private IEnumerator DropTrash_co()
    {
        float totaltime = 10f;
        float elapsedtime = 0f;

        

        while (elapsedtime < totaltime)
        {
            this.PutOnOb.transform.localScale *= 0.9f;
            elapsedtime += Time.deltaTime;
            yield return new WaitForSeconds(totaltime);
        }

    }



}
