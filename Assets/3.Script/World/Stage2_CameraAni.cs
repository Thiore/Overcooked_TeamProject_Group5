using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_CameraAni : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TestCamera testCamera; // 카메라 스크립트
    private Animator animator;
    private Vector3 vanPosition;
    // 애니메이션 상태를 추적하는 변수
    public bool isCameraAnimationPlaying { get; private set; }

    private void Start()
    {
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
}
