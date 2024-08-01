using System.Collections;
using UnityEngine;

public class WorldState : MonoBehaviour
{
    [SerializeField] private GameObject stage1; // FlagUIController�� �ִ� �θ� ������Ʈ�� �Ҵ��մϴ�.
    [SerializeField] private Animator cameraAnimator;
    [SerializeField] private TestCamera testCamera; // ī�޶� ��ũ��Ʈ
    [SerializeField] private Animator stage2;
    [SerializeField] private GameObject stage2Object;
    

    private FlagUIController flagController;
    private FlagUIController flagControllerStage2;
    private Animator animator;
    private int bestScore_W;
    private Vector3 vanPosition;

    // �ִϸ��̼� ���¸� �����ϴ� ����
    public bool isCameraAnimationPlaying { get; private set; }

    private void Start()
    {
        testCamera = FindObjectOfType<TestCamera>();
        if (stage1 == null)
        {
            Debug.LogError("stage1 ���� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        flagController = stage1.GetComponentInChildren<FlagUIController>(); // �ڽ� ������Ʈ���� FlagUIController�� ã���ϴ�.
        if (flagController == null)
        {
            Debug.LogError("FlagUIController ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        flagControllerStage2 = stage2Object.GetComponentInChildren<FlagUIController>(); // stage2Object�� �ڽ� ������Ʈ���� FlagUIController�� ã���ϴ�.
        if (flagControllerStage2 == null)
        {
            Debug.LogError("Stage2�� FlagUIController ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        flagController.OnUISet += OnFlagUISet;

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator ������Ʈ�� ã�� �� �����ϴ�!");
        }

        // ���� ȣ��� FlagUIController �ʱ�ȭ�� ��ٸ�
        //StartCoroutine(DelayedResetState());
    }

    private void OnFlagUISet()
    {
        ResetState(1); // UI ���� �Ϸ� �� ResetState ȣ��
    }

    private IEnumerator DelayedResetState()
    {
        // 1�� ��� (1�� �Ŀ��� �̺�Ʈ�� �������� �ʾҴٸ� ResetState�� ȣ��)
        yield return new WaitForSeconds(1);

        if (flagController != null && flagController.bestScore != 0)
        {
            ResetState(1);
        }
    }

    public void SaveState()
    {
        if (animator != null)
        {
            float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            PlayerPrefs.SetFloat("AnimatorState", normalizedTime);
            Debug.Log("�ִϸ��̼� ��� �ð� �����: " + normalizedTime);
        }
        else
        {
            Debug.Log("���忡�� ������ �߻��߽��ϴ�.");
        }
    }

    public void RestoreState()
    {
        float savedTime = PlayerPrefs.GetFloat("AnimatorState", 0);
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).fullPathHash, 0, savedTime);
        Debug.Log("�ִϸ��̼� ��� �ð�: " + savedTime);
    }

    public void ResetState(int stageNumber)
    {
        if (flagController == null)
        {
            Debug.LogError("ResetState ȣ�� �� FlagUIController�� �����ϴ�.");
            return;
        }

        bestScore_W = flagController.bestScore;
        Debug.Log($"ResetState called with bestScore_W: {bestScore_W}");
        if (bestScore_W <= 39)
        {
            SaveState();
            Debug.Log("ResetState �޼��忡 ���Խ��ϴ�.");
        }
        else
        {
            RestoreState();

            HandleStage2Animation();



            Debug.Log("else���� ResetState �޼��忡 ���Խ��ϴ�.");
        }
    }
    private void HandleStage2Animation()
    {
        if (flagControllerStage2 == null)
        {
            Debug.LogError("Stage2�� �÷�����Ʈ�ѷ��� ã�� �� �����ϴ�.");
            return;
        }
        // �ִϸ��̼� ���¸� ���� Ȯ��
        AnimatorStateInfo stateInfo = stage2.GetCurrentAnimatorStateInfo(0);
        bool isStage2AnimationPlaying = stateInfo.IsName("Stage2");

        if (!isStage2AnimationPlaying)
        {
            // Stage2 �ִϸ��̼��� ������� ���� ��쿡�� Ʈ���Ÿ� ����
            stage2.SetTrigger("Stage2_");
        }

        // Stage2 �ִϸ��̼��� �Ϸ�� ���·� �����
        if (flagControllerStage2.bestScore >= 50)
        {
            // �ִϸ��̼��� ������ �̵���Ű�� ���
            stage2.Play("Stage2", 0, 1f);

            
        }
    }
    //�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    //ī�޶� �ִϸ��̼� �޼���
    public void StartCameraAnimation_Stage1()
    {
        if (cameraAnimator != null)
        {
            isCameraAnimationPlaying = true;
            cameraAnimator.SetTrigger("Camera_Stage1");
        }
        else
        {
            Debug.Log("ī�޶� �ִϸ��̼� ��� �ȵ�.");
        }
    }
    public void StartCameraAnimation_Stage2()
    {
        if (cameraAnimator != null)
        {
            isCameraAnimationPlaying = true;
            cameraAnimator.SetTrigger("Camera_Stage2");
        }
    }
    public void OnCameraAnimationFinished()
    { 
    
        Debug.Log("ī�޶� �ִϸ��̼��� �������ϴ�."); // ������� ���� �α� �߰�
        isCameraAnimationPlaying = false;
        if (testCamera != null)
        {
            testCamera.StartFollowingPlayer();
            Debug.Log("ī�޶� �÷��̾ ���󰩴ϴ�."); // ������� ���� �α� �߰�
        }
        else
        {
            Debug.Log("testCamera�� �Ҵ���� �ʾҽ��ϴ�.");
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
    //�ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�
    //Van ������ ����

    public void SaveVanPosition(Vector3 position)
    {
        vanPosition = position;
        Debug.Log("�� ��ġ ���� ��" + vanPosition);
    }

    public Vector3 GetVanPosition()
    {
        return vanPosition;
    }
    
}
