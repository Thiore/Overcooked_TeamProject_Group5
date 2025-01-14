using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class Bus_MV : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private PlayerIntput playerInput;
    [SerializeField] private WorldState worldState;
    private AudioSource _audio;
    private Rigidbody player_rb;
    //private Animator animator;
    private Vector3 moveDirection;
    private bool isMoving = false;
    //private Player_Ray playerRay;
    //private bool isJumping = false;
    //private Vector3 jumpvelocity = Vector3.zero;


    

    private void Awake()
    {
        playerInput = GetComponent<PlayerIntput>();
        player_rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
        _audio.Play();
        //playerRay = GetComponent<Player_Ray>();
        //animator = GetComponent<Animator>();
    }

   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Debug.Log("눌림");
            GameManager.Instance.LoadScene("AnotherScene");
        }

        //카메라 애니메이션이 재생 중이라면 이동 막음
        if (worldState != null && worldState.isCameraAnimationPlaying)
        {
            return;
        }
        if (isMoving)
        {
            Walking();
        }



        //Debug.DrawRay(transform.position, transform.forward * 3f, Color.red);
        //if (Input.GetKeyDown(KeyCode.V) && isJumping.Equals(false))
        //{
        //    Jump();
        //}

        
    }

    //private void FixedUpdate()
    //{
       
    //}

    private void Walking()
    {
        if (playerInput.Move_Value != 0 || playerInput.Rotate_Value != 0)
        {
            _audio.UnPause();
            //animator.SetBool(IsWalking, true);
            Move();
        }
        else
        {
            _audio.Pause();
            //animator.SetBool(IsWalking, false);
            player_rb.angularVelocity = Vector3.zero;
            player_rb.velocity = player_rb.velocity * 0.5f;
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
        //Debug.Log("벤 위치 저장");
    }

    public void LoadVanPosition()
    {
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY") && PlayerPrefs.HasKey("PlayerPosZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerPosX");
            float y = PlayerPrefs.GetFloat("PlayerPosY");
            float z = PlayerPrefs.GetFloat("PlayerPosZ");
            transform.position = new Vector3(x, y, z);
            //Debug.Log("플레이어 벤 위치");
        }
        else
        {
            //Debug.Log("플레이어 위치 로드 못 함");
        }
    }
    public void StopMoving()
    {
        isMoving = false; // 이동을 멈춥니다.
    }
    public void StartMoving()
    {
        isMoving = true ; // 이동을 시작합니다.
    }

}
