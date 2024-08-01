using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_CameraAni : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TestCamera testCamera; // ī�޶� ��ũ��Ʈ
    private Animator animator;
    private Vector3 vanPosition;
    // �ִϸ��̼� ���¸� �����ϴ� ����
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
