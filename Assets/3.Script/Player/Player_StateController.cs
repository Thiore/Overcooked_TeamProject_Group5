using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StateController : MonoBehaviour
{
    private Animator animator;
    private GameObject PickOB;
    [SerializeField] private Transform Attachtransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    //��� �ڽ� �����ؼ� ��� ������ �뵵
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space) && other.transform.CompareTag("Crate"))
        {
            var ani = other.GetComponent<Animator>();
            if (ani != null)
            {
                ani.SetTrigger("Pick");
            }
        }
    }

    //��� ���� �뵵 
    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKeyDown(KeyCode.Space) && collision.transform.CompareTag("Ingredients"))
        {
            PickOB = collision.gameObject;
            Destroy(PickOB.transform.GetComponent<Rigidbody>());
            animator.SetBool("IsHold", true);
            PickOB.transform.SetParent(Attachtransform);
            PickOB.transform.rotation = Attachtransform.rotation;
            PickOB.transform.position = Attachtransform.position;
            
           
        }

        if (PickOB != null)
        {
            if(Input.GetKeyDown(KeyCode.I))
            {
                animator.SetBool("IsHold", false);
                PickOB.transform.SetParent(null);
            }
        }
    }


}
