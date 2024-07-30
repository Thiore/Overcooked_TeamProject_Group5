using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus_Ani : MonoBehaviour
{
    public GameObject inflatableObject;
    public GameObject propellerObject;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Active_t();
        }
        else
            Active_f();
    }

    public void Active_t()
    {
        inflatableObject.SetActive(true);
        propellerObject.SetActive(true);

    }
    public void Active_f()
    {
        inflatableObject.SetActive(false);
        propellerObject.SetActive(false);
    }

}
