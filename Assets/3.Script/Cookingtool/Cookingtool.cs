using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ICookingtool
{
    bool CookedCheck(GameObject gameObject);
    void StartCook();
}

public class Cookingtool : MonoBehaviour, ICookingtool
{
    public GameObject[] ingredients;
    public List<GameObject> availableList;

    public virtual bool CookedCheck(GameObject obj)
    {
        return false;
    }

    public virtual void StartCook()
    {

    }

}
