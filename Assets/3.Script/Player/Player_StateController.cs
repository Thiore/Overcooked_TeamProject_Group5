using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

//������ �÷��̾� ��ȣ�ۿ� (��� / �丮 ���� ��� ��ȣ�ۿ�) 
public class Player_StateController : MonoBehaviour
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
    private bool isChop = false;

    public bool IsHolding { get => isHolding; private set => isHolding = value; }
    public bool IsChop { get => isChop; private set => isChop = value; }

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
    private void Update()
    {
        nearcounter = emissionController.GetNearCounter();
        nearOb = nearcontroller.GetNearObject();

        // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
        if (Input.GetKey(KeyCode.Space))
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine(PlayerHodingChange());
            }
        }

        //�丮���� ��ȣ�ۿ�
        // ���� ��� 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            StartCoroutine(PlayerCookedChage());
        }
    }

    private IEnumerator PlayerCookedChage()
    {
        if (nearcounter != null)
        {
            // ���۸��ϰ� �������� ó���� ��ᰡ?

            animator.SetTrigger("Chop");
            if(nearcounter.transform.childCount.Equals(2))
            {

            }


        }

        yield return new WaitForSeconds(0.5f);
    }


    private IEnumerator PlayerHodingChange()
    {

        // ��Ḧ ����������
        if (isHolding)
        {
            DropObject();
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
                            yield return new WaitForSeconds(0.2f);
                        }
                    }

                    //�ڽ����� ���� ��ó ������Ʈ�� �ִٸ� 
                    if (nearOb != null)
                    {
                        // �÷��̾ ���� ����� ���ϰ� ��ῡ bool �� �־� true �� �׳� return �� �� �ֵ���
                        TakeHandObject(nearOb);
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else // �ڽ� ���� ������? 
                {
                    if(counter.PutOnOb != null)
                    {
                        TakeHandObject(nearOb);
                        counter.ChangePuton();
                        counter.PutOnOb = null;
                        yield return new WaitForSeconds(0.2f);
                    }
                    else
                    {
                        TakeHandObject(counter.transform.GetChild(0).gameObject);
                    }

                    counter.ChangePuton();
                    counter.PutOnOb = null;
                    yield return new WaitForSeconds(0.2f);

                }

            }
            else
            {
                if (nearOb != null)
                {
                    TakeHandObject(nearOb);
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }

        yield return new WaitForSeconds(0.2f);

        coroutine = null;
    }



    private void DropObject()
    {
        // ��ó�� ī���Ͱ� �ִٸ� 
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();
            if (!counter.IsPutOn) // ī���Ϳ� �ö����� �ʴٸ� 
            {
                if (counter.ChoppingBoard == null) // ī���Ϳ� ������ ���� ���̶�� 
                {
                    HandsOnOb.transform.SetParent(nearcounter.transform);
                    HandsOnOb.transform.position = nearcounter.transform.position + new Vector3(0, 0.1f, 0);
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChangePuton();
                    counter.PutOnOb = HandsOnOb;
                }
                else // ������ �ִٸ� 
                {
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position + new Vector3(0, 0.1f, 0);
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChangePuton();
                }

                animator.SetBool("IsTake", false);
                HandsOnOb = null;
                isHolding = false;
            }
            else // ī���Ϳ� �ö� �ִµ� ����Ϸ��� �ϸ� 
            {
                Debug.Log("�̹̿ö�����");
            }
        }
        else // ��ó�� ī���� ������ ���� �����ٴ� 
        {
            HandsOnOb.transform.SetParent(null);
            var rb = HandsOnOb.gameObject.AddComponent<Rigidbody>();
            rb.mass = 0.05f;
            rb.angularDrag = 0;
            animator.SetBool("IsTake", false);
            HandsOnOb = null;
            isHolding = false;
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
