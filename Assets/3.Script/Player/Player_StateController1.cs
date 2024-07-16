using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �÷��̾� ��ȣ�ۿ� (��� / �丮 ���� ��� ��ȣ�ۿ�) 
public class Player_StateController1 : MonoBehaviour
{
    private Animator animator;

    //���� ���� ������Ʈ
    private GameObject nearOb;
    //���� ���� ������Ʈ 
    private GameObject HandsOnOb;
    //�̰� �ν����Ϳ��� ���� �ؿ� ���̷��� Attach �־� ����ϱ�
    [SerializeField] private Transform Attachtransform;

    //���� ��� �ִ��� 
    private bool isHolding = false;

    //�ڷ�ƾ �迭�� �����ѵ� 
    // Start�� �ҋ� ����� �迭�� ����
    // �� ������Ʈ���� �ϳ��ϳ� �����հ� �÷��̾ �� �� ����?
    private Coroutine coroutine;

    private CounterEmissionController emissionController;
    private NearObject_EmissionController nearcontroller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        emissionController = GetComponent<CounterEmissionController>();
        nearcontroller = GetComponent<NearObject_EmissionController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(PlayerStateChange(other.gameObject));
        }
    }

    //�½����̷� �÷��̾� ���� ������ �������� ó��
    //private void OnTriggerStay(Collider other)
    //{
    //    if (coroutine == null)
    //    {
    //        coroutine = StartCoroutine(PlayerStateChange(other.gameObject));
    //    }
    //}


    private IEnumerator PlayerStateChange(GameObject gameObject)
    {
        if (gameObject != null)
        {
            // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
            if (Input.GetKey(KeyCode.Space))
            {
                // ��Ḧ ����������
                if (isHolding)
                {
                    DropObject();
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
                        if (nearcontroller.GetNearObject() != null)
                        {
                            TakeHandObject(nearcontroller.GetNearObject());
                        }
                        yield return new WaitForSeconds(0.5f);
                    }
                }

                coroutine = null;
            }


            //�丮���� ��ȣ�ۿ� 
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {

            }
        }

    }



    private void DropObject()
    {
        if (HandsOnOb != null)
        {
            animator.SetBool("IsTake", false);
            if (emissionController.IsPutOn)
            {
                //var counter = emissionController.ReadyPutCounter();
                //if (counter != null)
                //{
                //    HandsOnOb.transform.SetParent(counter.transform);
                //    HandsOnOb.transform.position = counter.transform.position;
                //    HandsOnOb.transform.rotation = Quaternion.identity;
                //    emissionController.IsPutOn = false;
                //}
            }
            else
            {
                HandsOnOb.transform.SetParent(null);
                var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
                rb.mass = 10;

            }
            HandsOnOb = null;
            isHolding = false;
        }
    }

    private void TakeHandObject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        HandsOnOb = gameObject;
        Destroy(HandsOnOb.transform.GetComponent<Rigidbody>());
        HandsOnOb.transform.SetParent(Attachtransform);
        HandsOnOb.transform.rotation = Attachtransform.rotation;
        HandsOnOb.transform.position = Attachtransform.position;
        isHolding = true;
    }


}
