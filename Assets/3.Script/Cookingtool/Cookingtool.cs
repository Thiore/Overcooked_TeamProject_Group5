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

    protected Transform SaveRange = null;//�������������� ������ �� �ش� ������������ ��������
    protected Ingredient ingre = null;
    public Ingredient Ingre { get { return ingre; } protected set { ingre = value; } }//���� ����ִ� ���

    protected Slider slider = null;
    public void StartCook()
    {
        if(!isFinish&&!isSoup)
        {
            isSoup = true;
            if(Soup_Anim != null)
            {
                Soup_Anim.SetTrigger("Cook");
            }
            
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
