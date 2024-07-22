using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneTemplate;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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
    private Coroutine coroutine = null;

    private CounterEmissionController emissionController;
    private NearObject_EmissionController nearcontroller;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        emissionController = GetComponent<CounterEmissionController>();
        nearcontroller = GetComponent<NearObject_EmissionController>();
    }

    private void Update()
    {
        nearcounter = emissionController.GetNearCounter();
        nearOb = nearcontroller.GetNearObject();

        // �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
        if (coroutine == null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                coroutine = StartCoroutine(PlayerHodingChange(nearcounter, nearOb));
            }

            //�丮���� ��ȣ�ۿ�
            // ���� ��� 
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                StartCoroutine(PlayerCookedChage());
            }
        }


        //// �����̽��ٴ� ������ �ִ� �繰���� ���� �ø�(���, �丮����, ����
        //if (coroutine == null)
        //{
        //    if (Input.GetKey(KeyCode.Space))
        //    {
        //        coroutine = StartCoroutine(PlayerHodingChange());
        //    }

        //    //�丮���� ��ȣ�ۿ�
        //    // ���� ��� 
        //    if (Input.GetKeyUp(KeyCode.LeftControl))
        //    {
        //        StartCoroutine(PlayerCookedChage());
        //    }
        //}
    }

    private IEnumerator PlayerHodingChange(GameObject nearCount, GameObject nearob)
    {
        if(nearCount == null && nearob == null)
        {
            yield return null;
        }

        // ��ó ī���Ͱ� �ְ� ���� ���� ���°� �ƴ϶�� 
        if(!isHolding)
        {
            if (nearCount != null)
            {
                //ī���� ���� ������Ʈ�� �ִ��� ������ Ȯ�� 
                var counter = nearCount.transform.GetComponent<CounterController>();

                if (!counter.IsPutOn) //�ö� ���� �ʴٸ� ��ó ������Ʈ����
                {
                    if (counter.CompareTag("Crate"))
                    {
                        var spawn = counter.transform.GetComponent<spawn_Ingredient>();
                        if (spawn != null)
                        {
                            TakeHandObject(spawn.PickAnim());
                            Debug.Log("����");
                            // ������ ��� ������Ʈ �ٷ� ���� �޼ҵ� �߰� �ʿ� 
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                    else
                    {
                        if (nearOb != null)
                        {
                            TakeHandObject(nearob);
                        }
                        yield return new WaitForSeconds(0.3f);
                    }

                }
                else 
                {
                    //ī���� ���� �� �ִ� ������Ʈ�� �ְ�, ������ ������ 
                    if (counter.PutOnOb.CompareTag("Plate") || counter.PutOnOb.CompareTag("Cooker") /* ��ȭ�� �±� �߰� �ʿ� */)
                    {
                        TakeHandObject(counter.PutOnOb);
                    }
                    //���� �� �ִ� ������Ʈ�� ���� ���� ������ 
                    else if (counter.ChoppingBoard != null)
                    {
                        TakeHandObject(counter.ChoppingBoard.transform.GetChild(1).gameObject);
                        counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        TakeHandObject(counter.PutOnOb);
                    }

                    counter.PutOnOb = null;
                    counter.ChangePuton();
                    yield return new WaitForSeconds(0.5f);
                }

            }
            // ��ó ī���Ͱ� ���ٸ�(���ٴ��̰���)
            else
            {
                if(nearOb != null)
                {
                    if (nearob.CompareTag("Plate") || nearob.CompareTag("Cooker"))
                    {
                        TakeHandObject(nearob);
                    }
                    else
                    {
                        TakeHandObject(nearob);
                    }

                    yield return new WaitForSeconds(0.3f);
                }
            }
        }
        else
        {
            DropObject(nearCount, nearob);
            yield return new WaitForSeconds(0.3f);
        }

        coroutine = null;
        yield return new WaitForSeconds(0.3f);
    }

    private void DropObject(GameObject nearcounter, GameObject nearob)
    {
        // ��ó�� ī���Ͱ� �ִٸ� 
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();
            if (!counter.IsPutOn) // ī���Ϳ� �ö����� �ʴٸ�  
            {
                if (counter.ChoppingBoard == null) // ī���Ϳ� ������ ���� ���̶�� 
                {
                    HandsOnOb.transform.SetParent(counter.transform);
                    if (counter.CompareTag("Crate")) // ��� �ڽ� ���� �ƴ϶� ����� �� 
                    {
                        Debug.Log("����");
                        var boxcol = counter.GetComponent<Collider>();
                        var boxtop = boxcol.bounds.center + new Vector3(0, boxcol.bounds.extents.y, 0);
                        HandsOnOb.transform.position = boxtop;
                    }
                    else
                    {
                        Debug.Log("gma");
                        HandsOnOb.transform.position = counter.transform.position + new Vector3(0, 0.1f, 0);
                    }
                    HandsOnOb.transform.rotation = Quaternion.identity;

                }
                else // ������ �ִٸ� 
                {
                    HandsOnOb.transform.SetParent(counter.ChoppingBoard.transform);
                    HandsOnOb.transform.position = counter.ChoppingBoard.transform.position + new Vector3(0, 0.1f, 0);
                    HandsOnOb.transform.rotation = Quaternion.identity;
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                }

                counter.ChangePuton();
                counter.PutOnOb = HandsOnOb;
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





    private IEnumerator PlayerCookedChage()
    {
        if (nearcounter != null)
        {
            var counter = nearcounter.transform.GetComponent<CounterController>();

            // ���۸��ϰ� �������� ó���� ��ᰡ?
            // ������ �ִ���, ���� �ڽĿ� �±װ� ����� ������Ʈ�� �ִ��� + ��� �� �� ���ִ� boolean���� 
            if (counter.ChoppingBoard != null && counter.ChoppingBoard.transform.GetChild(1).gameObject.CompareTag("Ingredients") /* ��ᰡ ����ִ���  */)
            {
                animator.SetTrigger("Chop");
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
                        var spawn = nearcounter.transform.GetComponent<spawn_Ingredient>();
                        if (spawn != null)
                        {
                            TakeHandObject(spawn.PickAnim());
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
                    if (counter.ChoppingBoard != null)
                    {
                        TakeHandObject(counter.ChoppingBoard.transform.GetChild(1).gameObject);
                        counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(true);
                        //���� �� Į�� �ѱ�
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
            else // ī���� ���� �ƴ϶�� ���� �մ°Ŷ� ���� 
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
                    counter.ChoppingBoard.transform.GetChild(0).gameObject.SetActive(false);
                    //������ ���� 
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
