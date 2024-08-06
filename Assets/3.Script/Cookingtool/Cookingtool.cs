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

    [SerializeField] protected Animator Soup_Anim;

    protected Transform SaveRange = null;//�������������� ������ �� �ش� ������������ ��������
    protected Ingredient Ingre = null;//���� ����ִ� ���

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
    public void ResetCook(Ingredient ingre)
    {
        isSoup = false;
        isFinish = false;
        transform.GetChild(0).gameObject.SetActive(true);
        Ingre = ingre;
        Ingre.transform.SetParent(transform);
        Ingre.transform.position = transform.position;
        Ingre.transform.rotation = transform.rotation;
    }


}
