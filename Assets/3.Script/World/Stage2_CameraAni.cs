using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_CameraAni : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TestCamera testCamera; // 카메라 스크립트
    [SerializeField] private Transform vanTransform;
    private Animator animator;
    private Vector3 vanPosition;
    private Bus_MV bus_MV;
    // 애니메이션 상태를 추적하는 변수
    public bool isCameraAnimationPlaying { get; private set; }

    private void Start()
    {
        bus_MV = GetComponent<Bus_MV>();
        testCamera = FindObjectOfType<TestCamera>();
        animator = GetComponent<Animator>();
    }

    public void StartCameraAnimation_Stage2()
    {
        if (cameraAnimator != null)
        {
            isCameraAnimationPlaying = true;
            cameraAnimator.SetTrigger("Camera_Stage2");
        }
    }

    public void OnAnimationStart()
    {
        if (testCamera != null)
        {
            testCamera.StopFollowingPlayer();
        }
    }
    public void OnAnimationEnd()
    {
        if (testCamera != null)
        {
            testCamera.StartFollowingPlayer();
        }
    }
    public void VanStop()
    {
        if (vanTransform != null)
        {
            Bus_MV busMV = vanTransform.GetComponent<Bus_MV>();
            if (busMV != null)
            {
                busMV.StopMoving(); // StopMoving 메서드를 추가하여 이동을 멈춥니다.
                Debug.Log("Van의 움직임이 멈췄습니다.");
            }
            else
            {
                Debug.LogWarning("Bus_MV 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("vanTransform이 할당되지 않았습니다.");
        }
    }
    public void VanStart()
    {
        if (vanTransform != null)
        {
            Bus_MV busMV = vanTransform.GetComponent<Bus_MV>();
            if (busMV != null)
            {
                // Bus_MV 스크립트의 Move 메서드를 호출하여 이동을 시작합니다.
                busMV.StartMoving(); // StartMoving 메서드를 추가하여 이동을 시작하도록 합니다.
                Debug.Log("Van의 움직임을 다시 시작합니다.");
            }
            else
            {
                Debug.LogWarning("Bus_MV 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("vanTransform이 할당되지 않았습니다.");
        }
    }
}
