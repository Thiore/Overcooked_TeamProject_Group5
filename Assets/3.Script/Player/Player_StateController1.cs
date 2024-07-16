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
    private Coroutine coroutine;

    private bool isEmission = false;
    public bool IsEmission { get => isEmission; set => isEmission = value; }

    private Ingredients_EmissionController ingre_controller;
    private EmissionController emissionController;

    private void Awake()
    {
        ingre_controller = GetComponent<Ingredients_EmissionController>();
        emissionController = GetComponent<EmissionController>();
        animator = GetComponent<Animator>();
    }

    //�½����� �ȿ��� �±׷� �������� ó��
    private void OnTriggerStay(Collider other)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(PlayerStateChange(other.gameObject));
        }
    }


    private IEnumerator PlayerStateChange(GameObject gameObject)
    {
        // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
        if (Input.GetKey(KeyCode.Space))
        {
            // ��Ḧ ����������
            if (isBellow)
            {
                DropObject();
                isBellow = false;
                yield return new WaitForSeconds(0.5f);
            }
            else   // ���� ���� ���� 
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
                    isBellow = true;
                    yield return new WaitForSeconds(0.5f);
                }
            }

            coroutine = null;
        }


        //�丮���� ��ȣ�ۿ� 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCoroutineRunning = true;
        }


    }



    private void DropObject()
    {
        if (PickOB != null)
        {
            animator.SetBool("IsTake", false);
            if (emissionController.IsPutOn)
            {
                Debug.Log("ǲ��");
                var counter = emissionController.ReadyPutCounter();
                if (counter != null && emissionController.PutOnReady())
                {
                    PickOB.transform.SetParent(counter.transform);
                    PickOB.transform.position = counter.transform.position;
                    PickOB.transform.rotation = Quaternion.identity;
                    emissionController.IsPutOn = false;
                }
            }
            else
            {
                Debug.Log("ǲ�ƴ�");
                PickOB.transform.SetParent(null);
                var rb = PickOB.gameObject.AddComponent<Rigidbody>();
                rb.mass = 10;

            }
            PickOB = null;
        }
    }

    private void TakeHandObject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        PickOB = gameObject;
        Destroy(PickOB.transform.GetComponent<Rigidbody>());
        ingre_controller.ByeObeject(PickOB);
        PickOB.transform.SetParent(Attachtransform);
        PickOB.transform.rotation = Attachtransform.rotation;
        PickOB.transform.position = Attachtransform.position;
    }

}
