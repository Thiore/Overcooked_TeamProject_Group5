using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : CounterController
{
    [SerializeField] Transform inwater;
    [SerializeField] Transform outwater;
    public Queue<Plate> InPlate = new Queue<Plate>();
    public Stack<Plate> outPlate = new Stack<Plate>();


    private void Update()
    {
        if (InPlate.Count > 0)
        {
            InPlate.Peek().TryGetComponent(out Plate plate);
            plate.Wash();
            if (!plate.isWash)
            {
                InPlate.Dequeue();
                if(outPlate.Count.Equals(0))
                {
                    ChangePuton();
                }
                outPlate.Push(plate);
                plate.transform.SetParent(outwater.transform);
                plate.transform.position = outwater.transform.position+Vector3.up * (outPlate.Count - 1) *0.08f;
                plate.GetComponent<SphereCollider>().enabled = true;
                
            }
        }
    }

    public bool CheckPos(Transform player)
    {
        float inDis = Vector3.Distance(player.position, inwater.position);
        float outDis = Vector3.Distance(player.position, outwater.position);
        if (inDis < outDis)
            return true;
        else
            return false;
    }


    //Playerstatecotrol에서 접시 놓을때 판별 
    public bool CheckInWaterPlate(GameObject obj)
    {
        if(obj.TryGetComponent(out Plate plate))
        {
            if (!plate.isWash)
            {
                return false;
            }
            else
            {
                InPlate.Enqueue(plate);
                inwater.position -= plate.transform.forward * 0.06f;
                plate.Change_Plate(plate.isWash, eWash.inSink);
                plate.transform.SetParent(inwater);
                plate.transform.position = inwater.position+plate.transform.forward*(InPlate.Count-1)*0.08f;

                return true;
            }
        }

        return false;
    }

    public GameObject GetPlate()
    {
        GameObject obj = null;
        if (outPlate.Count > 0)
        {
            obj = outPlate.Pop().gameObject;
            obj.transform.SetParent(null);
            
            if (outPlate.Count.Equals(0))
            {
                PutOnOb = null;
                ChangePuton();
            }
            else
            {
                PutOnOb = outPlate.Peek().gameObject;
            }
        }
        return obj;
    }

}
