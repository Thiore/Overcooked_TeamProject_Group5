using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Player_Movent : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 540f;
    [SerializeField] private PlayerIntput playerInput;
    private PlayerStateControl playerStateController;

    private Rigidbody player_rb;
    private Animator animator;
    private Vector3 moveDirection;
    [SerializeField] private Image directionArrow;

    private Player_Ray playerRay;
    private bool isJumping = false;
    private bool isRotation = false;

    private readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Awake()
    {
        playerInput = GetComponent<PlayerIntput>();
        player_rb = GetComponent<Rigidbody>();
        playerRay = GetComponent<Player_Ray>();
        animator = GetComponent<Animator>();
        TryGetComponent(out playerStateController);
    }

    private void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward * 2f, Color.red);
        if (Input.GetKeyDown(KeyCode.V) && isJumping.Equals(false) && !isRotation)
        {
            Jump();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && isRotation)
        {
            isRotation = false;
            directionArrow.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && (playerStateController.HandsOnObGet != null && playerStateController.HandsOnObGet.CompareTag("Ingredients")))
        {
            //�����ϴ�(ȸ�����ϴ�)
            isRotation = true;
            animator.SetBool(IsWalking, false);
            directionArrow.gameObject.SetActive(true);
        }

        ThrowRoration();
        if (!isRotation&&!isJumping)
        {
            Walking();
        }
    }

    private void FixedUpdate()
    {
        

        player_rb.angularVelocity = Vector3.zero;
        if(!isJumping)
            player_rb.velocity = new Vector3(0f, -5f, 0f);

    }

    private void ThrowRoration()
    {
        if (isRotation)
        {           
            moveDirection = new Vector3(playerInput.Rotate_Value, 0, playerInput.Move_Value).normalized * moveSpeed * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

                // ���� ���� �̻� ���̳��� ��쿡�� ȸ��
                if (angleDifference > 0.1f)
                {
                    float step = rotateSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
                }
            }
        }

    }

    private void Walking()
    {
        if (playerInput.Move_Value != 0 || playerInput.Rotate_Value != 0)
        {
            animator.SetBool(IsWalking, true);
            Move();
        }
        else
        {
            animator.SetBool(IsWalking, false);
        }
    }

    private void Move()
    {
        // �̵� �� ȸ���� �ְ� ��
        moveDirection = new Vector3(playerInput.Rotate_Value, 0, playerInput.Move_Value).normalized * moveSpeed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

            // ���� ���� �̻� ���̳��� ��쿡�� ȸ��
            if (angleDifference > 0.1f)
            {
                float step = rotateSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            }

        }

        player_rb.MovePosition(player_rb.position + moveDirection);
    }

    private void Jump()
    {
        StartCoroutine(Jump_co());
    }

    private IEnumerator Jump_co()
    {
        isJumping = true;
        animator.SetBool(IsWalking, true);

        Vector3 endPos = player_rb.position + transform.forward*0.5f;
        float elaspedTime = 0f;

        while (elaspedTime < 0.3f)
        {
            if (playerRay.ShotRayFront(out Vector3 hitPoint))
            {
                if (hitPoint != null)
                {
                    //Debug.Log(hitPoint);
                    var dis = Vector3.Distance(player_rb.position, hitPoint);
                    if (dis < 0.5f)
                    {
                        endPos = player_rb.position;
                    }
                }
            }

            player_rb.AddForce(transform.forward*0.5f, ForceMode.VelocityChange);
           // player_rb.MovePosition(Vector3.Lerp(player_rb.position, endPos, elaspedTime / 0.3f));
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        animator.SetBool(IsWalking, false);
        isJumping = false;
    }

}
