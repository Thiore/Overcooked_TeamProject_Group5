using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    private Animator Anim;
    [SerializeField] private Transform[] pivot;

    private Player_SwapManager controlPlayer;

    private bool iscontrol = false;

    private void Awake()
    {
        TryGetComponent(out Anim);
    }

    private void Update()
    {
        if (iscontrol)
        {
            float X = Input.GetAxis("Horizontal");
            float Y = Input.GetAxis("Vertical");

            Anim.SetFloat("Horizontal", X);
            Anim.SetFloat("Vertical", Y);
            if (X > 0)
                transform.Translate(new Vector3(X * 2f * Time.deltaTime, 0, Y * 3f * Time.deltaTime));
            else
                transform.Translate(new Vector3(X * 4f * Time.deltaTime, 0, Y * 3f * Time.deltaTime));

            if (Input.GetKeyUp(KeyCode.Space))
            {
                controlPlayer.CurrentPlayer.GetComponent<Player_Movent>().enabled = true;
                controlPlayer.CurrentPlayer.GetComponent<PlayerStateControl>().enabled = true;
                iscontrol = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            controlPlayer.CurrentPlayer.GetComponent<Player_Movent>().enabled = true;
            controlPlayer.CurrentPlayer.GetComponent<PlayerStateControl>().enabled = true;
            iscontrol = false;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(!iscontrol)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                controlPlayer = collision.gameObject.transform.GetComponentInParent<Player_SwapManager>();
                if (controlPlayer.CurrentPlayer != null)
                {
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        controlPlayer.CurrentPlayer.GetComponent<Player_Movent>().enabled = false;
                        controlPlayer.CurrentPlayer.GetComponent<PlayerStateControl>().enabled = false;
                        controlPlayer.AniWalkingSetbool(controlPlayer.CurrentPlayer);
                        iscontrol = true;
                    }
                }

            }
        }
        


    }

    private void LateUpdate()
    {
        float X = transform.position.x;
        float Z = transform.position.z;
        X = Mathf.Clamp(X, pivot[0].position.x, pivot[1].position.x);
        Z = Mathf.Clamp(Z, pivot[0].position.z, pivot[1].position.z);
        transform.position = new Vector3(X, transform.position.y, Z);

    }
}
