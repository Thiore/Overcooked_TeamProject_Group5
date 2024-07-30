using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    private Animator Anim;
    [SerializeField] private Transform[] pivot;

    private Player_StateController player1;
    private Player_StateController player2;

    private void Awake()
    {
        TryGetComponent(out Anim);
    }

    private void Update()
    {
        if(player1 != null || player2 != null)
        {
            //아래꺼 위에 넣고 joystick 조종할때 사용하는 bool변수 사용해주시면됩니다.
        }
        float X = Input.GetAxis("Horizontal");
        float Y = Input.GetAxis("Vertical");

        Anim.SetFloat("Horizontal", X);
        Anim.SetFloat("Vertical", Y);
        if(X>0)
            transform.Translate(new Vector3(X*2f*Time.deltaTime, 0, Y*3f*Time.deltaTime));
        else
            transform.Translate(new Vector3(X*4f*Time.deltaTime, 0, Y*3f*Time.deltaTime));
        

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (player1 == null)
            {
                collision.transform.TryGetComponent(out player1);
            }
            else
            {
                collision.transform.TryGetComponent(out player2);
            }

        }
       
            
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.gameObject == player1.gameObject)
            {
                player1 = null;
            }
            else
            {
                player2 = null;
            }
        }
    }

    private void LateUpdate()
    {
        float X = transform.position.x;
        float Z = transform.position.z;
        X = Mathf.Clamp(X, pivot[0].position.x, pivot[1].position.x );
        Z = Mathf.Clamp(Z, pivot[0].position.z, pivot[1].position.z);
        transform.position = new Vector3(X, transform.position.y, Z);
        
    }
}
