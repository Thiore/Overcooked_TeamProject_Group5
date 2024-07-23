using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate_Return : MonoBehaviour
{
    [SerializeField] private BoxCollider PlateCol;
    
    private Stack<Plate> Plate_Stack = new Stack<Plate>();
    private float PlateY;
    private CounterController counter;
    private void Awake()
    {
        PlateY = PlateCol.bounds.size.y;
        TryGetComponent(out counter);
    }

    public void SetPlate(Plate plate)
    {
        if(Plate_Stack.Count.Equals(0))
        {
            counter.ChangePuton();
        }
        plate.transform.SetParent(transform);
        counter.PutOnOb = plate.gameObject;
        plate.transform.position = Vector3.zero + Vector3.up * (Plate_Stack.Count - 1) * PlateY;
        plate.gameObject.SetActive(true);
        Plate_Stack.Push(plate);
    }

    public GameObject GetPlate()
    {
        GameObject obj = null;
        if (Plate_Stack.Count>0)
        {
            obj = Plate_Stack.Pop().gameObject;
            obj.transform.SetParent(null);
            counter.PutOnOb = obj;
            if(Plate_Stack.Count.Equals(0))
            {
                counter.ChangePuton();
            }
        }
        return obj;
    }
   

}


