using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : CounterController
{
   


    public IEnumerator DropTrash_co()
    {
        float totaltime = 10f;
        float elapsedtime = 0f;
       
        while (elapsedtime < totaltime)
        {
            this.PutOnOb.transform.localScale *= 0.9f;
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        this.PutOnOb.GetComponent<Ingredient>().Die();
        this.PutOnOb = null;
        this.IsPutOn = false;

    }


    public void DropTrash(GameObject gameObject)
    {
        if(gameObject.transform.CompareTag("Ingredients"))
        {
            
        }
    }


}
