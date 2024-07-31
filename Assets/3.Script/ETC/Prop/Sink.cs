using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : CounterController
{
    [SerializeField] Transform inwater;
    [SerializeField] Transform outwater;

    private void Update()
    {
        if (inwater.childCount > 0)
        {
            for (int i = 0; i < inwater.childCount; i++)
            {
                if (inwater.GetChild(i).TryGetComponent(out Plate plate))
                {
                    plate.Wash();                    
                    if(plate.isWash == false)
                    {
                        plate.transform.SetParent(outwater.transform);
                        plate.transform.position = outwater.transform.position;
                        plate.GetComponent<SphereCollider>().enabled = true;
                    }
                }
            }
        }
    }



    //Playerstatecotrol에서 접시 놓을때 판별 
    public bool CheckInWaterPlate(GameObject obj)
    {
        if(obj.TryGetComponent(out Plate plate))
        {
            if (plate != null) 
            {
                if (!plate.isWash)
                {
                    return false;
                }
                else
                {
                    plate.transform.SetParent(inwater);
                    plate.transform.position = inwater.position;
                    plate.Change_Plate(plate.isWash);
                    return true;
                }
            } 
        }

        return false;
    }

}
