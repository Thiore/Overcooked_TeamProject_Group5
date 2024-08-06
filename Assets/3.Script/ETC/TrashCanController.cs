using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCanController : CounterController
{

    public bool isTrash = false;
    private float totaltime = 2f;
    private float elapsedtime = 0f;

    private void Update()
    {
       if(isTrash)
        {
            elapsedtime += Time.deltaTime;
            PutOnOb.transform.Rotate(Vector3.up, 2f);
            PutOnOb.transform.localScale *= 0.98f;
            if (elapsedtime>totaltime)
            {
                IsPutOn = false;
                PutOnOb.GetComponent<Ingredient>().Die();
                PutOnOb = null;
                isTrash = false;
            }
        }

       
    }


    public void DropTrash(GameObject gameObject)
    {
        if(gameObject.transform.CompareTag("Ingredients"))
        {
            
        }
    }


}
