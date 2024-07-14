using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    [SerializeField] Transform rayPoint;

    public Vector3? ShotRayFront()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(rayPoint.position, rayPoint.forward, out hit, 3f, layerMask))
        {

            if (hit.collider.gameObject != null)
            {
                Debug.Log(hit.collider.name);
                return hit.point;
            }
        }
        return null;
    }


}
