using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    private GameObject returnob;

    public GameObject ReturnOb { get => returnob; }

    public void ShotRay()
    {
        StartCoroutine(ShotRay_co());
    }

    private IEnumerator ShotRay_co()
    {
        while (true)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
                if (hit.collider != null)
                {
                    returnob = hit.collider.gameObject;
                    yield return null;
                }
            }
            yield return null;
        }
    }


}
