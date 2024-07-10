using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{



    public IEnumerator ShotRay(System.Action<GameObject> callback)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 2f) )
        {
            Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
            if(hit.collider != null )
            {
                callback(hit.collider.gameObject);
            }
        }
        yield return null;
    }


}
