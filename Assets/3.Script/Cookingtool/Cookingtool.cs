using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class Cookingtool : MonoBehaviour
{
    public bool isSoup;

    protected bool isCooking;

    protected bool isFinish;

    public bool isPlate = false;

    [SerializeField] protected Animator Soup_Anim;
    public GameObject Soup;

    protected Transform SaveRange = null;//가스레인지에서 떨어진 후 해당 가스레인지를 끄기위해
    protected Ingredient Ingre = null;//냄비가 들고있는 재료

    protected Slider slider = null;
    public void StartCook()
    {
        if(transform.childCount.Equals(2)&&!isFinish&&!isSoup)
        {
            isSoup = true;
            Soup_Anim.SetTrigger("Cook");
            SaveRange = transform.parent.parent;
            return;
        }
           
        
    }
    public virtual void ResetCook(Ingredient ingre)
    {
        isSoup = false;
        isFinish = false;
        if(Soup != null)
            transform.GetChild(0).gameObject.SetActive(true);
        Ingre = ingre;
        Ingre.transform.SetParent(transform);
        Ingre.transform.position = transform.position;
        Ingre.transform.rotation = transform.rotation;
    }


}
