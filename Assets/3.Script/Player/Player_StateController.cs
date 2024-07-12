using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateController : MonoBehaviour
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

    //��� �ڽ� �����ؼ� ��� ������ �뵵
    private void OnTriggerStay(Collider other)
    {
        if(!isCoroutineRunning) 
        { 
            StartCoroutine(PlayerStateChange(other.gameObject));
        }
    }


    private IEnumerator PlayerStateChange(GameObject gameObject)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isCoroutineRunning = true;

            //��� ���� �տ��� 
            if (gameObject.CompareTag("Crate") && !isBellow)
            {
                var ani = gameObject.transform.GetComponent<Animator>();
                if (ani != null)
                {
                    ani.SetTrigger("Pick");
                }
                yield return new WaitForSeconds(0.5f);
            }

            // ��Ḧ ����������
            if (isBellow)
            {
                animator.SetBool("IsHold", false);
                isBellow = false;
                PickOB.gameObject.AddComponent<Rigidbody>();
                //ingre_controller.PickObject(PickOB);
                PickOB.transform.SetParent(null);
                PickOB = null;
                yield return new WaitForSeconds(0.5f);                
            }

            // ��Ḧ �鶧
            if (!isBellow && gameObject.CompareTag("Ingredients"))
            {
                animator.SetBool("IsHold", true);
                isBellow = true;
                PickOB = gameObject;
                Destroy(PickOB.transform.GetComponent<Rigidbody>());
                ingre_controller.ByeObeject(PickOB);
                PickOB.transform.SetParent(Attachtransform);
                PickOB.transform.rotation = Attachtransform.rotation;
                PickOB.transform.position = Attachtransform.position;
                yield return new WaitForSeconds(0.5f);
            }

            isCoroutineRunning = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {

        }


    }



}
