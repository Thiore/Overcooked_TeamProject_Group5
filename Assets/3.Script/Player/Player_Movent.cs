using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class Player_Movent : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 380f;
    [SerializeField] private PlayerIntput playerInput;

    private Rigidbody player_rb;
    private Animator animator;
    private Vector3 moveDirection;

    private bool isJumping = false;

    private void Awake()
    {
        playerInput = GetComponent<PlayerIntput>();
        player_rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Walking();

        if (Input.GetKeyDown(KeyCode.V) && isJumping.Equals(false))
        {
            Jump();
        }

    }

    private void Walking()
    {
        if (playerInput.Move_Value != 0 || playerInput.Rotate_Value != 0)
        {
            animator.SetBool("IsWalking", true);
            Move();
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }

    private void Move()
    {
        // �̵� �� ȸ���� �ְ� ��
        moveDirection = new Vector3(playerInput.Rotate_Value, 0, playerInput.Move_Value) * moveSpeed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

            // ���� ���� �̻� ���̳��� ��쿡�� ȸ��
            if (angleDifference > 0.3f)
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
        animator.SetBool("IsWalking", true);

        Vector3 endPos = player_rb.position + transform.forward * 3f;
        float elaspedTime = 0f;

        while (elaspedTime < 0.15f)
        {
            player_rb.MovePosition(Vector3.Lerp(player_rb.position, endPos, elaspedTime / 0.15f));
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        player_rb.MovePosition(endPos);
        animator.SetBool("IsWalking", false);
        isJumping = false;
    }

}
