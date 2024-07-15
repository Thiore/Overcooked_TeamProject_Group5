using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �÷��̾� ��ȣ�ۿ� (��� / �丮 ���� ��� ��ȣ�ۿ�) 
public class Player_StateController1 : MonoBehaviour
{
    private Animator animator;
    private GameObject PickOB;
    [SerializeField] private Transform Attachtransform;

    private bool isBellow = false;
    private bool isCoroutineRunning = false;

    private bool isEmission = false;
    public bool IsEmission { get => isEmission; set => isEmission = value; }

    private Ingredients_EmissionController ingre_controller;

    private void Awake()
    {
        ingre_controller = GetComponent<Ingredients_EmissionController>();
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
        // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
        if (Input.GetKey(KeyCode.Space))
        {
            isCoroutineRunning = true;

            // ��Ḧ ����������
            if (isBellow)
            {
                DropObject();
                yield return new WaitForSeconds(0.5f);
            }

            // ���� ���� ���� 
            if (!isBellow)
            {
                //��� ���� �տ��� 
                if (gameObject.CompareTag("Crate"))
                {
                    var ani = gameObject.transform.GetComponent<Animator>();
                    if (ani != null)
                    {
                        ani.SetTrigger("Pick");
                        // ������ ��� ������Ʈ �ٷ� ���� �޼ҵ� �߰� �ʿ� 
                    }
                    yield return new WaitForSeconds(0.5f);
                }
                else if (gameObject.CompareTag("Cooker") || gameObject.CompareTag("Ingredients"))
                {
                    TakeHandObject(gameObject);
                    yield return new WaitForSeconds(0.5f);
                }
            }

            isCoroutineRunning = false;
        }


        //�丮���� ��ȣ�ۿ� 
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {

        }


    }



    private void DropObject()
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

    private void TakeHandObject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        isBellow = true;
        PickOB = gameObject;
        Destroy(PickOB.transform.GetComponent<Rigidbody>());
        ingre_controller.ByeObeject(PickOB);
        PickOB.transform.SetParent(Attachtransform);
        PickOB.transform.rotation = Attachtransform.rotation;
        PickOB.transform.position = Attachtransform.position;
    }

}
