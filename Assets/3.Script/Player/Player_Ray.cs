using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Ray : MonoBehaviour
{
    #region ����Dash
    /*
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
    */
    #endregion
    [SerializeField] private Transform rayPoint;

    private LayerMask InteractionLayer;

    private void Awake()
    {
        InteractionLayer = LayerMask.GetMask("InteractionProp");
    }

    public Vector3 ShotRayFront()
    {
        RaycastHit hit;
        Vector3 hitPos = rayPoint.position + rayPoint.forward * 3f ;
        Debug.Log("PlayerTrans : " + transform.position);
        Debug.Log(hitPos);
        Debug.DrawRay(rayPoint.position, transform.forward * 3f, Color.red);
        if (Physics.Raycast(rayPoint.position, transform.forward, out hit, 3f, InteractionLayer))
        {
            
            if (hit.collider.gameObject != null)
            {
                hitPos = hit.point;
            }
        }
        
        return hitPos;
    }


}
