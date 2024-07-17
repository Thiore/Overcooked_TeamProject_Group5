using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ �÷��̾� ��ȣ�ۿ� (��� / �丮 ���� ��� ��ȣ�ۿ�) 
public class Player_StateController1 : MonoBehaviour
{
    private Animator animator;

    //���� ���� ������Ʈ
    private GameObject nearOb;
    //�� ��ó ī����
    private GameObject nearcounter;
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

    //�½����̷� �÷��̾� ���� ������ �������� ó��
    private void OnTriggerStay(Collider other)
    {
        if (coroutine == null)
        {
            nearOb = nearcontroller.GetNearObject();
            nearcounter = emissionController.GetNearCounter();
            coroutine = StartCoroutine(PlayerStateChange());
        }
    }


    private IEnumerator PlayerStateChange()
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
                if (nearcounter != null) // ī���� ������ Ȯ�� 
                {
                    var counter = nearcounter.transform.GetComponent<CounterController>();

                    // �ڽ� ���� �ƹ��͵� ���ٸ� 
                    if (!counter.IsPutOn)
                    {
                        if (nearcounter.CompareTag("Crate"))
                        {
                            var ani = nearcounter.transform.GetComponent<Animator>();
                            if (ani != null)
                            {
                                ani.SetTrigger("Pick");
                                Debug.Log("����");
                                // ������ ��� ������Ʈ �ٷ� ���� �޼ҵ� �߰� �ʿ� 
                            }
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                    else
                    {
                        if (nearOb != null)
                        {
                            TakeHandObject(nearOb);
                            counter.IsPutOn = false;
                        }
                        yield return new WaitForSeconds(0.5f);
                    }

                }
                else
                {
                    if (nearOb != null)
                    {
                        TakeHandObject(nearOb);
                    }
                    yield return new WaitForSeconds(0.5f);
                }
            }

        }


        //�丮���� ��ȣ�ۿ�
        // ���� ��� 
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (nearcounter != null)
            {
                // ���۸��ϰ� �������� ó���� ��ᰡ?
                animator.SetTrigger("Chop");
            }
        }

        coroutine = null;
    }



    private void DropObject()
    {
        if (HandsOnOb != null)
        {
            if (nearcounter != null)
            {
                var counter = nearcounter.transform.GetComponent<CounterController>();
                if (!counter.IsPutOn)
                {
                    HandsOnOb.transform.SetParent(nearcounter.transform);
                    HandsOnOb.transform.position = nearcounter.transform.position;
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.IsPutOn = true;

                    animator.SetBool("IsTake", false);
                    HandsOnOb = null;
                    isHolding = false;
                }
                else
                {
                    Debug.Log("�̹̿ö�����");
                }
            }
            else
            {
                HandsOnOb.transform.SetParent(null);
                var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
                rb.mass = 10;
                rb.angularDrag = 0;
                animator.SetBool("IsTake", false);
                HandsOnOb = null;
                isHolding = false;
            }
        }
    }

    private void TakeHandObject(GameObject gameObject)
    {
        animator.SetBool("IsTake", true);
        HandsOnOb = gameObject;
        nearcontroller.ChangeOriginEmission(HandsOnOb);
        Destroy(HandsOnOb.transform.GetComponent<Rigidbody>());
        HandsOnOb.transform.SetParent(Attachtransform);
        HandsOnOb.transform.rotation = Attachtransform.rotation;
        HandsOnOb.transform.position = Attachtransform.position;
        isHolding = true;
    }


}
