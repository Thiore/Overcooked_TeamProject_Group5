using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �÷��̾� ��ȣ�ۿ� (��� / �丮 ���� ��� ��ȣ�ۿ�) 
public class Player_StateController : MonoBehaviour
{
    private Animator animator;
    private GameObject PickOB;
    [SerializeField] private Transform Attachtransform;

    public bool isBellow = false;
    private bool isCoroutineRunning = false;

    private bool isEmission = false;
    public bool IsEmission { get => isEmission; set => isEmission = value; }

    private NearObject_EmissionController ingre_controller;

    private void Awake()
    {
        ingre_controller = GetComponent<NearObject_EmissionController>();
        animator = GetComponent<Animator>();
    }

    //�½����� �ȿ��� �±׷� �������� ó��
    private void OnTriggerStay(Collider other)
    {
        if(!isCoroutineRunning) 
        { 
            StartCoroutine(PlayerStateChange(other.gameObject));
        }
    }


    private IEnumerator PlayerStateChange(GameObject gameObject)
    {
        // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮���� ���)
        if (Input.GetKey(KeyCode.Space))
        {
            isCoroutineRunning = true;

            //��� ���� �տ��� 
            if (gameObject.CompareTag("Crate") && !isBellow)
            {
                //var ani = gameObject.transform.GetComponent<Animator>();
                //if (ani != null)
                //{
                //    ani.SetTrigger("Pick");
                //}
                yield return new WaitForSeconds(0.5f);
            }

            // ��Ḧ ����������
            if (isBellow)
            {
                DropIngredients();
                yield return new WaitForSeconds(0.5f);
            }

            // ��Ḧ �鶧
            if (!isBellow && gameObject.CompareTag("Ingredients"))
            {
                TakeIngredients(gameObject);
                yield return new WaitForSeconds(0.5f);
            }

            isCoroutineRunning = false;
        }


        //�丮���� ��ȣ�ۿ� 
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            //�������� 



        }


    }


    //private IEnumerator PlayerStateChange(GameObject gameObject)
    //{
    //    // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        // ��Ḧ ����������
    //        if (isBellow)
    //        {
    //            DropObject();
    //            isBellow = false;
    //            yield return new WaitForSeconds(0.5f);
    //        }
    //        else   // ���� ���� ���� 
    //        {
    //            //��� ���� �տ��� 
    //            if (gameObject.CompareTag("Crate"))
    //            {
    //                var ani = gameObject.transform.GetComponent<Animator>();
    //                if (ani != null)
    //                {
    //                    ani.SetTrigger("Pick");
    //                    // ������ ��� ������Ʈ �ٷ� ���� �޼ҵ� �߰� �ʿ� 
    //                }
    //                yield return new WaitForSeconds(0.5f);
    //            }
    //            else if (gameObject.CompareTag("Cooker") || gameObject.CompareTag("Ingredients"))
    //            {
    //                TakeHandObject(gameObject);
    //                isBellow = true;
    //                yield return new WaitForSeconds(0.5f);
    //            }
    //        }

    //        coroutine = null;
    //    }


    //    //�丮���� ��ȣ�ۿ� 
    //    if (Input.GetKeyDown(KeyCode.LeftControl))
    //    {
    //        isCoroutineRunning = true;
    //    }


    //}



    private void DropIngredients()
    {
        if(PickOB != null)
        {
            animator.SetBool("IsTake", false);
            isBellow = false;
            PickOB.gameObject.AddComponent<Rigidbody>();
            PickOB.transform.SetParent(null);
            PickOB = null;
        }
    }

    //private void DropObject()
    //{
    //    if (PickOB != null)
    //    {
    //        animator.SetBool("IsTake", false);
    //        if (emissionController.IsPutOn)
    //        {
    //            Debug.Log("ǲ��");
    //            var counter = emissionController.ReadyPutCounter();
    //            if (counter != null && emissionController.PutOnReady())
    //            {
    //                PickOB.transform.SetParent(counter.transform);
    //                PickOB.transform.position = counter.transform.position;
    //                PickOB.transform.rotation = Quaternion.identity;
    //                emissionController.IsPutOn = false;
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("ǲ�ƴ�");
    //            PickOB.transform.SetParent(null);
    //            var rb = PickOB.gameObject.AddComponent<Rigidbody>();
    //            rb.mass = 10;

    //        }
    //        PickOB = null;
    //    }
    //}

    private void TakeIngredients(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        isBellow = true;
        PickOB = gameObject;
        Destroy(PickOB.transform.GetComponent<Rigidbody>());
        ingre_controller.ChangeOriginEmission(PickOB);
        PickOB.transform.SetParent(Attachtransform);
        PickOB.transform.rotation = Attachtransform.rotation;
        PickOB.transform.position = Attachtransform.position;
    }

}
