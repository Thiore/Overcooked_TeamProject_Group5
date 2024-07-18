using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    [SerializeField] Transform rayPoint;

    public bool ShotRayFront(out Vector3 hitPoint)
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(rayPoint.position, rayPoint.forward, out hit, 2f, layerMask))
        {

            if (hit.collider.gameObject != null)
            {
                hitPoint = hit.point;
                return true;
            }
        }
        hitPoint = Vector3.zero;    
        return false;
    }


}
