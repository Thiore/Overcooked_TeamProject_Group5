using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class Bus_MV : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 380f;
    [SerializeField] private PlayerIntput playerInput;
    [SerializeField] private WorldState worldState;

    private Rigidbody player_rb;
    //private Animator animator;
    private Vector3 moveDirection;

    //private Player_Ray playerRay;
    //private bool isJumping = false;
    //private Vector3 jumpvelocity = Vector3.zero;


    

    private void Awake()
    {
        playerInput = GetComponent<PlayerIntput>();
        player_rb = GetComponent<Rigidbody>();
        //playerRay = GetComponent<Player_Ray>();
        //animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //카메라 애니메이션이 재생 중이라면 이동 막음
        if (worldState != null && worldState.isCameraAnimationPlaying)
        {
            return;
        }
        Walking();

        //Debug.DrawRay(transform.position, transform.forward * 3f, Color.red);
        //if (Input.GetKeyDown(KeyCode.V) && isJumping.Equals(false))
        //{
        //    Jump();
        //}

        player_rb.angularVelocity = Vector3.zero;
        player_rb.velocity = Vector3.zero;

    }

    private void Walking()
    {
        if (playerInput.Move_Value != 0 || playerInput.Rotate_Value != 0)
        {
            //animator.SetBool(IsWalking, true);
            Move();
        }
        else
        {
            //animator.SetBool(IsWalking, false);
        }
    }

    private void Move()
    {
        // 이동 전 회전을 주고 비교
        moveDirection = new Vector3(playerInput.Rotate_Value, 0, playerInput.Move_Value) * moveSpeed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            float angleDifference = Quaternion.Angle(transform.rotation, targetRotation);

            // 일정 각도 이상 차이나는 경우에만 회전
            if (angleDifference > 0.3f)
            {
                float step = rotateSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
            }

        }

        player_rb.MovePosition(player_rb.position + moveDirection);

    }

    //private void Jump()
    //{
    //    StartCoroutine(Jump_co());
    //}

    //private IEnumerator Jump_co()
    //{
    //    isJumping = true;
    //    //animator.SetBool(IsWalking, true);

    //    #region 정문Dash
    //    /*
    //    Vector3 endPos = player_rb.position + transform.forward * 2f;
    //    float elaspedTime = 0f;

    //    while (elaspedTime < 0.3f)
    //    {    
    //        if(playerRay.ShotRayFront(out Vector3 hitPoint))
    //        {
    //            endPos = hitPoint - transform.forward * 0.2f;
    //            endPos.y = 0f;
    //            break;
    //        }

    //        player_rb.MovePosition(Vector3.Lerp(player_rb.position, endPos,elaspedTime / 0.3f));
    //        elaspedTime += Time.deltaTime;
    //        yield return null;
    //    }
    //    player_rb.MovePosition(endPos);     
    //    */
    //    #endregion

    //    //#region 영훈Dash
    //    //Vector3 EndPos = playerRay.ShotRayFront();

    //    //float DashDistance = Vector3.Distance(transform.position, EndPos);
    //    //while (DashDistance > 0.5f)
    //    //{
    //    //    Debug.Log(DashDistance);
    //    //    player_rb.MovePosition(player_rb.position + transform.forward * moveSpeed * 1.3f * Time.deltaTime);
    //    //    DashDistance = Vector3.Distance(transform.position, EndPos);
    //    //    yield return null;
    //    //}

    //    //#endregion

    //    //animator.SetBool(IsWalking, false);
    //    isJumping = false;
    //}
    public void SaveVanPosition()
    {
        Vector3 playerPosition = transform.position;
        PlayerPrefs.SetFloat("PlayerPosX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPosZ", playerPosition.z);
        //저장
        PlayerPrefs.Save();
        Debug.Log("벤 위치 저장");
    }

    public void LoadVanPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            transform.position = new Vector3(x, y, z);
            Debug.Log("플레이어 벤 위치");
        }
        else
        {
            Debug.Log("플레이어 위치 로드 못 함");
        }
    }

}
