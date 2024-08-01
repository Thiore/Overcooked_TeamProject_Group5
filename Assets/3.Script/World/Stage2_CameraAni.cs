using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_CameraAni : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TestCamera testCamera; // ī�޶� ��ũ��Ʈ
    [SerializeField] private Transform vanTransform;
    private Animator animator;
    private Vector3 vanPosition;
    private Bus_MV bus_MV;
    // �ִϸ��̼� ���¸� �����ϴ� ����
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
                busMV.StopMoving(); // StopMoving �޼��带 �߰��Ͽ� �̵��� ����ϴ�.
                Debug.Log("Van�� �������� ������ϴ�.");
            }
            else
            {
                Debug.LogWarning("Bus_MV ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("vanTransform�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
    public void VanStart()
    {
        if (vanTransform != null)
        {
            Bus_MV busMV = vanTransform.GetComponent<Bus_MV>();
            if (busMV != null)
            {
                // Bus_MV ��ũ��Ʈ�� Move �޼��带 ȣ���Ͽ� �̵��� �����մϴ�.
                busMV.StartMoving(); // StartMoving �޼��带 �߰��Ͽ� �̵��� �����ϵ��� �մϴ�.
                Debug.Log("Van�� �������� �ٽ� �����մϴ�.");
            }
            else
            {
                Debug.LogWarning("Bus_MV ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning("vanTransform�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
