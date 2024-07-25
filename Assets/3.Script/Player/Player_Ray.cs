using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    [SerializeField] Transform rayPoint;

    private void Awake()
    {

    }

    public bool ShotRayFront(out Vector3 hitPoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(rayPoint.position, rayPoint.forward, out hit, 2f))
        {
            if (hit.collider.gameObject != null)
            {
                Debug.Log(hit.collider.gameObject);
                hitPoint = hit.point;
                return true;
            }
        }
        hitPoint = Vector3.zero;
        return false;
    }

}
