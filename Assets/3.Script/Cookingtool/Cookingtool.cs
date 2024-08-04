using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Cookingtool : MonoBehaviour
{
    public bool isSoup;

    protected bool isFinish;

    [SerializeField] protected Animator Soup_Anim;

    protected Transform SaveRange = null;//�������������� ������ �� �ش� ������������ ��������
    protected Ingredient Ingre = null;//���� ����ִ� ���

    public bool StartCook()
    {
        if (!isSoup)
        {
            isSoup = true;
            Soup_Anim.SetTrigger("Cook");
            SaveRange = transform.parent.parent;
            return true;
        }
        else
            return false;
        
    }
    public void ResetCook(Ingredient ingre)
    {
        isSoup = false;
        isFinish = false;
        transform.GetChild(0).gameObject.SetActive(true);
        Ingre = ingre;
        Ingre.Off_Mesh();
    }


}
